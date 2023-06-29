namespace Commons.Token;

public static class AuthenticationExtensions
{
    public static AuthenticationBuilder AddJWTAuthentication(this IServiceCollection services, TokenOptions options)
    {
        TokenOptions opt = options;
        return services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = opt.Issuer,
                ValidAudience = opt.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(opt.Key))
            };
        });
    }
}
