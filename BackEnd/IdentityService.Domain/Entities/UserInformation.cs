namespace IdentityService.Domain.Entities;

public class UserInformation
{
    public Guid Id { get; set; }
    public string? ProfilePhoto { get; set; }
    public string? NickName { get; set; }
    public int Gender { get; set; } 
    public DateTime? Birthday { get; set; }

    public Guid UserId { get; set; }
}
