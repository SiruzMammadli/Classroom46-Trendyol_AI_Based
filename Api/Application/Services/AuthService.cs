using Api.Infrastructure.Extensions;

namespace Api.Application.Services;

public sealed class AuthService(IRepository<User> users, ITokenService tokens) : IAuthService
{
    /// <summary>
    /// Yeni user qeydiyyatı: email normallaşdırılır, artıq mövcuddursa null qaytarılır (409),
    /// şifrə BCrypt ilə hash-lənir, user yadda saxlanılır və dərhal JWT token verilir.
    /// </summary>
    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request, CancellationToken ct)
    {
        var email = NormalizeEmail(request.Email);
        if (await users.AnyAsync(u => u.Email == email, ct))
            return null;

        var user = new User
        {
            FullName = request.FullName.Trim(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
        };

        await users.AddAsync(user, ct);
        await users.SaveChangesAsync(ct);
        return CreateResponse(user);
    }

    /// <summary>
    /// Login: email ilə user tapılır, şifrə BCrypt hash ilə yoxlanılır.
    /// Email və ya şifrə səhvdirsə null qaytarılır (hansının səhv olduğu bildirilmir).
    /// </summary>
    public async Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var email = NormalizeEmail(request.Email);
        var user = await users.FirstOrDefaultAsync(u => u.Email == email, ct);

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return null;

        return CreateResponse(user);
    }

    /// <summary>Id ilə user məlumatını (şifrə hash-i olmadan) qaytarır.</summary>
    public async Task<UserResponse?> GetByIdAsync(Guid id, CancellationToken ct) =>
        await users.GetByIdAsync(id, ct) is { } user ? ToResponse(user) : null;

    private AuthResponse CreateResponse(User user)
    {
        var (token, expiresAt) = tokens.CreateToken(user);
        return new AuthResponse(token, expiresAt, ToResponse(user));
    }

    private static UserResponse ToResponse(User user) =>
        new(user.Id, user.FullName, user.Email, user.CreatedAt);

    // Email-lər böyük/kiçik hərfdən asılı olmadan unikal olsun deyə
    private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();
}
