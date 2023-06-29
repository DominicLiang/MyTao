namespace IdentityService.Domain.Interface;

public interface IIdentityUserInfoRepository
{
    /// <summary>
    /// 获取用户资料
    /// </summary>
    /// <param name="id">id</param>
    /// <returns>用户资料对象</returns>
    Task<UserInformation?> GetUserInfoAsync(string id);

    /// <summary>
    /// 更新用户资料
    /// </summary>
    /// <param name="userId">用户id</param>
    /// <param name="profilePhoto">用户头像链接</param>
    /// <param name="nickName">昵称</param>
    /// <param name="gender">性别</param>
    /// <param name="birthday">生日</param>
    /// <returns>是否成功</returns>
    Task<bool> UpdateUserInfoAsync(string userId, string? profilePhoto, string? nickName, int gender, DateTime? birthday);
}
