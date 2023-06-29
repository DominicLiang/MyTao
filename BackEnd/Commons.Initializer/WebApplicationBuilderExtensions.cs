namespace Commons.Initializer;

public static class WebApplicationBuilderExtensions
{
    public static void ConfigureExtraServices(this WebApplicationBuilder builder, InitializerOptions initOptions)
    {
        IServiceCollection services = builder.Services;
        IConfiguration configuration = builder.Configuration;

        // apollo配置 写到管理用户机密里
        // {
        //     "MetaServer": "http://xxx:8080/",
        //     "AppId": "xxx",
        //     "Namespaces": [ "xxx","xxx" ]
        // }
        builder.Host.ConfigureAppConfiguration((c, b) =>
        {
            b.AddApollo(configuration)
            .AddNamespace("T1.Commons", ConfigFileFormat.Json)
            .AddNamespace("Identity", ConfigFileFormat.Json);
        });

        // 基础模块和dbcontext注册
        // "Database.ConnStr": "xxxxxxxxxxx"
        var assemblies = ReflectionHelper.GetAllReferencedAssemblies();
        services.RunModuleInitializers(assemblies);
        services.AddAllDbContexts(ctx =>
        {
            string connStr = configuration.GetValue<string>("Database.ConnStr")
            ?? throw new NullReferenceException("无法从配置Database.ConnStr中获取数据");
            ctx.UseSqlServer(connStr);
        }, assemblies);

        // 注册日志
        // "Logging.Exceptionless": "xxxxxxx"
        // https://exceptionless.com/
        string exceptionlessconnect = configuration.GetValue<string>("Logging.Exceptionless")
        ?? throw new NullReferenceException("无法从配置Logging.Exceptionless中获取数据");
        ExceptionlessClient.Default.Startup(exceptionlessconnect);
        builder.Logging.ClearProviders();
        Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.File(new JsonFormatter(), initOptions.LogFilePath, fileSizeLimitBytes: 100000, retainedFileCountLimit: 50)
                    .WriteTo.Exceptionless()
                    .CreateLogger();
        builder.Logging.AddSerilog();

        // jwt和认证与swagger认证按钮
        // "Authentication.Token": { "Issuer" : "xxx", "Audience" : "xxx", "Key" : "xxx", "Expire" : "xxx" }
        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication();
        services.Configure<TokenOptions>(configuration.GetSection("Authentication.Token"));
        TokenOptions opt = configuration.GetSection("Authentication.Token").Get<TokenOptions>()
        ?? throw new NullReferenceException("无法从配置Authentication.Token中获取数据");
        builder.Services.AddJWTAuthentication(opt);
        builder.Services.Configure<SwaggerGenOptions>(c => { c.AddAuthenticationHeader(); });

        // mediatR事件
        services.AddMediatR(assemblies);

        // Cors跨域
        //"Cors.Origins": { "Origins" : ["http://xxxxxxx:xxx"] }
        services.AddCors(options =>
        {
            var corsOpt = configuration.GetSection("Cors.Origins").Get<CorsSettings>()
            ?? throw new NullReferenceException("无法从配置Cors.Origins中获取数据");
            string[] urls = corsOpt.Origins ?? Array.Empty<string>();
            options.AddDefaultPolicy(corsbuilder => corsbuilder.WithOrigins(urls)
                .AllowAnyMethod().AllowAnyHeader().AllowCredentials());
        });

        // rabbitMQ和eventBus
        // "EventBus.RabbitMQOptions": { "HostName" : "xxx", "ExchangeName" : "xxx", "UserName" : "xxx", "Password" : "xxx" }
        services.Configure<IntegrationEventRabbitMQOptions>(configuration.GetSection("EventBus.RabbitMQOptions"));
        services.AddEventBus(initOptions.EventBusQueueName, assemblies);
        Console.WriteLine("HostName   " + configuration.GetValue<string>("Logging.Exceptionless"));
        Console.WriteLine("HostName   " + configuration.GetSection("EventBus.RabbitMQOptions").Get<IntegrationEventRabbitMQOptions>().HostName);
        Console.WriteLine("ExchangeName   " + configuration.GetSection("EventBus.RabbitMQOptions").Get<IntegrationEventRabbitMQOptions>().ExchangeName);
        Console.WriteLine("UserName   " + configuration.GetSection("EventBus.RabbitMQOptions").GetValue<string>("UserName"));
        Console.WriteLine("Password   " + configuration.GetSection("EventBus.RabbitMQOptions").GetValue<string>("Password"));
        Console.WriteLine("Section   " + configuration.GetSection("EventBus.RabbitMQOptions").Value);

        //// redis缓存
        // "Cache.Redis": { "Config" : "localhost", "InstanceName" : "RedisCache"}
        var redisOptions = configuration.GetSection("Cache.Redis").Get<RedisOptions>()
            ?? throw new NullReferenceException("无法从配置Cache.Redis中获取数据");

        builder.Services.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = redisOptions.Config;
            opt.InstanceName = redisOptions.InstanceName;
        });

        // 开启数据验证过滤器
        builder.Services.Configure<ApiBehaviorOptions>(opt => opt.SuppressModelStateInvalidFilter = true);

        // 添加过滤器
        builder.Services.Configure<MvcOptions>(opt =>
        {
            opt.Filters.Add<TokenValidationFilter>();
            opt.Filters.Add<TokenRefreshFilter>();
            opt.Filters.Add<ModelValidateFilter>();
            opt.Filters.Add<ProductionSafetyFilter>();
        });
    }
}