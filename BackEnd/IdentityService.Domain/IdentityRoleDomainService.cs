namespace IdentityService.Domain;

public class IdentityRoleDomainService : IIdentityRoleDomainService
{
    private readonly IIdentityRoleRepository roleRepository;

    public IdentityRoleDomainService(IIdentityRoleRepository roleRepository)
    {
        this.roleRepository = roleRepository;
    }

    public async Task<IQueryable<Role>> GetRolesAsync()
    {
        return await roleRepository.GetRolesAsync();
    }

    public async Task<bool> IsRoleExistsAsync(string roleName)
    {
        return await roleRepository.IsRoleExistsAsync(roleName);
    }

    public async Task<Role?> FindRoleByIdAsync(string id)
    {
        return await roleRepository.FindRoleByIdAsync(id);
    }

    public async Task<Role?> FindRoleByNameAsync(string rolename)
    {
        return await roleRepository.FindRoleByNameAsync(rolename);
    }

    public async Task<bool> CreateRoleAsync(Role role)
    {
        if (await roleRepository.IsRoleExistsAsync(role.Name)) return true;
        return await roleRepository.CreateRoleAsync(role);
    }

    public async Task<bool> DeleteRoleForRealAsync(Role role)
    {
        if (!await roleRepository.IsRoleExistsAsync(role.Name)) return true;
        return await roleRepository.DeleteRoleForRealAsync(role);
    }

    public async Task<bool> UpdateRoleInfoAsync(Role role)
    {
        return await roleRepository.UpdateRoleAsync(role);
    }
}
