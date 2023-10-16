using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HttpClientDemo.Server.Controllers;

[ApiController]
public class TestController : ControllerBase
{
    [HttpGet("api/test1")]
    public async Task<ActionResult<string>> Get()
    {
        await Task.Delay(50);

        return Ok("Hello from server.");
    }

    [HttpGet("api/test2")]
    [Authorize]
    public async Task<ActionResult<string>> GetProtected()
    {
        await Task.Delay(50);

        return Ok("Hello from server. Protected endpoint.");
    }
}
