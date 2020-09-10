using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TodoApi.Datasource;
using TodoApi.Model.Todo;
using TodoApi.Query.Interface;

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
        public Task<TodoDTO> SaveAsync(Todo todo)
        {
            throw new NotImplementedException();
        }
        public Task<TodoDTO> UpdateAsync(int id, Todo todo)
        {
            throw new NotImplementedException();
        }
    }
}
