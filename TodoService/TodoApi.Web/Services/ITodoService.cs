using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Model.Todo;
using TodoApi.Query.Interface;
using TodoApi.Query.Interface.DTOs;

namespace TodoApi.Web.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoDTO>> ListAll();
        Task<IEnumerable<TodoDTO>> ListTodoAsync(string userId, GetTodoQuery query);
        Task<List<TodoDTO>> GetTodosAsync(GetTodoQuery filter = null, PaginationFilter paginationFilter= null); // How we can use PaginationFilter?
        Task<TodoDTO> GetOne(string todoId);
        Task<bool> SaveAsync(CreateTodoDTO todo);
        Task<bool> UpdateAsync(string Todoid, UpdateTodoDTO todo);
        Task<TodoDTO> UpdateSubTodoAsync(string TodoId, UpdateSubTodoTaskDTO subTodo);
        Task<bool> DeleteAsync(string todoId);
        //Task<List<Tag>> GetAllTagAsync();
    }
}