namespace Api.Application.Dtos;

public sealed record LoginRequest(
    [Required, EmailAddress] string Email,
    [Required] string Password);
