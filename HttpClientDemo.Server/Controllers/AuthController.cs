using Microsoft.AspNetCore.Mvc;

namespace HttpClientDemo.Server.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly IdentityService _identityService;

    public AuthController(IdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost("/identity/authenticate")]
    public async Task<ActionResult<TokenDto>> Authenticate()
    {
        await Task.Delay(50);

        var token = _identityService.BuildToken();

        return Ok(token);
    }

    [HttpPost("/identity/refresh-token")]
    public async Task<ActionResult<TokenDto>> RefreshToken()
    {
        await Task.Delay(50);

        var token = _identityService.BuildToken();

        return Ok(token);
    }
}
