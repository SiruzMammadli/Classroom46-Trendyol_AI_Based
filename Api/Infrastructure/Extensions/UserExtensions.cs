namespace Api.Infrastructure.Extensions;

public static class UserExtensions
{
    public static User UpdatePassword(this User user, string passwordHash)
    {
        user.PasswordHash = passwordHash;

        return user;
    }
}