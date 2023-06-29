namespace IdentityService.Domain.Interface;

public interface IPhoneService
{
    Task<bool> SendVerifyMessageAsync(string toPhoneNumber, string templateId, string username, string token);
}
