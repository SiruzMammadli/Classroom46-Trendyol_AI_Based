using Api.Application.Jobs;

namespace Api.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNecessaryServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthorization();

        services.AddControllers();
        services.AddOpenApi();

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("Default")));

        services.AddSingleton(TimeProvider.System);
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddHostedService<TestJob>();

        return services;
    }
}