namespace IdentityService.WebAPI.ViewModels;

public class UserDataViewModel
{
    [MinLength(5, ErrorMessage = "用户名要求不小于5个字符")]
    [MaxLength(12, ErrorMessage = "用户名要求不大于12个字符")]
    public string? Username { get; set; }

    [MinLength(6, ErrorMessage = "密码要求不小于6个字符")]
    [MaxLength(24, ErrorMessage = "密码要求不大于24个字符")]
    public string? Password { get; set; }

    [EmailAddress(ErrorMessage = "邮箱地址不正确")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "电话号码不正确")]
    public string? PhoneNumber { get; set; }

    public string? Token { get; set; }
}
