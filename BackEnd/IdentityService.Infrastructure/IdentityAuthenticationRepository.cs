namespace IdentityService.Infrastructure;

public class IdentityAuthenticationRepository : IIdentityAuthenticationRepository
{
    private readonly UserManager<User> userManager;

    public IdentityAuthenticationRepository(UserManager<User> userManager)
    {
        this.userManager = userManager;
    }

    public async Task<string> GenerateEmailVerificationTokenAsync(User user, string purpose)
    {
        return await userManager.GenerateUserTokenAsync(user, "EmailTokenProvider", purpose);
    }

    public async Task<bool> VerifyByEmailAsync(User user, string purpose, string token)
    {
        return await userManager.VerifyUserTokenAsync(user, "EmailTokenProvider", purpose, token);
    }

    public async Task<string> GeneratePhoneVerificationTokenAsync(User user, string purpose)
    {
        return await userManager.GenerateUserTokenAsync(user, "PhoneTokenProvider", purpose);
    }

    public async Task<bool> VerifyByPhoneAsync(User user, string purpose, string token)
    {
        return await userManager.VerifyUserTokenAsync(user, "PhoneTokenProvider", purpose, token);
    }

    public async Task<bool> ResetPasswordAsync(User user, string newPassword)
    {
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var result = await userManager.ResetPasswordAsync(user, newPassword, token);

        return result.Succeeded;
    }

    public async Task<bool> ConfirmEmailAsync(User user)
    {
        user.EmailConfirmed = true;
        var result = await userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task<bool> ConfirmPhoneNumberAsync(User user)
    {
        user.PhoneNumberConfirmed = true;
        var result = await userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task<bool> ChangeEmailAsync(User user, string newEmail)
    {
        user.Email = newEmail;
        user.EmailConfirmed = true;
        var result = await userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task<bool> ChangePhoneNumberAsync(User user, string newPhoneNumber)
    {
        user.PhoneNumber = newPhoneNumber;
        user.PhoneNumberConfirmed = true;
        var result = await userManager.UpdateAsync(user);
        return result.Succeeded;
    }
}
