using Microsoft.AspNetCore.Mvc;

namespace ToDo.Api.Controllers
{
    [ApiController]
    [Route("todos")]
    public class TodoController : ControllerBase
    {
       [HttpGet]
       public async Task<IActionResult> Get()
        { 
           return Ok();
        }
    }
}
