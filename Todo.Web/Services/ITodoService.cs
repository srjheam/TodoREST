using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Core.Models;

namespace Todo.Web.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoItem>> GetTodoList();
        Task<(bool IsSuccess, TodoItem NewTodo)> AddTodo(string title);
        Task<bool> ChangeValues(TodoItem todo);
    }
}
