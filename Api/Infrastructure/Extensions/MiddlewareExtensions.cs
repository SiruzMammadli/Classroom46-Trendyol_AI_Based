namespace Api.Infrastructure.Extensions;

public static class MiddlewareExtensions
{
    public static WebApplication UseNecessaryMiddleware(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}
