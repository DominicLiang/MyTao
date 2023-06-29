namespace IdentityService.Infrastructure;

public class IdentityRoleRepository : IIdentityRoleRepository
{
    private readonly RoleManager<Role> roleManager;

    public IdentityRoleRepository(RoleManager<Role> roleManager)
    {
        this.roleManager = roleManager;
    }

    public async Task<IQueryable<Role>> GetRolesAsync()
    {
        return await Task.FromResult(roleManager.Roles);
    }

    public async Task<bool> IsRoleExistsAsync(string roleName)
    {
        return await roleManager.RoleExistsAsync(roleName);
    }

    public async Task<bool> UpdateRoleAsync(Role role)
    {
        var result = await roleManager.UpdateAsync(role);
        return result.Succeeded;
    }

    public async Task<Role?> FindRoleByIdAsync(string id)
    {
        return await roleManager.FindByIdAsync(id);
    }

    public async Task<Role?> FindRoleByNameAsync(string name)
    {
        return await roleManager.FindByNameAsync(name);
    }

    public async Task<bool> CreateRoleAsync(Role role)
    {
        var result = await roleManager.CreateAsync(role);
        return result.Succeeded;
    }

    public async Task<bool> DeleteRoleForRealAsync(Role role)
    {
        var result = await roleManager.DeleteAsync(role);
        return result.Succeeded;
    }
}