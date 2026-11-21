namespace Api.Infrastructure.Services.Jwt;

public sealed class JwtTokenService(IOptions<JwtOptions> options, TimeProvider clock) : ITokenService
{
    /// <summary>
    /// User üçün HS256 ilə imzalanmış JWT yaradır.
    /// Claim-lər: sub (user Id), email, name. Müddət appsettings-dəki ExpireMinutes ilə təyin olunur.
    /// </summary>
    public (string Token, DateTime ExpiresAt) CreateToken(User user)
    {
        var jwt = options.Value;
        var expiresAt = clock.GetUtcNow().UtcDateTime.AddMinutes(jwt.ExpireMinutes);

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = jwt.Issuer,
            Audience = jwt.Audience,
            Expires = expiresAt,
            Claims = new Dictionary<string, object>
            {
                [JwtRegisteredClaimNames.Sub] = user.Id.ToString(),
                [JwtRegisteredClaimNames.Email] = user.Email,
                [JwtRegisteredClaimNames.Name] = user.FullName,
            },
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
                SecurityAlgorithms.HmacSha256),
        };

        return (new JsonWebTokenHandler().CreateToken(descriptor), expiresAt);
    }
}
