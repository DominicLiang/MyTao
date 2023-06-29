namespace IdentityService.Infrastructure.TokenProvider;

public class PhoneTokenProvider<TUser> : TokenProvider<TUser> where TUser : class
{
    public PhoneTokenProvider(IDistributedCache distributedCache, IConfiguration configuration) : base(distributedCache, configuration)
    {
    }

    protected override string GenerateToken()
    {
        var result = new StringBuilder();
        for (var i = 0; i < 6; i++)
        {
            result.Append(Random.Shared.Next(0, 10).ToString());
        }
        return result.ToString();
    }
}
