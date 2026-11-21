namespace Api.Application.Dtos;

public sealed record RegisterRequest(
    [Required, MaxLength(100)] string FullName,
    [Required, EmailAddress, MaxLength(256)] string Email,
    [Required, MinLength(6), MaxLength(100)] string Password);
