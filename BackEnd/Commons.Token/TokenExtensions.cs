namespace Commons.Token;

public static class TokenExtensions
{
    public static string CreateToken(this TokenOptions options, IEnumerable<Claim> claims)
    {
        DateTime expires = DateTime.Now.AddMinutes(options.Expire);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new JwtSecurityToken(options.Issuer, options.Audience, claims, DateTime.Now, expires, credentials);

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}
