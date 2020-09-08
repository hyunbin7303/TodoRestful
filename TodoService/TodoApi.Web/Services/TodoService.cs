using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Model.Todo;
using TodoApi.Query.Interface;

namespace TodoApi.Web.Services
{
    public class TodoService : ITodoService
    {
        public Task<TodoDTO> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<TodoDTO>> ListAsync()
        {
            throw new NotImplementedException();
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
