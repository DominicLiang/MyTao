namespace IdentityService.Domain.Events;

public record UserLogoutEvent(User User) : INotification;
