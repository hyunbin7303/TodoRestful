using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Model.Todo;
using TodoApi.Query.Interface;

namespace TodoApi.Web.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoDTO>> ListAsync();
        Task<TodoDTO> SaveAsync(Todo todo);
        Task<TodoDTO> UpdateAsync(int id, Todo todo);
        Task<TodoDTO> DeleteAsync(int id);
    }
}