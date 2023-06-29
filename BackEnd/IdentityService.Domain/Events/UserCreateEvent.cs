namespace IdentityService.Domain.Events;

public record UserCreateEvent(User User) : INotification;

