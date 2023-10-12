using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HttpClientDemo.Server;

public class IdentityService
{
    public const string VALID_ISSUER = "testIssuer";
    public const string VALID_AUDIENCE = "demoServer";

    public TokenDto BuildToken()
    {
        var accessTokenExpiration = DateTime.UtcNow.AddMinutes(2);

        var securityToken = new JwtSecurityToken
        (
            issuer: VALID_ISSUER,
            audience: VALID_AUDIENCE,
            claims: GetClaims(),
            expires: accessTokenExpiration,
            notBefore: DateTime.UtcNow,
            signingCredentials: SigningConfigurations.Instance.SigningCredentials
        );

        var handler = new JwtSecurityTokenHandler();
        var accessToken = handler.WriteToken(securityToken);

        var token = new TokenDto
        {
            AccessToken = accessToken!,
            Expiration = accessTokenExpiration.Ticks,
            RefreshToken = Guid.NewGuid().ToString()
        };

        return token;
    }

    private static IEnumerable<Claim> GetClaims()
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, "testUsername"),
            new Claim(JwtRegisteredClaimNames.GivenName, "FirstName"),
            new Claim(JwtRegisteredClaimNames.FamilyName, "LastName")
        };

        return claims;
    }
}
