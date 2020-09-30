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
            Expression<Func<Todo, bool>> todoExpr = null;
            var getTodo = _todoRepository.FindOne(todoExpr).ConvertTo();
            throw new NotImplementedException();
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
        private static IQueryable<Todo> AddFiltersOnQuery(GetTodoQuery filter, IQueryable<Todo> queryable)
        {
            if (!string.IsNullOrEmpty(filter?.UserId))
            {
                queryable = queryable.Where(x => x.UserId == filter.UserId);
            }
            return queryable;
        }

        public Task<IEnumerable<TodoDTO>> ListAsync(string userId, GetTodoQuery query)
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
        public Task<TodoDTO> UpdateAsync(string TodoId, Todo todo)
        {
            var existingProduct = _todoRepository.FindByIdAsync(TodoId);
            if (existingProduct == null)
            {
                //return new ProductResponse("Product not found.");
                return null;
            }
            _todoRepository.ReplaceOne(todo);
            return null;
            //var existingCategory = await _categoryRepository.FindByIdAsync(product.CategoryId);
            //if (existingCategory == null)
            //    return new ProductResponse("Invalid category.");
            //existingProduct.Name = product.Name;
            //existingProduct.UnitOfMeasurement = product.UnitOfMeasurement;
            //existingProduct.QuantityInPackage = product.QuantityInPackage;
            //existingProduct.CategoryId = product.CategoryId;
            //try
            //{
            //    _productRepository.Update(existingProduct);
            //    await _unitOfWork.CompleteAsync();

            //    return new ProductResponse(existingProduct);
            //}
            //catch (Exception ex)
            //{
            //    // Do some logging stuff
            //    return new ProductResponse($"An error occurred when updating the product: {ex.Message}");
            //}
        }
        public Task<TodoDTO> UpdateSubTodoAsync(UpdateSubTodoTaskDTO subTodo)
        {
            throw new NotImplementedException();
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
            Expression<Func<Todo, bool>> expr = null;
            _todoRepository.DeleteOneAsync(expr);
            return Task.FromResult(true);
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
