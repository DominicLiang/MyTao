namespace IdentityService.Infrastructure.TokenProvider;

public class TokenProvider<TUser> : IUserTwoFactorTokenProvider<TUser> where TUser : class
{
    private readonly IDistributedCache distributedCache;
    private readonly IConfiguration configuration;

    public TokenProvider(IDistributedCache distributedCache, IConfiguration configuration)
    {
        this.distributedCache = distributedCache;
        this.configuration = configuration;
    }

    public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
    {
        return Task.FromResult(false);
    }

    public async Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
    {
        string key = await GetKey(purpose, manager, user);
        string token = GenerateToken();
        int Expire = configuration.GetValue<int>("TokenProvider.Expire");

        await distributedCache.SetStringAsync(key, token, new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Expire)
        });

        return await Task.FromResult(token);
    }

    public async Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
    {
        string key = await GetKey(purpose, manager, user);
        string tokenInCache = await distributedCache.GetStringAsync(key);
        return await Task.FromResult(tokenInCache == token);
    }

    private async Task<string> GetKey(string purpose, UserManager<TUser> manager, TUser user)
    {
        var userId = await manager.GetUserIdAsync(user);
        string? stamp = null;
        if (manager.SupportsUserSecurityStamp)
        {
            stamp = await manager.GetSecurityStampAsync(user);
        }
        return $"{userId}-{stamp}-{purpose}";
    }

    protected virtual string GenerateToken()
    {
        return string.Empty;
    }
}