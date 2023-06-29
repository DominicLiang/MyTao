namespace Commons.Initializer;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// 在var app = builder.Build();之后使用
    /// 在app.UseAuthorization()和app.UseHttpsRedirection()前面
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseDefault(this IApplicationBuilder app)
    {
        app.UseEventBus();
        app.UseCors();
        app.UseAuthentication();
        return app;
    }
}