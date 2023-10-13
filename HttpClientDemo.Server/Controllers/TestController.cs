using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HttpClientDemo.Server.Controllers;

[ApiController]
public class TestController : ControllerBase
{
    [HttpGet("api/test1")]
    public ActionResult Get()
    {
        return Ok("Hello from server.");
    }

    [HttpGet("api/test2")]
    [Authorize]
    public ActionResult GetProtected()
    {
        return Ok("Hello from server. Protected endpoint.");
    }
}
