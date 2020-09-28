using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TodoApi.Datasource;
using TodoApi.Model.Todo;
using TodoApi.Query.Interface;
using TodoApi.Infrastructure.AutoMapper;

namespace TodoApi.Web.Services
{
    public class TodoService : ITodoService
    {
        private readonly IMongoRepository<Todo> _todoRepository;
        public TodoService(IMongoRepository<Todo> todoRepository)
        {
            _todoRepository = todoRepository ?? throw new ArgumentException(nameof(todoRepository));
        }
        public Task<TodoDTO> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<TodoDTO> GetOne(string todoId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Todo>> GetTodosAsync(GetTodoQuery filter = null, PaginationFilter paginationFilter = null)
        {
            var queryable = _todoRepository.AsQueryable();
            if(paginationFilter == null)
            {
                var check = await queryable.Include(x => x.Tag).ToListAsync();
                return check;
            }
            queryable = AddFiltersOnQuery(filter, queryable);
            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await queryable.Include(x => x.Tag).Skip(skip).Take(paginationFilter.PageSize).ToListAsync();
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
        public async Task<TodoDTO> SaveAsync(CreateTodoDTO todo)
        {
            //var check =  await _todoRepository.InsertOneAsync(todo);
            //return check;
            return null; // Convert to DTO.
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

        Task<List<TodoDTO>> ITodoService.GetTodosAsync(GetTodoQuery filter, PaginationFilter paginationFilter)
        {
            throw new NotImplementedException();
        }

        public Task<TodoDTO> UpdateSubTodoAsync(UpdateSubTodoTaskDTO subTodo)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TodoDTO>> ListAll()
        {
            var todoDTOs = _todoRepository.FindAll().Result;
            return null;
        }
    }
}
