namespace IdentityService.Domain.Interface;

public interface IIdentityUserRepository
{
    /// <summary>
    /// 获得所有用户的集合
    /// </summary>
    /// <returns>用户集合</returns>
    Task<IQueryable<User>> GetUsersAsync();

    /// <summary>
    /// 更新用户信息
    /// </summary>
    /// <param name="user">用户对象</param>
    /// <returns>是否成功</returns>
    Task<bool> UpdateUserAsync(User user);

    /// <summary>
    /// 按id来寻找用户，失败返回空
    /// </summary>
    /// <param name="id">用户id</param>
    /// <returns>用户对象</returns>
    Task<User?> FindUserByIdAsync(string id);

    /// <summary>
    /// 按用户名来寻找用户，失败返回空
    /// </summary>
    /// <param name="id">用户名</param>
    /// <returns>用户对象</returns>
    Task<User?> FindUserByNameAsync(string name);

    /// <summary>
    /// 按用户邮箱地址来寻找用户，失败返回空
    /// </summary>
    /// <param name="id">用户邮箱地址</param>
    /// <returns>用户对象</returns>
    Task<User?> FindUserByEmailAsync(string email);

    /// <summary>
    /// 按用户电话号码来寻找用户，失败返回空
    /// </summary>
    /// <param name="id">电话号码</param>
    /// <returns>用户对象</returns>
    Task<User?> FindUserByPhoneAsync(string phone);

    /// <summary>
    /// 检查用户的密码是否正确
    /// </summary>
    /// <param name="user">用户对象</param>
    /// <param name="password">密码</param>
    /// <returns>是否成功</returns>
    Task<bool> CheckPasswordAsync(User user, string password);

    /// <summary>
    /// 检查用户名是否已经注册
    /// </summary>
    /// <param name="username">用户名</param>
    /// <returns>是否成功</returns>
    Task<bool> IsRegisteredUsernameAsync(string username);

    /// <summary>
    /// 检查邮箱地址是否已经注册
    /// </summary>
    /// <param name="email">邮箱地址</param>
    /// <returns>是否成功</returns
    Task<bool> IsRegisteredEmailAsync(string email);

    /// <summary>
    /// 检查电话号码是否已经注册
    /// </summary>
    /// <param name="phone">电话号码</param>
    /// <returns>是否成功</returns>
    Task<bool> IsRegisteredPhoneAsync(string phone);

    /// <summary>
    /// 创建新用户，传入的用户对象只需要填基本资料（用户名、邮箱地址、电话号码）
    /// </summary>
    /// <param name="user">用户对象</param>
    /// <param name="password">密码</param>
    /// <returns>是否成功</returns>
    Task<bool> CreateUserAsync(User user, string password);

    /// <summary>
    /// 删除用户（软删除可恢复）
    /// </summary>
    /// <param name="user">用户对象</param>
    /// <returns>是否成功</returns>
    Task<bool> DeleteUserAsync(User user);

    /// <summary>
    /// 删除用户（永久删除）
    /// </summary>
    /// <param name="user">用户对象</param>
    /// <returns>是否成功</returns>
    Task<bool> DeleteUserForRealAsync(User user);

    /// <summary>
    /// 获得用户的所有角色名
    /// </summary>
    /// <param name="user">用户对象</param>
    /// <returns>角色名集合</returns>
    Task<IEnumerable<string>> GetRolesAsync(User user);

    /// <summary>
    /// 用户是否某个角色
    /// </summary>
    /// <param name="user">用户对象</param>
    /// <param name="role">角色名</param>
    /// <returns>是否成功</returns>
    Task<bool> IsInRoleAsync(User user, string role);

    /// <summary>
    /// 将用户加入某个角色，如果用户已经是这个角色，还是返回true
    /// </summary>
    /// <param name="user">用户对象</param>
    /// <param name="role">角色名</param>
    /// <returns>是否成功</returns>
    Task<bool> AddUserToRoleAsync(User user, string role);

    /// <summary>
    /// 将用户移出某个角色，如果用户没有这个角色，会返回false
    /// </summary>
    /// <param name="user">用户对象</param>
    /// <param name="role">角色名</param>
    /// <returns>是否成功</returns>
    Task<bool> RemoveRoleFromUserAsync(User user, string role);
}
