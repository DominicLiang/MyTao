namespace IdentityService.WebAPI.Controllers;

[Route("API/[controller]/[action]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> logger;
    private readonly IIdentityUserDomainService userDomainService;
    private readonly IIdentityRoleDomainService roleDomainService;
    private readonly IConfiguration configuration;

    public TestController(
        ILogger<TestController> logger,
        IIdentityUserDomainService userDomainService,
        IIdentityRoleDomainService roleDomainService,
        IConfiguration configuration)
    {
        this.logger = logger;
        this.userDomainService = userDomainService;
        this.roleDomainService = roleDomainService;
        this.configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> Init(string serialNumer, string adminEmail)
    {
        string SerialNumber = configuration.GetValue<string>("Init.SerialNumber")
        ?? throw new NullReferenceException("无法从配置Init.SerialNumber中获取数据");

        if (serialNumer != SerialNumber) return Ok(new Response(false, "序列号错误", null));

        if (!await roleDomainService.CreateRoleAsync(new Role() { Name = "admin" })) return Ok(new Response(false, "创建admin角色失败", null));
        if (!await roleDomainService.CreateRoleAsync(new Role() { Name = "staff" })) return Ok(new Response(false, "创建staff角色失败", null));
        if (!await roleDomainService.CreateRoleAsync(new Role() { Name = "member" })) return Ok(new Response(false, "创建member角色失败", null));

        if (await userDomainService.IsRegisteredUsernameAsync("admin"))
        {
            await userDomainService.DeleteUserForRealAsync((await userDomainService.FindUserByNameAsync("admin"))!.Id.ToString());
        };

        if (!await userDomainService.CreateUserAsync(new User("admin") { Email = adminEmail }, "Admin123")) return Ok(new Response(false, "Admin创建失败", null));
        User? user = await userDomainService.FindUserByNameAsync("admin");
        if (user == null) return Ok(new Response(false, "Admin不存在", null));

        if (!await userDomainService.AddUserToRoleAsync(user, "admin")) return Ok(new Response(false, "添加admin角色失败", null));
        if (!await userDomainService.AddUserToRoleAsync(user, "staff")) return Ok(new Response(false, "添加staff角色失败", null));
        if (!await userDomainService.AddUserToRoleAsync(user, "member")) return Ok(new Response(false, "添加admin角色失败", null));

        logger.LogInformation("初始化成功，生成超级管理员admin，密码Admin123");
        return Ok(new Response(true, "初始化成功", new
        {
            username = user.UserName,
            userPassword = "Admin123",
            userEmail = user.Email,
        }));
    }

    [HttpGet]
    public IActionResult Content()
    {
        return Ok(new Response(true, "Content  正常连接了", null));
    }

    [HttpGet]
    [Authorize(Roles = "member")]
    public IActionResult ContentNeedMember()
    {
        return Ok(new Response(true, "ContentNeedMember  正常连接了", null));
    }

    [HttpGet]
    [Authorize(Roles = "staff")]
    public IActionResult ContentNeedStaff()
    {
        return Ok(new Response(true, "ContentNeedStaff  正常连接了", null));
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public IActionResult ContentNeedAdmin()
    {
        return Ok(new Response(true, "ContentNeedAdmin  正常连接了", null));
    }
}
