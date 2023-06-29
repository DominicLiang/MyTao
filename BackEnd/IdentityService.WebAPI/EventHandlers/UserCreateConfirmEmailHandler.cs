namespace IdentityService.WebAPI.EventHandlers;

public class UserCreateConfirmEmailHandler : NotificationHandler<UserCreateEvent>
{
    private readonly IIdentityAuthenticationDomainService authenticationDomainService;

    public UserCreateConfirmEmailHandler(
        IIdentityAuthenticationDomainService authenticationDomainService)
    {
        this.authenticationDomainService = authenticationDomainService;
    }

    protected override async void Handle(UserCreateEvent notification)
    {
        await authenticationDomainService.SendVerifyTokenByEmailAsync(notification.User,"ConfirmEmail", "MailKit.Template.Confirm");
    }
}
