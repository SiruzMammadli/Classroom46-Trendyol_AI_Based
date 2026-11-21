namespace Api.Application.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IAuthService auth) : ControllerBase
{
    /// <summary>Yeni user qeydiyyatı. Email artıq varsa 409 qaytarır.</summary>
    [HttpPost("register")]
    [ProducesResponseType<AuthResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterAsync(RegisterRequest request, CancellationToken ct) =>
        await auth.RegisterAsync(request, ct) is { } response
            ? Ok(response)
            : Conflict(new { message = "Bu email artıq qeydiyyatdan keçib." });

    /// <summary>Email və şifrə ilə login. Uğurlu olduqda JWT token qaytarır, əks halda 401.</summary>
    [HttpPost("login")]
    [ProducesResponseType<AuthResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LoginAsync(LoginRequest request, CancellationToken ct) =>
        await auth.LoginAsync(request, ct) is { } response
            ? Ok(response)
            : Unauthorized(new { message = "Email və ya şifrə yanlışdır." });
}
