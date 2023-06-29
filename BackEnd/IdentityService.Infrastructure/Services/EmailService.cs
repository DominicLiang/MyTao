namespace IdentityService.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration configuration;

    public EmailService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public Task<bool> SendVerifyEmailAsync(string toEmailAddress, string templateId, string username, string token)
    {
        string template = configuration.GetValue<string>(templateId)
        ?? throw new NullReferenceException("Can't get config for MailKit.Template!");

        string fromName = configuration.GetValue<string>("MailKit.FromName")
        ?? throw new NullReferenceException("Can't get config for MailKit.FromName!");

        string address = configuration.GetValue<string>("MailKit.Address")
        ?? throw new NullReferenceException("Can't get config for MailKit.Address!");

        string subject = configuration.GetValue<string>("MailKit.Subject")
        ?? throw new NullReferenceException("Can't get config for MailKit.Subject!");

        string host = configuration.GetValue<string>("MailKit.Host")
        ?? throw new NullReferenceException("Can't get config for MailKit.Host!");

        string port = configuration.GetValue<string>("MailKit.Port")
        ?? throw new NullReferenceException("Can't get config for MailKit.Port!");

        string password = configuration.GetValue<string>("MailKit.Password")
        ?? throw new NullReferenceException("Can't get config for MailKit.Password!");

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(fromName, address));
        message.To.Add(new MailboxAddress(username, toEmailAddress));
        message.Subject = subject;

        message.Body = new TextPart("plain")
        {
            Text = string.Format(template, username, token)
        };

        using var client = new SmtpClient();
        client.Connect(host, Convert.ToInt32(port), false);
        client.Authenticate(address, password);
        var response = client.Send(message);
        client.Disconnect(true);

        return Task.FromResult(response.StartsWith("ok"));
    }
}
