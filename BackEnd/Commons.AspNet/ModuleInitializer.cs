using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Zack.Commons;
using Commons.AspNet.Filters;

namespace IdentityService.Infrastructure;

public class ModuleInitializer : IModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationMiddlewareResultHandler, AuthorizationResponseHandler>();
    }
}