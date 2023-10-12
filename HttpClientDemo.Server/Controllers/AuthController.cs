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
    public ActionResult<TokenDto> Authenticate()
    {
        var accessTokenExpiration = DateTime.UtcNow.AddMinutes(2);

        var token = _identityService.BuildToken();

        return Ok(token);
    }

    [HttpPost("/identity/refresh-token")]
    public ActionResult<TokenDto> RefreshToken()
    {
        var accessTokenExpiration = DateTime.UtcNow.AddMinutes(2);

        var token = _identityService.BuildToken();

        return Ok(token);
    }
}
