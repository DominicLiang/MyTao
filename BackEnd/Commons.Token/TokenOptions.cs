namespace Commons.Token;

public class TokenOptions
{
    // token的签发主体
    public string Issuer { get; set; } = string.Empty;
    // token的接收对象
    public string Audience { get; set; } = string.Empty;
    // token的密钥
    public string Key { get; set; } = string.Empty;
    // token的过期时间，单位为分钟
    public int Expire { get; set; }
}
