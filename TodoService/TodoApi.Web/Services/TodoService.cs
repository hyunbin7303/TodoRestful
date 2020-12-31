using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Datasource;
using TodoApi.Model.Todo;
using TodoApi.Query.Interface;
using TodoApi.Infrastructure.Extensions;
using AutoMapper;
using System.Linq.Expressions;
using TodoApi.Query.Interface.DTOs;
using MongoDB.Bson;

namespace TodoApi.Web.Services
{
    public class TodoService : ITodoService
    {
        private readonly IMongoRepository<Todo> _todoRepository;
        private readonly IMapper _mapper;
        public TodoService(IMongoRepository<Todo> todoRepository, IMapper mapper)
        {
            _todoRepository = todoRepository ?? throw new ArgumentException(nameof(todoRepository));
            _mapper = mapper;
        }
        public Task<TodoDTO> GetOne(string todoId)
        {
            Expression<Func<Todo, bool>> todoExpr = x => x.Id.ToString() == todoId;
            var getTodo = _todoRepository.FindOne(todoExpr).ConvertTo();
            return Task.FromResult<TodoDTO>(getTodo);
        }
        public async Task<List<TodoDTO>> GetTodosAsync(GetTodoQuery filter = null, PaginationFilter paginationFilter = null)
        {
            var queryable = _todoRepository.AsQueryable();
            if(paginationFilter == null)
            {
                var check = await queryable.Include(x => x.Tag).ToListAsync();
                return check.ConvertTo().ToList();
            }
            queryable = AddFiltersOnQuery(filter, queryable);
            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            var listTodo = await queryable.Include(x => x.Tag).Skip(skip).Take(paginationFilter.PageSize).ToListAsync();
            return listTodo.ConvertTo().ToList();
        }
        public Task<IEnumerable<TodoDTO>> ListTodoAsync(string userId, GetTodoQuery query)
        {
            var userTodos = _todoRepository.FindByUserId(userId).Result.ToList();
            if (query.Date != null)
                userTodos = userTodos.FindAll(x => x.Datetime == new DateTime(query.Date.Value.Year, query.Date.Value.Month, query.Date.Value.Day));
            if (query.SortByDate)
                userTodos = userTodos.OrderByDescending(x => x.Datetime).ToList();
            if (query.TodoStatus != null)
            {
                userTodos = userTodos.FindAll(x => x.Status == query.TodoStatus);
            }
            bool check = query.TodoStatus != null ? true :  false;
            return Task.FromResult(userTodos.ConvertTo());
        }
        public Task<bool> SaveAsync(CreateTodoDTO createTodoDTO)
        {
            var mappingTest = _mapper.Map<CreateTodoDTO, Todo>(createTodoDTO);
            var check = _todoRepository.InsertOneAsync(mappingTest);
            return Task.FromResult(check.IsCompleted);
        }
        public async Task<UpdateTodoDTO> UpdateAsync(string TodoId, UpdateTodoDTO todo)
        {
            var existingTodo = _todoRepository.FindById(TodoId);
            if (existingTodo == null){
                return await Task.FromResult(todo);
            }
            try
            {
                var mappingTest = _mapper.Map<UpdateTodoDTO, Todo>(todo);
                await _todoRepository.ReplaceOneAsync(mappingTest);
                // await _unitOfWork.CompleteAsync();
                return await Task.FromResult(todo);
            }
            catch (Exception ex)
            {
                //    return new ProductResponse($"An error occurred when updating the product: {ex.Message}");
                return await Task.FromResult(todo);
            }
        }
        public Task<TodoDTO> UpdateSubTodoAsync(string TodoId, UpdateSubTodoTaskDTO subTodo)
        {
            var existingTodo = _todoRepository.FindByIdAsync(TodoId).Result;
            var findSubTodo = existingTodo.TodoTask.ToList().Find(x => x.TodoTaskId == subTodo.TodoTaskId);
            if(findSubTodo == null)
            {
                return null;
            }
            try
            {
                //There is no Replace method for replacing.
                //_todoRepository.ReplaceOne()
                return null;
            }
            catch (Exception)
            {
                // Do some logging stuff.
                return null;
            }
        }
        public Task<IEnumerable<TodoDTO>> ListAll()
        {
            var todoDTOs = _todoRepository.FindAll().Result.ConvertTo();
            return Task.FromResult(todoDTOs);
        }
        public Task<bool> DeleteAsync(string todoId)
        {
            var check = _todoRepository.DeleteByIdAsync(todoId).IsCompleted;
            return Task.FromResult(check);
        }
        public Task<bool> DeleteOneAsync(string todo)
        {
            Expression<Func<Todo, bool>> expr = null; // Should not call Expression in here.
            _todoRepository.DeleteOneAsync(expr);
            return Task.FromResult(true);
        }
        private static IQueryable<Todo> AddFiltersOnQuery(GetTodoQuery filter, IQueryable<Todo> queryable)
        {
            if (!string.IsNullOrEmpty(filter?.UserId))
            {
                queryable = queryable.Where(x => x.UserId == filter.UserId);
            }
            return queryable;
        }
        private async Task AddNewTags(CreateTodoDTO createTodoDTO)
        {
            foreach(var tag in createTodoDTO.Tags)
            {
                // Finding Tags in Repository?....
                // How can I find Tags in MongoDB?
                // Need to run Query(Index) Later.
            }
            /*
             *             foreach (var tag in post.Tags)
            {
                var existingTag =
                    await _dataContext.Tags.SingleOrDefaultAsync(x =>
                        x.Name == tag.TagName);
                if (existingTag != null)
                    continue;

                await _dataContext.Tags.AddAsync(new Tag
                    {Name = tag.TagName, CreatedOn = DateTime.UtcNow, CreatorId = post.UserId});
            }
             * 
             * 
             */

        }

    }
}
