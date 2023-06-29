namespace IdentityService.Domain.Interface;

public interface IIdentityRoleDomainService
{
    /// <summary>
    /// 获得所有角色的集合
    /// </summary>
    /// <returns>角色集合</returns>
    Task<IQueryable<Role>> GetRolesAsync();

    /// <summary>
    /// 通过角色名确定角色是否存在
    /// </summary>
    /// <param name="roleName">角色名</param>
    /// <returns>角色是否存在</returns>
    Task<bool> IsRoleExistsAsync(string roleName);

    /// <summary>
    /// 通过id来寻找角色，失败返回空
    /// </summary>
    /// <param name="id">id</param>
    /// <returns>角色对象</returns>
    Task<Role?> FindRoleByIdAsync(string id);

    /// <summary>
    /// 通过角色名来寻找角色，失败返回空
    /// </summary>
    /// <param name="rolename">角色名</param>
    /// <returns>角色对象</returns>
    Task<Role?> FindRoleByNameAsync(string rolename);

    /// <summary>
    /// 创建角色，只需要填角色名,如果角色已经存在还是返回true
    /// </summary>
    /// <param name="role">角色对象</param>
    /// <returns>是否成功</returns>
    Task<bool> CreateRoleAsync(Role role);

    /// <summary>
    /// 删除角色（永久删除）如果角色不存在还是返回true
    /// </summary>
    /// <param name="role">角色对象</param>
    /// <returns>是否成功</returns>
    Task<bool> DeleteRoleForRealAsync(Role role);

    /// <summary>
    /// 更新角色信息，一般是角色名
    /// </summary>
    /// <param name="role">角色对象</param>
    /// <returns>是否成功</returns>
    Task<bool> UpdateRoleInfoAsync(Role role);
}
