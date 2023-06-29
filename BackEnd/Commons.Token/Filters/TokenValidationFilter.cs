namespace Commons.Token.Filters;

public class TokenValidationFilter : IAsyncAuthorizationFilter
{
    private readonly IDistributedCache distributedCache;

    public TokenValidationFilter(IDistributedCache distributedCache)
    {
        this.distributedCache = distributedCache;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        string tokenInContext = string.Empty;
        string? authorization = context.HttpContext.Request.Headers["Authorization"];
        if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
        {
            tokenInContext = authorization.Substring("Bearer ".Length);
        }
        if (context.HttpContext.User.Identity?.Name != null)
        {
            var id = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            if (id != null)
            {
                string tokenInCache = await distributedCache.GetStringAsync(id);
                if (tokenInCache == null || tokenInCache == string.Empty || tokenInCache != tokenInContext)
                {
                    context.Result = new ObjectResult(new Response(false, "token不合法", null))
                    {
                        StatusCode = (int)HttpStatusCode.OK
                    };
                }
            }
        }
    }
}