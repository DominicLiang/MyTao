var builder = WebApplication.CreateBuilder(args);

builder.ConfigureExtraServices(new InitializerOptions()
{
    LogFilePath = "Logs/IdentityService.log",
    EventBusQueueName = "IdentityService"
});

builder.Services.AddDataProtection();
builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.Tokens.PasswordResetTokenProvider = Microsoft.AspNetCore.Identity.TokenOptions.DefaultEmailProvider;
    options.Tokens.EmailConfirmationTokenProvider = Microsoft.AspNetCore.Identity.TokenOptions.DefaultEmailProvider;
    options.Tokens.ChangeEmailTokenProvider = Microsoft.AspNetCore.Identity.TokenOptions.DefaultEmailProvider;
    options.Tokens.ChangePhoneNumberTokenProvider = Microsoft.AspNetCore.Identity.TokenOptions.DefaultPhoneProvider;
})
.AddTokenProvider<PhoneTokenProvider<User>>("PhoneTokenProvider")
.AddTokenProvider<IdentityService.Infrastructure.TokenProvider.EmailTokenProvider<User>>("EmailTokenProvider");

IdentityBuilder idBuilder = new IdentityBuilder(typeof(User), typeof(Role), builder.Services);
idBuilder.AddEntityFrameworkStores<IdentityDbContext>()
         .AddDefaultTokenProviders()
         .AddUserManager<UserManager<User>>()
         .AddRoleManager<RoleManager<Role>>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefault();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

