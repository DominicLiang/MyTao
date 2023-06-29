namespace IdentityService.Infrastructure.TokenProvider;

public class TokenData
{
    public string Token { get; set; }
    public System.Timers.Timer Timer { get; set; }

    public TokenData(string token, System.Timers.Timer timer)
    {
        Token = token;
        Timer = timer;
    }
}
