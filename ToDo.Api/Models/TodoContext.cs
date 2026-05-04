using Microsoft.EntityFrameworkCore;
using ToDo.Api.Interfaces;
namespace ToDo.Api.Models
{

        public class TodoContext(DbContextOptions<TodoContext> options) : DbContext(options), ITodoContext
        { 
        public DbSet<Todo> Todos { get; set; } = null!;
        }
    
}
