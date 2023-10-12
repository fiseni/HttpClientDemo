using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HttpClientDemo.Server;

public class SigningConfigurations
{
    public static SigningConfigurations Instance = new SigningConfigurations(Guid.NewGuid().ToString());

    public SecurityKey SecurityKey { get; }
    public SigningCredentials SigningCredentials { get; }

    private SigningConfigurations(string key)
    {
        var keyBytes = Encoding.ASCII.GetBytes(key);

        SecurityKey = new SymmetricSecurityKey(keyBytes);
        SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature);
    }
}
