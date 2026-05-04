using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
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
        [Fact]
        public async Task CanGetTaskById()
        {
            // Arrange
            var client = _factory.CreateClient();
            var newTodo = new
            {
                Title = "Test Todo",
                IsCompleted = false
            };
            var creationResponse = await client.PostAsync("/todos", new StringContent(JsonConvert.SerializeObject(newTodo), Encoding.UTF8, "application/json"));
            creationResponse.EnsureSuccessStatusCode();
            
            // act
            var GetByIdResponse = await client.GetAsync("/todos/1");
            GetByIdResponse.EnsureSuccessStatusCode();
            var todo = await GetByIdResponse.Content.ReadFromJsonAsync<TodoResponse>();
            
            // Assert
            Assert.NotNull(todo);
            Assert.Equal(newTodo.Title, todo.Title);
            Assert.NotEqual(0, todo.Id);
        }
        [Fact]
        public async Task CanSetTaskAsCompleted()
        {
            // Arrange
            var client = _factory.CreateClient();
            var newTodo = new
            {
                Title = "Test Todo",
                IsCompleted = false
            };
            var creationResponse = await client.PostAsync("/todos", new StringContent(JsonConvert.SerializeObject(newTodo), Encoding.UTF8, "application/json"));

            creationResponse.EnsureSuccessStatusCode();
            // Act
            var response = await client.PatchAsync("/todos/1", new StringContent(JsonConvert.SerializeObject(new { IsCompleted = true }), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            var getResponse = await client.GetAsync("/todos/1");
            // Assert
            getResponse.EnsureSuccessStatusCode();
            var todo = await getResponse.Content.ReadFromJsonAsync<TodoResponse>();
            Assert.NotNull(todo);
            Assert.True(todo.IsCompleted);
        }
        [Fact]
        public async Task CanDeleteTask()
        {
            // Arrange
            var client = _factory.CreateClient();
            var newTodo = new
            {
                Title = "Winning",
                IsCompleted = false
            };
            var newTodo2 = new
            {
                Title = "losing",
                IsCompleted = false
            };
            var creationResponse = await client.PostAsync("/todos", new StringContent(JsonConvert.SerializeObject(newTodo), Encoding.UTF8, "application/json"));
            creationResponse.EnsureSuccessStatusCode();

            creationResponse = await client.PostAsync("/todos", new StringContent(JsonConvert.SerializeObject(newTodo2), Encoding.UTF8, "application/json"));
            creationResponse.EnsureSuccessStatusCode();

            // Act
            var deletionResponse = await client.DeleteAsync("/todos/2");

            // Assert
            var getResponse = await client.GetAsync("/todos/1");
            getResponse.EnsureSuccessStatusCode();
            var todo = await getResponse.Content.ReadFromJsonAsync<TodoResponse>();
            Assert.NotNull(todo);
            getResponse = await client.GetAsync("/todos/2");
            Assert.Equal(404, (int)getResponse.StatusCode);
        }
    }
}
