namespace IdentityService.WebAPI.Controllers;

[Route("API/[controller]/[action]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> logger;
    private readonly IIdentityUserDomainService userDomainService;
    private readonly IIdentityAuthenticationDomainService authenticationDomainService;

    public UserController(
        ILogger<UserController> logger,
        IIdentityUserDomainService userDomainService,
        IIdentityAuthenticationDomainService authenticationDomainService)
    {
        this.logger = logger;
        this.userDomainService = userDomainService;
        this.authenticationDomainService = authenticationDomainService;
    }

    [HttpPost]
    public async Task<IActionResult> SendChangePasswordByEmailToken(UserDataViewModel userdata)
    {
        if (userdata.Email == null || userdata.Email == string.Empty) return Ok(new Response(false, "用户邮箱不能为空", null));

        User? user = await userDomainService.FindUserByEmailAsync(userdata.Email);
        if (user == null) return Ok(new Response(false, "用户不存在", null));

        var result = await authenticationDomainService.SendVerifyTokenByEmailAsync(user, "ChangePasswordByEmail", "MailKit.Template.ChangePassword");
        if (result == false) return Ok(new Response(false, "发送邮件失败", null));

        return Ok(new Response(true, "发送邮件成功", new
        {
            username = user.UserName,
        }));
    }

    [HttpPost]
    public async Task<IActionResult> ChangePasswordByEmail(UserDataViewModel userdata)
    {
        if (userdata.Username == null || userdata.Username == string.Empty) return Ok(new Response(false, "用户名不能为空", null));
        if (userdata.Token == null || userdata.Token == string.Empty) return Ok(new Response(false, "token不能为空", null));
        if (userdata.Password == null || userdata.Password == string.Empty) return Ok(new Response(false, "密码不能为空", null));

        User? user = await userDomainService.FindUserByNameAsync(userdata.Username);
        if (user == null) return Ok(new Response(false, "用户不存在", null));

        if (!await authenticationDomainService.VerifyByEmailAsync(user, "ChangePasswordByEmail", userdata.Token)) return Ok(new Response(false, "token不正确", null));

        var result = await authenticationDomainService.ResetPasswordAsync(user, userdata.Password);
        if (result == false) return Ok(new Response(false, "更改密码失败", null));

        logger.LogInformation($"用户 {user.Id} 通过邮箱认证更改密码了");
        return Ok(new Response(true, "更改密码成功", null));
    }

    [HttpPost]
    public async Task<IActionResult> SendChangePasswordByPhoneToken(UserDataViewModel userdata)
    {
        if (userdata.PhoneNumber == null || userdata.PhoneNumber == string.Empty) return Ok(new Response(false, "用户名电话号码不能为空", null));

        User? user = await userDomainService.FindUserByPhoneAsync(userdata.PhoneNumber);
        if (user == null) return Ok(new Response(false, "用户不存在", null));

        var result = await authenticationDomainService.SendVerifyTokenByPhoneAsync(user, "ChangePasswordByPhone", "TencentCloud.Template.ChangePassword");
        if (result == false) return Ok(new Response(false, "发送短信失败", null));

        return Ok(new Response(true, "发送短信成功", new
        {
            username = user.UserName,
        }));
    }

    [HttpPost]
    public async Task<IActionResult> ChangePasswordByPhone(UserDataViewModel userdata)
    {
        if (userdata.Username == null || userdata.Username == string.Empty) return Ok(new Response(false, "用户名不能为空", null));
        if (userdata.Token == null || userdata.Token == string.Empty) return Ok(new Response(false, "token不能为空", null));
        if (userdata.Password == null || userdata.Password == string.Empty) return Ok(new Response(false, "密码不能为空", null));

        User? user = await userDomainService.FindUserByNameAsync(userdata.Username);
        if (user == null) return Ok(new Response(false, "用户不存在", null));

        if (!await authenticationDomainService.VerifyByPhoneAsync(user, "ChangePasswordByPhone", userdata.Token)) return Ok(new Response(false, "token不正确", null));

        var result = await authenticationDomainService.ResetPasswordAsync(user, userdata.Password);
        if (result == false) return Ok(new Response(false, "更改密码失败", null));

        logger.LogInformation($"用户 {user.Id} 通过电话认证更改密码了");
        return Ok(new Response(true, "更改密码成功", null));
    }

    [HttpPost]
    public async Task<IActionResult> SendConfirmEmailToken(UserDataViewModel userdata)
    {
        if (userdata.Email == null || userdata.Email == string.Empty) return Ok(new Response(false, "用户邮箱不能为空", null));

        User? user = await userDomainService.FindUserByEmailAsync(userdata.Email);
        if (user == null) return Ok(new Response(false, "用户不存在", null));

        var result = await authenticationDomainService.SendVerifyTokenByEmailAsync(user, "ConfirmEmail", "MailKit.Template.Confirm");
        if (result == false) return Ok(new Response(false, "发送邮件失败", null));

        return Ok(new Response(true, "发送邮件成功", new
        {
            username = user.UserName,
        }));
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmEmail(UserDataViewModel userdata)
    {
        if (userdata.Username == null || userdata.Username == string.Empty) return Ok(new Response(false, "用户名不能为空", null));
        if (userdata.Token == null || userdata.Token == string.Empty) return Ok(new Response(false, "token不能为空", null));

        User? user = await userDomainService.FindUserByNameAsync(userdata.Username);
        if (user == null) return Ok(new Response(false, "用户不存在", null));

        if (!await authenticationDomainService.VerifyByEmailAsync(user, "ConfirmEmail", userdata.Token)) return Ok(new Response(false, "token不正确", null));

        var result = await authenticationDomainService.ConfirmEmailAsync(user);
        if (result == false) return Ok(new Response(false, "邮箱验证失败", null));

        logger.LogInformation($"用户 {user.Id} 通过邮箱认证了");
        return Ok(new Response(true, "邮箱验证成功", null));
    }

    [HttpPost]
    public async Task<IActionResult> SendConfirmPhoneNumberToken(UserDataViewModel userdata)
    {
        if (userdata.PhoneNumber == null || userdata.PhoneNumber == string.Empty) return Ok(new Response(false, "用户电话号码不能为空", null));

        User? user = await userDomainService.FindUserByPhoneAsync(userdata.PhoneNumber);
        if (user == null) return Ok(new Response(false, "用户不存在", null));

        var result = await authenticationDomainService.SendVerifyTokenByPhoneAsync(user, "ConfirmPhoneNumber", "TencentCloud.Template.Confirm");
        if (result == false) return Ok(new Response(false, "发送短信失败", null));

        return Ok(new Response(true, "发送短信成功", new
        {
            username = user.UserName,
        }));
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmPhoneNumber(UserDataViewModel userdata)
    {
        if (userdata.Username == null || userdata.Username == string.Empty) return Ok(new Response(false, "用户名不能为空", null));
        if (userdata.Token == null || userdata.Token == string.Empty) return Ok(new Response(false, "token不能为空", null));

        User? user = await userDomainService.FindUserByNameAsync(userdata.Username);
        if (user == null) return Ok(new Response(false, "用户不存在", null));

        if (!await authenticationDomainService.VerifyByPhoneAsync(user, "ConfirmPhoneNumber", userdata.Token)) return Ok(new Response(false, "token不正确", null));

        var result = await authenticationDomainService.ConfirmPhoneNumberAsync(user);
        if (result == false) return Ok(new Response(false, "电话验证失败", null));

        logger.LogInformation($"用户 {user.Id} 通过电话认证了");
        return Ok(new Response(true, "电话验证成功", null));
    }

    [HttpPost]
    public async Task<IActionResult> SendChangeEmailToken(UserDataViewModel userdata)
    {
        if (userdata.Email == null || userdata.Email == string.Empty) return Ok(new Response(false, "用户邮箱不能为空", null));

        User? user = await userDomainService.FindUserByEmailAsync(userdata.Email);
        if (user == null) return Ok(new Response(false, "用户不存在", null));

        var result = await authenticationDomainService.SendVerifyTokenByEmailAsync(user, "ChangeEmail", "MailKit.Template.ChangeEmail");
        if (result == false) return Ok(new Response(false, "发送邮件失败", null));

        return Ok(new Response(true, "发送邮件成功", new
        {
            username = user.UserName,
        }));
    }

    [HttpPost]
    public async Task<IActionResult> ChangeEmail(UserDataViewModel userdata)
    {
        if (userdata.Username == null || userdata.Username == string.Empty) return Ok(new Response(false, "用户名不能为空", null));
        if (userdata.Token == null || userdata.Token == string.Empty) return Ok(new Response(false, "token不能为空", null));
        if (userdata.Email == null || userdata.Email == string.Empty) return Ok(new Response(false, "用户邮箱不能为空", null));

        User? user = await userDomainService.FindUserByNameAsync(userdata.Username);
        if (user == null) return Ok(new Response(false, "用户不存在", null));

        if (!await authenticationDomainService.VerifyByEmailAsync(user, "ChangeEmail", userdata.Token)) return Ok(new Response(false, "token不正确", null));

        var result = await authenticationDomainService.ChangeEmailAsync(user, userdata.Email);
        if (result == false) return Ok(new Response(false, "更改邮箱失败", null));

        logger.LogInformation($"用户 {user.Id} 更改邮箱地址了");
        return Ok(new Response(true, "更改邮箱成功", null));
    }

    [HttpPost]
    public async Task<IActionResult> SendChangePhoneNumberToken(UserDataViewModel userdata)
    {
        if (userdata.PhoneNumber == null || userdata.PhoneNumber == string.Empty) return Ok(new Response(false, "电话号码不能为空", null));

        User? user = await userDomainService.FindUserByPhoneAsync(userdata.PhoneNumber);
        if (user == null) return Ok(new Response(false, "用户不存在", null));

        var result = await authenticationDomainService.SendVerifyTokenByPhoneAsync(user, "ChangePhoneNumber", "TencentCloud.Template.ChangePhoneNumber");
        if (result == false) return Ok(new Response(false, "发送短信失败", null));

        return Ok(new Response(true, "发送短信成功", new
        {
            username = user.UserName,
        }));
    }

    [HttpPost]
    public async Task<IActionResult> ChangePhoneNumber(UserDataViewModel userdata)
    {
        if (userdata.Username == null || userdata.Username == string.Empty) return Ok(new Response(false, "用户名不能为空", null));
        if (userdata.Token == null || userdata.Token == string.Empty) return Ok(new Response(false, "token不能为空", null));
        if (userdata.PhoneNumber == null || userdata.PhoneNumber == string.Empty) return Ok(new Response(false, "电话号码不能为空", null));

        User? user = await userDomainService.FindUserByNameAsync(userdata.Username);
        if (user == null) return Ok(new Response(false, "用户不存在", null));

        if (!await authenticationDomainService.VerifyByPhoneAsync(user, "ChangePhoneNumber", userdata.Token)) return Ok(new Response(false, "token不正确", null));

        var result = await authenticationDomainService.ChangePhoneNumberAsync(user, userdata.PhoneNumber);
        if (result == false) return Ok(new Response(false, "更改电话失败", null));

        logger.LogInformation($"用户 {user.Id} 更改电话号码了");
        return Ok(new Response(true, "更改电话成功", null));
    }
}
