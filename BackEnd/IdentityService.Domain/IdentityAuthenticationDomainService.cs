namespace IdentityService.Domain;

public class IdentityAuthenticationDomainService : IIdentityAuthenticationDomainService
{
    private readonly IIdentityAuthenticationRepository authenticationRepository;
    private readonly IEmailService emailService;
    private readonly IPhoneService phoneService;

    public IdentityAuthenticationDomainService(
        IIdentityAuthenticationRepository authenticationRepository,
        IEmailService emailService,
        IPhoneService phoneService)
    {
        this.authenticationRepository = authenticationRepository;
        this.emailService = emailService;
        this.phoneService = phoneService;
    }

    public async Task<bool> SendVerifyTokenByEmailAsync(User user, string purpose, string template)
    {
        var token = await authenticationRepository.GenerateEmailVerificationTokenAsync(user, purpose);
        return await emailService.SendVerifyEmailAsync(user.Email, template, user.UserName, token);
    }

    public async Task<bool> VerifyByEmailAsync(User user, string purpose, string token)
    {
        return await authenticationRepository.VerifyByEmailAsync(user, purpose, token);
    }

    public async Task<bool> SendVerifyTokenByPhoneAsync(User user, string purpose, string template)
    {
        var token = await authenticationRepository.GeneratePhoneVerificationTokenAsync(user, purpose);
        return await phoneService.SendVerifyMessageAsync(user.PhoneNumber, template, user.UserName, token);
    }

    public async Task<bool> VerifyByPhoneAsync(User user, string purpose, string token)
    {
        return await authenticationRepository.VerifyByPhoneAsync(user, purpose, token);
    }

    public async Task<bool> ResetPasswordAsync(User user, string newPassword)
    {
        return await authenticationRepository.ResetPasswordAsync(user, newPassword);
    }

    public async Task<bool> ConfirmEmailAsync(User user)
    {
        return await authenticationRepository.ConfirmEmailAsync(user);
    }

    public async Task<bool> ConfirmPhoneNumberAsync(User user)
    {
        return await authenticationRepository.ConfirmPhoneNumberAsync(user);
    }

    public async Task<bool> ChangeEmailAsync(User user, string newEmail)
    {
        return await authenticationRepository.ChangeEmailAsync(user, newEmail);
    }

    public async Task<bool> ChangePhoneNumberAsync(User user, string newPhoneNumber)
    {
        return await authenticationRepository.ChangePhoneNumberAsync(user, newPhoneNumber);
    }
}
