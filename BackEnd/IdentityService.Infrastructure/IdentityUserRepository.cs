namespace IdentityService.Infrastructure;

public class IdentityUserRepository : IIdentityUserRepository
{
    private readonly UserManager<User> userManager;

    public IdentityUserRepository(UserManager<User> userManager)
    {
        this.userManager = userManager;
    }

    public async Task<IQueryable<User>> GetUsersAsync()
    {
        return await Task.FromResult(userManager.Users);
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        var result = await userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task<User?> FindUserByIdAsync(string id)
    {
        return await userManager.FindByIdAsync(id);
    }

    public async Task<User?> FindUserByNameAsync(string name)
    {
        return await userManager.FindByNameAsync(name);
    }

    public async Task<User?> FindUserByEmailAsync(string email)
    {
        return await userManager.FindByEmailAsync(email);
    }

    public async Task<User?> FindUserByPhoneAsync(string phone)
    {
        return await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phone);
    }

    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
        return await userManager.CheckPasswordAsync(user, password);
    }

    public async Task<bool> IsRegisteredUsernameAsync(string username)
    {
        return await userManager.Users.AnyAsync(u => u.UserName == username);
    }

    public async Task<bool> IsRegisteredEmailAsync(string email)
    {
        return await userManager.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<bool> IsRegisteredPhoneAsync(string phone)
    {
        return await userManager.Users.AnyAsync(u => u.PhoneNumber == phone);
    }

    public async Task<bool> CreateUserAsync(User user, string password)
    {
        var result = await userManager.CreateAsync(user, password);
        return result.Succeeded;
    }

    public async Task<bool> DeleteUserAsync(User user)
    {
        user.SoftDelete();
        var result = await userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task<bool> DeleteUserForRealAsync(User user)
    {
        var reuslt = await userManager.DeleteAsync(user);
        return reuslt.Succeeded;
    }

    public async Task<IEnumerable<string>> GetRolesAsync(User user)
    {
        return await userManager.GetRolesAsync(user);
    }

    public async Task<bool> IsInRoleAsync(User user, string role)
    {
        return await userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AddUserToRoleAsync(User user, string role)
    {
        if (await userManager.IsInRoleAsync(user, role)) return true;
        var result = await userManager.AddToRoleAsync(user, role);
        return result.Succeeded;
    }

    public async Task<bool> RemoveRoleFromUserAsync(User user, string role)
    {
        if (!await userManager.IsInRoleAsync(user, role)) return false;
        var result = await userManager.RemoveFromRoleAsync(user, role);
        return result.Succeeded;
    }
}
