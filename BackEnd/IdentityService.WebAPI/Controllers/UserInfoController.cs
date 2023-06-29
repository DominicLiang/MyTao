namespace IdentityService.WebAPI.Controllers;

[Route("API/[controller]/[action]")]
[ApiController]
public class UserInfoController : ControllerBase
{
    private readonly IIdentityUserInfoDomainService userInfoDomainService;

    public UserInfoController(IIdentityUserInfoDomainService userInfoDomainService)
    {
        this.userInfoDomainService = userInfoDomainService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserInfo()
    {
        var id = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
        if (id == null) return Ok(new Response(false, "用户名不存在", null));

        var info = await userInfoDomainService.GetUserInfoAsync(id);
        if (info == null) return Ok(new Response(false, "获取用户资料失败", null));

        return Ok(new Response(true, "成功获取用户资料", new
        {
            profilePhoto = info.ProfilePhoto,
            nickName = info.NickName,
            gender = info.Gender,
            birthday = info.Birthday,
        }));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UpdateUserInfo(UserInfoViewModel userinfo)
    {
        var id = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
        if (id == null) return Ok(new Response(false, "用户名不存在", null));

        var result = await userInfoDomainService.UpdateUserInfoAsync(id, userinfo.ProfilePhoto, userinfo.NickName, userinfo.Gender, userinfo.Birthday);
        if (!result) return Ok(new Response(false, "用户信息更新失败", null));

        return Ok(new Response(false, "用户信息更新成功", null));
    }
}
