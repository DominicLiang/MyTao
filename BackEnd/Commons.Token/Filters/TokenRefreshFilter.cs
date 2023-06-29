namespace Commons.Token.Filters;

public class TokenRefreshFilter : IAsyncAuthorizationFilter
{
    private readonly IOptionsSnapshot<TokenOptions> options;
    private readonly IConfiguration configuration;
    private readonly IDistributedCache distributedCache;

    public TokenRefreshFilter(
        IOptionsSnapshot<TokenOptions> options,
        IConfiguration configuration,
        IDistributedCache distributedCache)
    {
        this.options = options;
        this.configuration = configuration;
        this.distributedCache = distributedCache;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        JwtSecurityToken? token = null;
        string? authorization = context.HttpContext.Request.Headers["Authorization"];

        if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
        {
            token = new JwtSecurityTokenHandler().ReadJwtToken(authorization.Substring("Bearer ".Length));
        }

        int leadTime = configuration.GetValue<int>("Authentication.TokenRefreshTime");
        int Expire = configuration.GetSection("Authentication.Token").GetValue<int>("Expire");

        //刷新Token
        if (token != null
            && token.ValidTo > DateTime.Now
            && token.ValidTo.AddMinutes(-1 * leadTime) <= DateTime.Now)
        {
            string newToken = options.Value.CreateToken(token.Claims);
            context.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "X-Token");
            context.HttpContext.Response.Headers.Add("X-Token", newToken);

            var id = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            if (id != null)
            {
                await distributedCache.SetStringAsync(id, newToken, new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Expire)
                });
            }
        }
    }
}