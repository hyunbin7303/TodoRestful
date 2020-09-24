using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Model.Todo;
using TodoApi.Query.Interface;

namespace TodoApi.Web.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoDTO>> ListAsync(string userId, GetTodoQuery query);
        Task<List<Todo>> GetTodosAsync(GetTodoQuery filter = null, PaginationFilter paginationFilter= null); // How we can use PaginationFilter?
        //Do we have to change it to  ToDoDTO to return it?

        Task<TodoDTO> GetOne(string todoId);
        Task<TodoDTO> SaveAsync(Todo todo);
        Task<TodoDTO> UpdateAsync(string id, Todo todo);
        Task<TodoDTO> DeleteAsync(int id);
    }
}