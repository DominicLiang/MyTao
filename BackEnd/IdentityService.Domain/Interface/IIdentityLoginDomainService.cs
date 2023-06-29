namespace IdentityService.Domain.Interface;

public interface IIdentityLoginDomainService
{
    /// <summary>
    /// 没有任何认证，直接通过用户对象登录,记录生成的JWT并返回,如果登录失败JWT会返回false，登录会触发UserLoginEvent事件
    /// </summary>
    /// <param name="user">用户对象</param>
    /// <returns>JWT</returns>
    Task<string?> LoginAsync(User user);

    /// <summary>
    /// 登出，清除记录的JWT，登出会触发UserLogoutEvent事件
    /// </summary>
    /// <param name="username">id</param>
    /// <returns>是否成功</returns>
    Task<bool> LogoutAsync(string id);
}
