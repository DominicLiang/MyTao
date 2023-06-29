namespace IdentityService.WebAPI.Controllers;

[Route("API/[controller]/[action]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly ILogger<LoginController> logger;
    private readonly IIdentityUserDomainService userDomainService;
    private readonly IIdentityLoginDomainService loginDomainService;
    private readonly IIdentityAuthenticationDomainService authenticationDomainService;

    public LoginController(
        ILogger<LoginController> logger,
        IIdentityUserDomainService userDomainService,
        IIdentityLoginDomainService loginDomainService,
        IIdentityAuthenticationDomainService authenticationDomainService)
    {
        this.logger = logger;
        this.userDomainService = userDomainService;
        this.loginDomainService = loginDomainService;
        this.authenticationDomainService = authenticationDomainService;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterNewUser(UserDataViewModel userdata)
    {
        if (userdata.Username == null || userdata.Username == string.Empty) return Ok(new Response(false, "用户名不能为空", null));
        if (await userDomainService.IsRegisteredUsernameAsync(userdata.Username))
            return Ok(new Response(false, "用户名已被其他用户注册", null));

        if (userdata.Email == null || userdata.Email == string.Empty) return Ok(new Response(false, "用户邮箱不能为空", null));
        if (await userDomainService.IsRegisteredEmailAsync(userdata.Email))
            return Ok(new Response(false, "邮件地址已被其他用户注册", null));

        if (userdata.PhoneNumber != null && userdata.PhoneNumber != string.Empty && await userDomainService.IsRegisteredPhoneAsync(userdata.PhoneNumber))
            return Ok(new Response(false, "电话号码已被其他用户注册", null));

        if (userdata.Password == null || userdata.Password == string.Empty) return Ok(new Response(false, "密码不能为空", null));

        User newUser = new User(userdata.Username)
        {
            Email = userdata.Email,
            PhoneNumber = userdata.PhoneNumber,
        };
        if (!await userDomainService.CreateUserAsync(newUser, userdata.Password))
            return Ok(new Response(false, "用户注册失败", null));

        User? user = await userDomainService.FindUserByNameAsync(userdata.Username);
        if (user == null) return Ok(new Response(false, "用户不存在", null));

        if (!await userDomainService.AddUserToRoleAsync(user, "member"))
            return Ok(new Response(false, "添加member角色失败", null));

        return Ok(new Response(true, "用户注册成功", null));
    }

    [HttpPost]
    public async Task<IActionResult> SendPhoneVerifyLoginToken(UserDataViewModel userdata)
    {
        if (userdata.PhoneNumber == null || userdata.PhoneNumber == string.Empty) return Ok(new Response(false, "电话号码不能为空", null));

        User? user = await userDomainService.FindUserByPhoneAsync(userdata.PhoneNumber);
        if (user == null) return Ok(new Response(false, "用户不存在", null));

        if (!await authenticationDomainService.SendVerifyTokenByPhoneAsync(user, "LoginByPhone", "TencentCloud.Template.Login")) return Ok(new Response(false, "发送短信失败", null));

        return Ok(new Response(true, "发送短信成功", null));
    }

    [HttpPost]
    public async Task<IActionResult> LoginByPhoneVerify(UserDataViewModel userdata)
    {
        if (userdata.PhoneNumber == null || userdata.PhoneNumber == string.Empty) return Ok(new Response(false, "用户名电话号码不能为空", null));
        if (userdata.Token == null || userdata.Token == string.Empty) return Ok(new Response(false, "token不能为空", null));

        User? user = await userDomainService.FindUserByPhoneAsync(userdata.PhoneNumber);
        if (user == null) return Ok(new Response(false, "用户不存在", null));

        if (!user.EmailConfirmed) return Ok(new Response(false, "登陆前请先验证电子邮箱", null));
        if (!user.PhoneNumberConfirmed) return Ok(new Response(false, "登陆前请先验证电话号码", null));

        if (!await authenticationDomainService.VerifyByPhoneAsync(user, "LoginByPhone", userdata.Token)) return Ok(new Response(false, "token不正确", null));

        var token = await loginDomainService.LoginAsync(user);
        if (token == null) return Ok(new Response(false, "登录失败", null));

        HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "X-Token");
        HttpContext.Response.Headers.Add("X-Token", token);

        logger.LogInformation($"用户 {user.UserName} 通过电话号码登录了");
        return Ok(new Response(true, "正常登录", null));
    }

    [HttpPost]
    public async Task<IActionResult> LoginByPassword(UserDataViewModel userdata)
    {
        if (userdata.Username == null || userdata.Username == string.Empty) return Ok(new Response(false, "用户识别不能为空", null));
        if (userdata.Password == null || userdata.Password == string.Empty) return Ok(new Response(false, "密码不能为空", null));

        User? userByName = await userDomainService.FindUserByNameAsync(userdata.Username);
        User? userByEmail = await userDomainService.FindUserByEmailAsync(userdata.Username);
        User? userByPhone = await userDomainService.FindUserByPhoneAsync(userdata.Username);
        User? user = userByName != null ? userByName : userByEmail != null ? userByEmail : userByPhone != null ? userByPhone : null;
        if (user == null) return Ok(new Response(false, "用户不存在", null));

        if (!user.EmailConfirmed) return Ok(new Response(false, "登陆前请先验证电子邮箱", null));

        if (!await userDomainService.CheckPasswordAsync(user, userdata.Password)) return Ok(new Response(false, "密码不正确", null));

        var token = await loginDomainService.LoginAsync(user);
        if (token == null) return Ok(new Response(false, "登录失败", null));

        HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "X-Token");
        HttpContext.Response.Headers.Add("X-Token", token);

        logger.LogInformation($"用户 {user.Id} 通过用户名登录了");
        return Ok(new Response(true, "正常登录", null));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        Claim? claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
        if (claim == null) return Ok(new Response(false, "claim为空", null));

        if (claim.Value == null) return Ok(new Response(false, "用户为空", null));

        var result = await loginDomainService.LogoutAsync(claim.Value);

        if (!result) return Ok(new Response(false, "登出失败", null));

        logger.LogInformation($"用户 {claim.Value} 登出了");
        return Ok(new Response(true, "正常登出", null));
    }
}
