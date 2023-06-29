namespace IdentityService.WebAPI.ViewModels;

public class UserInfoViewModel
{
    public string UserName { get; set; } = string.Empty;
    public string? ProfilePhoto { get; set; }
    public string? NickName { get; set; }
    public int Gender { get; set; }
    public DateTime? Birthday { get; set; }
}
