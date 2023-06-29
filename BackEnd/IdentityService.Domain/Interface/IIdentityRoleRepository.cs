namespace IdentityService.Domain.Interface;

public interface IIdentityRoleRepository
{
    /// <summary>
    /// 获得所有角色的集合
    /// </summary>
    /// <returns>角色集合</returns>
    Task<IQueryable<Role>> GetRolesAsync();

    /// <summary>
    /// 检查是否有某个角色
    /// </summary>
    /// <param name="roleName">角色名</param>
    /// <returns>是否成功</returns>
    Task<bool> IsRoleExistsAsync(string roleName);

    /// <summary>
    /// 更新角色信息，一般是角色名
    /// </summary>
    /// <param name="role">角色对象</param>
    /// <returns>是否成功</returns>
    Task<bool> UpdateRoleAsync(Role role);

    /// <summary>
    /// 按id来寻找角色，失败返回空
    /// </summary>
    /// <param name="id">角色id</param>
    /// <returns>角色对象</returns>
    Task<Role?> FindRoleByIdAsync(string id);

    /// <summary>
    /// 按角色名来寻找角色，失败返回空
    /// </summary>
    /// <param name="id">角色id</param>
    /// <returns>角色对象</returns>
    Task<Role?> FindRoleByNameAsync(string name);

    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="role">角色对象</param>
    /// <returns>是否成功</returns>
    Task<bool> CreateRoleAsync(Role role);

    /// <summary>
    /// 删除角色（永久删除）
    /// </summary>
    /// <param name="role">角色对象</param>
    /// <returns>是否成功</returns>
    Task<bool> DeleteRoleForRealAsync(Role role);
}
