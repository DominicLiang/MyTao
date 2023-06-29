namespace IdentityService.Domain;

public class IdentityLoginDomainService : IIdentityLoginDomainService
{
    private readonly IIdentityUserRepository userRepository;
    private readonly IOptions<Commons.Token.TokenOptions> options;
    private readonly IDistributedCache distributedCache;
    private readonly IMediator mediator;

    public IdentityLoginDomainService(
        IIdentityUserRepository userRepository,
        IOptions<Commons.Token.TokenOptions> options,
        IDistributedCache distributedCache,
        IMediator mediator)
    {
        this.userRepository = userRepository;
        this.options = options;
        this.distributedCache = distributedCache;
        this.mediator = mediator;
    }

    public async Task<string?> LoginAsync(User user)
    {
        string token = await CreateTokenAsync(user);

        await distributedCache.SetStringAsync(user.Id.ToString(), token, new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(options.Value.Expire)
        });

        var result = await userRepository.UpdateUserAsync(user);
        if (!result) return null;

        await mediator.Publish(new UserLoginEvent(user));
        return token;
    }

    public async Task<bool> LogoutAsync(string id)
    {
        var user = await userRepository.FindUserByIdAsync(id);
        if (user == null) return false;

        await distributedCache.RemoveAsync(user.Id.ToString());

        var result = await userRepository.UpdateUserAsync(user);
        if (!result) return false;

        await mediator.Publish(new UserLogoutEvent(user));
        return true;
    }

    private async Task<string> CreateTokenAsync(User user)
    {
        var roles = await userRepository.GetRolesAsync(user);
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Sid,user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
        };
        foreach (string role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        return options.Value.CreateToken(claims);
    }
}
