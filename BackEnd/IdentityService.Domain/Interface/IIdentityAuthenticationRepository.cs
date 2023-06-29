namespace IdentityService.Domain.Interface;

public interface IIdentityAuthenticationRepository
{
    /// <summary>
    /// 创建邮箱验证的token
    /// </summary>
    /// <param name="user">用户对象</param>
    /// <param name="purpose">验证作用</param>
    /// <returns>token</returns>
    Task<string> GenerateEmailVerificationTokenAsync(User user, string purpose);

    /// <summary>
    /// 通过电话验证
    /// </summary>
    /// <param name="user">用户对象</param>
    /// <param name="purpose">验证作用</param>
    /// <param name="token">token</param>
    /// <returns>是否成功</returns>
    Task<bool> VerifyByEmailAsync(User user, string purpose, string token);

    /// <summary>
    /// 创建电话验证的token
    /// </summary>
    /// <param name="user">用户对象</param>
    /// <param name="purpose">验证作用</param>
    /// <returns>token</returns>
    Task<string> GeneratePhoneVerificationTokenAsync(User user, string purpose);

    /// <summary>
    /// 通过电话验证
    /// </summary>
    /// <param name="user">用户对象</param>
    /// <param name="purpose">验证作用</param>
    /// <param name="token">token</param>
    /// <returns>是否成功</returns>
    Task<bool> VerifyByPhoneAsync(User user, string purpose, string token);

    /// <summary>
    /// 重置用户密码
    /// </summary>
    /// <param name="user">用户对象</param>
    /// <param name="newPassword">新密码</param>
    /// <returns>是否成功</returns>
    Task<bool> ResetPasswordAsync(User user, string newPassword);

    /// <summary>
    /// 确认邮箱地址
    /// </summary>
    /// <param name="user">用户对象</param>
    /// <returns>是否成功</returns>
    Task<bool> ConfirmEmailAsync(User user);

    /// <summary>
    /// 确认电话号码
    /// </summary>
    /// <param name="user">用户对象</param>
    /// <returns>是否成功</returns>
    Task<bool> ConfirmPhoneNumberAsync(User user);

    /// <summary>
    /// 更改邮箱地址
    /// </summary>
    /// <param name="user">用户对象</param>
    /// <param name="newEmail">新邮箱地址</param>
    /// <returns>是否成功</returns>
    Task<bool> ChangeEmailAsync(User user, string newEmail);

    /// <summary>
    /// 更改电话号码
    /// </summary>
    /// <param name="user">用户对象</param>
    /// <param name="newPhoneNumber">新电话号码</param>
    /// <returns>是否成功</returns>
    Task<bool> ChangePhoneNumberAsync(User user, string newPhoneNumber);
}
