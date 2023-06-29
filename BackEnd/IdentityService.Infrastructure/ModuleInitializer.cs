namespace IdentityService.Infrastructure;

public class ModuleInitializer : IModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddScoped<IIdentityRoleDomainService, IdentityRoleDomainService>();
        services.AddScoped<IIdentityUserDomainService, IdentityUserDomainService>();
        services.AddScoped<IIdentityAuthenticationDomainService, IdentityAuthenticationDomainService>();
        services.AddScoped<IIdentityLoginDomainService, IdentityLoginDomainService>();
        services.AddScoped<IIdentityUserInfoDomainService, IdentityUserInfoDomainService>();

        services.AddScoped<IIdentityRoleRepository, IdentityRoleRepository>();
        services.AddScoped<IIdentityUserRepository, IdentityUserRepository>();
        services.AddScoped<IIdentityAuthenticationRepository, IdentityAuthenticationRepository>();
        services.AddScoped<IIdentityUserInfoRepository, IdentityUserInfoRepository>();

        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IPhoneService, PhoneService>();
    }
}