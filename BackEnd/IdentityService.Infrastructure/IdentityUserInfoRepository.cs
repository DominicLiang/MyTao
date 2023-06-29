namespace IdentityService.Infrastructure;

public class IdentityUserInfoRepository : IIdentityUserInfoRepository
{
    private readonly Database.IdentityDbContext dbContext;

    public IdentityUserInfoRepository(Database.IdentityDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<UserInformation?> GetUserInfoAsync(string id)
    {
        var user = await dbContext.Users.Include(e => e.Information).FirstOrDefaultAsync(e => e.Id.ToString() == id);

        return await Task.FromResult(user?.Information);
    }

    public async Task<bool> UpdateUserInfoAsync(string userId, string? profilePhoto, string? nickName, int gender, DateTime? birthday)
    {
        var user = await dbContext.Users.Include(e => e.Information).FirstOrDefaultAsync(e => e.Id.ToString() == userId);
        if (user == null) return false;
        user.Information ??= new UserInformation();
        user.Information.ProfilePhoto = profilePhoto;
        user.Information.NickName = nickName;
        user.Information.Gender = gender;
        user.Information.Birthday = birthday;
        await dbContext.SaveChangesAsync();
        return true;
    }
}
