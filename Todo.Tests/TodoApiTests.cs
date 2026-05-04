using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace Todo.Tests
{
    public class TodoApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public TodoApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }
        private record TodoResponse(int Id, string Title, bool IsCompleted);

        [Fact]
        public async Task EnsureTodoListNotNull()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/todos");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response.Content);

        }
        [Fact]
        public async Task CanRetrieveTodoList()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act
            var response = await client.GetAsync("/todos");
            // Assert
            response.EnsureSuccessStatusCode();
            var todos = await response.Content.ReadFromJsonAsync<TodoResponse[]>();
            Assert.NotNull(todos);
        }
        [Fact]
        public async Task CanSendPostRequestToApi()
        {
            // Arrange
            var client = _factory.CreateClient();

            var newTodo = new
            {
                Title = "Test Todo",
                IsCompleted = false
            };
            // Act

            var response = await client.PostAsync("/todos", new StringContent(JsonConvert.SerializeObject(newTodo), Encoding.UTF8, "application/json"));

            // Assert
            response.EnsureSuccessStatusCode();
            
            Assert.NotNull(response.Content);
            
        }
        [Fact]
        public async Task CanAddTaskToList()
        {
            // Arrange
            var client = _factory.CreateClient();
            var newTodo = new
            {
                Title = "Test Todo",
                IsCompleted = false
            };
            
            // Act

            var response = await client.PostAsync("/todos", new StringContent(JsonConvert.SerializeObject(newTodo), Encoding.UTF8, "application/json"));

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<TodoResponse>();
            Assert.NotNull(result);
            Assert.Equal(newTodo.Title, result.Title);
            Assert.NotEqual(0, result.Id);

        }

    }
}
