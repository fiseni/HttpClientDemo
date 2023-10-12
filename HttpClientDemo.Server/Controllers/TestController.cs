using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HttpClientDemo.Server.Controllers;

[ApiController]
[Authorize]
public class TestController : ControllerBase
{
    [HttpGet("api/test")]
    public ActionResult Get()
    {
        return Ok("Hello from server");
    }
}
