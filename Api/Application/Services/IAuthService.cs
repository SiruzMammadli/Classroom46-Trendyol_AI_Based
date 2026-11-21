namespace Api.Application.Services;

public interface IAuthService
{
    Task<AuthResponse?> RegisterAsync(RegisterRequest request, CancellationToken ct);
    Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken ct);
    Task<UserResponse?> GetByIdAsync(Guid id, CancellationToken ct);
}
