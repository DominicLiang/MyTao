namespace IdentityService.Domain.Interface;

public interface IIdentityUserDomainService
{
    /// <summary>
    /// 获得所有用户的集合
    /// </summary>
    /// <returns>用户集合</returns>
    Task<IQueryable<User>> GetUsersAsync();

    /// <summary>
    /// 检查密码是否正确
    /// </summary>
    /// <param name="user">用户对象</param>
    /// <param name="password">密码</param>
    /// <returns>是否正确</returns>
    Task<bool> CheckPasswordAsync(User user, string password);

    /// <summary>
    /// 检查用户名是否已经注册
    /// </summary>
    /// <param name="username">用户名</param>
    /// <returns>是否已注册</returns>
    Task<bool> IsRegisteredUsernameAsync(string username);

    /// <summary>
    /// 检查邮箱地址是否已经注册
    /// </summary>
    /// <param name="email">邮箱地址</param>
    /// <returns>是否已注册</returns>
    Task<bool> IsRegisteredEmailAsync(string email);

    /// <summary>
    /// 检查电话号码是否已经注册
    /// </summary>
    /// <param name="phone">电话号码</param>
    /// <returns>是否已注册</returns>
    Task<bool> IsRegisteredPhoneAsync(string phone);

    /// <summary>
    /// 按id来寻找用户，失败返回空
    /// </summary>
    /// <param name="id">id</param>
    /// <returns>用户对象</returns>
    Task<User?> FindUserByIdAsync(string id);

    /// <summary>
    /// 按用户名来寻找用户，失败返回空
    /// </summary>
    /// <param name="username">用户名</param>
    /// <returns>用户对象</returns>
    Task<User?> FindUserByNameAsync(string username);

    /// <summary>
    /// 按邮件地址来寻找用户，失败返回空
    /// </summary>
    /// <param name="email">邮件地址</param>
    /// <returns>用户对象</returns>
    Task<User?> FindUserByEmailAsync(string email);

    /// <summary>
    /// 按电话号码来寻找用户，失败返回空
    /// </summary>
    /// <param name="phone">电话号码</param>
    /// <returns>用户对象</returns>
    Task<User?> FindUserByPhoneAsync(string phone);

    /// <summary>
    /// 创建新用户，传入的用户对象只需要填基本资料（用户名、邮箱地址、电话号码）
    /// 创建新用户的时候会触发UserCreateEvent事件
    /// </summary>
    /// <param name="newUser">用户对象</param>
    /// <param name="password">密码</param>
    /// <returns>是否成功</returns>
    Task<bool> CreateUserAsync(User newUser, string password);

    /// <summary>
    /// 删除用户（软删除可恢复）,如果用户不存在返回false
    /// </summary>
    /// <param name="id">id</param>
    /// <returns>是否成功</returns>
    Task<bool> DeleteUserAsync(string id);

    /// <summary>
    /// 删除用户（永久删除），如果用户不存在返回false
    /// </summary>
    /// <param name="id">id</param>
    /// <returns>是否成功</returns>
    Task<bool> DeleteUserForRealAsync(string id);

    /// <summary>
    /// 更新用户信息
    /// </summary>
    /// <param name="user">用户对象</param>
    /// <returns>是否成功</returns>
    Task<bool> UpdateUserAsync(User user);

    /// <summary>
    /// 将用户加入某个角色，如果角色不存在会返回false，如果用户已经是这个角色还是会返回true
    /// </summary>
    /// <param name="user">用户对象</param>
    /// <param name="roleName">角色名</param>
    /// <returns>是否成功</returns>
    Task<bool> AddUserToRoleAsync(User user, string roleName);

    /// <summary>
    /// 将用户移出某个角色，如果角色不存在返回false，如果用户没有这个角色返回true
    /// </summary>
    /// <param name="user">用户对象</param>
    /// <param name="roleName">角色名</param>
    /// <returns>是否成功</returns>
    Task<bool> RemoveRoleFromUserAsync(User user, string roleName);
}
