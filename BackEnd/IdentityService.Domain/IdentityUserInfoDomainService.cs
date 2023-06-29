namespace IdentityService.Domain;

public class IdentityUserInfoDomainService : IIdentityUserInfoDomainService
{
    private readonly IIdentityUserInfoRepository userInfoRepository;

    public IdentityUserInfoDomainService(IIdentityUserInfoRepository userInfoRepository)
    {
        this.userInfoRepository = userInfoRepository;
    }

    public async Task<UserInformation?> GetUserInfoAsync(string id)
    {
        return await userInfoRepository.GetUserInfoAsync(id);
    }

    public async Task<bool> UpdateUserInfoAsync(string userId, string? profilePhoto, string? nickName, int gender, DateTime? birthday)
    {
        return await userInfoRepository.UpdateUserInfoAsync(userId, profilePhoto, nickName, gender, birthday);
    }
}
