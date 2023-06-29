namespace IdentityService.Infrastructure.TokenProvider;

public class EmailTokenProvider<TUser> : TokenProvider<TUser> where TUser : class
{
    public EmailTokenProvider(IDistributedCache distributedCache, IConfiguration configuration) : base(distributedCache, configuration)
    {
    }

    protected override string GenerateToken()
    {
        return Guid.NewGuid().ToString("N");
    }
}
