namespace IdentityService.Domain;

public class IdentityUserDomainService : IIdentityUserDomainService
{
    private readonly IIdentityRoleRepository roleRepository;
    private readonly IIdentityUserRepository userRepository;
    private readonly IMediator mediator;

    public IdentityUserDomainService(
        IIdentityRoleRepository roleRepository,
        IIdentityUserRepository userRepository,
        IMediator mediator)
    {
        this.roleRepository = roleRepository;
        this.userRepository = userRepository;
        this.mediator = mediator;
    }

    public async Task<IQueryable<User>> GetUsersAsync()
    {
        return await userRepository.GetUsersAsync();
    }

    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
        return await userRepository.CheckPasswordAsync(user, password);
    }

    public async Task<bool> IsRegisteredUsernameAsync(string username)
    {
        return await userRepository.IsRegisteredUsernameAsync(username);
    }

    public async Task<bool> IsRegisteredEmailAsync(string email)
    {
        return await userRepository.IsRegisteredEmailAsync(email);
    }

    public async Task<bool> IsRegisteredPhoneAsync(string phone)
    {
        return await userRepository.IsRegisteredPhoneAsync(phone);
    }

    public async Task<User?> FindUserByIdAsync(string id)
    {
        return await userRepository.FindUserByIdAsync(id);
    }

    public async Task<User?> FindUserByNameAsync(string username)
    {
        return await userRepository.FindUserByNameAsync(username);
    }

    public async Task<User?> FindUserByEmailAsync(string email)
    {
        return await userRepository.FindUserByEmailAsync(email);
    }

    public async Task<User?> FindUserByPhoneAsync(string phone)
    {
        return await userRepository.FindUserByPhoneAsync(phone);
    }

    public async Task<bool> CreateUserAsync(User newUser, string password)
    {
        var result = await userRepository.CreateUserAsync(newUser, password);
        if (!result) return false;

        var user = await userRepository.FindUserByNameAsync(newUser.UserName);
        if (user == null) return false;

        user.Information = new UserInformation() { Gender = 0 };

        if (!await userRepository.UpdateUserAsync(user)) return false;

        await mediator.Publish(new UserCreateEvent(user));
        return true;
    }

    public async Task<bool> DeleteUserAsync(string id)
    {
        User? user = await userRepository.FindUserByIdAsync(id);
        if (user == null) return false;

        return await userRepository.DeleteUserAsync(user);
    }

    public async Task<bool> DeleteUserForRealAsync(string id)
    {
        User? user = await userRepository.FindUserByIdAsync(id);
        if (user == null) return false;

        return await userRepository.DeleteUserForRealAsync(user);
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        return await userRepository.UpdateUserAsync(user);
    }

    public async Task<bool> AddUserToRoleAsync(User user, string roleName)
    {
        if (!await roleRepository.IsRoleExistsAsync(roleName)) return false;
        if (await userRepository.IsInRoleAsync(user, roleName)) return true;
        return await userRepository.AddUserToRoleAsync(user, roleName);
    }

    public async Task<bool> RemoveRoleFromUserAsync(User user, string roleName)
    {
        if (!await roleRepository.IsRoleExistsAsync(roleName)) return false;
        if (!await userRepository.IsInRoleAsync(user, roleName)) return true;
        return await userRepository.RemoveRoleFromUserAsync(user, roleName);
    }
}
