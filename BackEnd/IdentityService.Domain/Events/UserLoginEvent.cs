namespace IdentityService.Domain.Events;

public record UserLoginEvent(User User) : INotification;

