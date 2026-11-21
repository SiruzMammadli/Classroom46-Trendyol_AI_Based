namespace Api.Application.Dtos;

public sealed record UserResponse(Guid Id, string FullName, string Email, DateTime CreatedAt);
