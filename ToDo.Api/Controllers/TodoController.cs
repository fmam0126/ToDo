using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using ToDo.Api.Models;

namespace ToDo.Api.Controllers
{
    [ApiController]
    [Route("todos")]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _todoContext;

        public TodoController(TodoContext todoContext)
        {
            _todoContext = todoContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetTodoList()
        {
            var todos = await _todoContext.Todos.ToListAsync();
            return Ok(todos);
        }
        [HttpPost]
        public async Task<IActionResult> PostTodoItem(TodoDTO newTodo)
        {
            var Todo = new Todo
            {
                Title = newTodo.Title,
                IsCompleted = newTodo.IsCompleted,
            };

            _todoContext.Todos.Add(Todo);
            await _todoContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoList), new { id = Todo.Id }, Todo);
        }
    }
}
