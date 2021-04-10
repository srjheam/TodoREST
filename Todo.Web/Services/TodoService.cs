using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Todo.Core.Models;

namespace Todo.Web.Services
{
    public class TodoService : ITodoService
    {
        private const string BASE_REQUEST_URI = "api/todoitems";
        private readonly HttpClient httpClient;

        public TodoService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<TodoItem>> GetTodoList()
        {
            var response = await httpClient.GetStringAsync(BASE_REQUEST_URI);
            var todoList = JsonConvert.DeserializeObject<IEnumerable<TodoItem>>(response);

            return todoList;
        }

        public async Task<(bool IsSuccess, TodoItem NewTodo)> AddTodo(string title)
        {
            var json = JsonConvert.SerializeObject(new { Title = title });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(BASE_REQUEST_URI, content);

            (bool IsSuccess, TodoItem NewTodo) result = (false, null);
            if (response.IsSuccessStatusCode)
            {
                result.IsSuccess = true;
                result.NewTodo = JsonConvert.DeserializeObject<TodoItem>(await response.Content.ReadAsStringAsync());
            }
            return result;
        }

        public async Task<bool> ChangeValues(TodoItem todo)
        {
            var json = JsonConvert.SerializeObject(todo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(Path.Combine(BASE_REQUEST_URI, todo.Id.ToString()), content);

            return response.IsSuccessStatusCode;
        }
    }
}
