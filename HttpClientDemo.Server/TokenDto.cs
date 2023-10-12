namespace HttpClientDemo.Server;

public class TokenDto
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public long Expiration { get; set; }
}
