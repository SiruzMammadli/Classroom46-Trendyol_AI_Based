namespace Api.Application.Controllers;

[ApiController]
[Authorize]
[Route("api/users")]
public sealed class UsersController(IAuthService auth) : ControllerBase
{
    /// <summary>Token-dəki "sub" claim-dən user Id-ni götürüb cari user-in məlumatını qaytarır.</summary>
    [HttpGet("me")]
    [ProducesResponseType<UserResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> MeAsync(CancellationToken ct)
    {
        if (!Guid.TryParse(User.FindFirstValue(JwtRegisteredClaimNames.Sub), out var userId))
            return Unauthorized();

        return await auth.GetByIdAsync(userId, ct) is { } user ? Ok(user) : NotFound();
    }
}
