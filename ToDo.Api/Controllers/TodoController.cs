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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoById(int id)
        {
            var todo = await _todoContext.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            return Ok(todo);
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
        [HttpPatch("{id}")]
        public async Task<IActionResult> MarkAsComplete(int id)
        {
            var todo = await _todoContext.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            todo.IsCompleted = true;
            await _todoContext.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            var todo = await _todoContext.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            _todoContext.Todos.Remove(todo);
            await _todoContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
