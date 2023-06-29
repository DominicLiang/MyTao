namespace IdentityService.Domain.Interface;

public interface IEmailService
{
    public Task<bool> SendVerifyEmailAsync(string toEmailAddress, string templateId, string username, string token);
}
