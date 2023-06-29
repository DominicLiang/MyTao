namespace IdentityService.Infrastructure.Services;

public class PhoneService : IPhoneService
{
    private readonly IConfiguration configuration;

    public PhoneService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public Task<bool> SendVerifyMessageAsync(string toPhoneNumber, string templateId, string username, string token)
    {
        Credential cred = configuration.GetSection("TencentCloud.Credential").Get<Credential>()
        ?? throw new NullReferenceException("Can't get config for TencentCloud.Credential!");

        string endPoint = configuration.GetValue<string>("TencentCloud.EndPoint")
        ?? throw new NullReferenceException("Can't get config for TencentCloud.EndPoint!");

        string region = configuration.GetValue<string>("TencentCloud.Region")
        ?? throw new NullReferenceException("Can't get config for TencentCloud.Region!");

        string appId = configuration.GetValue<string>("TencentCloud.SmsSdkAppId")
        ?? throw new NullReferenceException("Can't get config for TencentCloud.SmsSdkAppId!");

        string sign = configuration.GetValue<string>("TencentCloud.SignName")
        ?? throw new NullReferenceException("Can't get config for TencentCloud.SignName!");

        string template = configuration.GetValue<string>(templateId)
        ?? throw new NullReferenceException("Can't get config for TencentCloud.TemplateId!");

        ClientProfile clientProfile = new ClientProfile();
        HttpProfile httpProfile = new HttpProfile();
        httpProfile.Endpoint = (endPoint);
        clientProfile.HttpProfile = httpProfile;
        SmsClient client = new SmsClient(cred, region, clientProfile);

        SendSmsRequest request = new SendSmsRequest();
        request.PhoneNumberSet = new string[] { toPhoneNumber };
        request.SmsSdkAppId = appId;
        request.SignName = sign;
        request.TemplateId = template;
        request.TemplateParamSet = new string[] { token };

        SendSmsResponse response = client.SendSmsSync(request);

        return Task.FromResult(response.SendStatusSet.Any(x => x.Code == "Ok"));
    }
}
