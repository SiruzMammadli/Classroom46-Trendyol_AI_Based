using System.Linq;
using System.Text.Json;

namespace Api.Application.Jobs;

public sealed class TestJob(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new(TimeSpan.FromSeconds(5));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            using var context = serviceProvider.CreateScope()
                .ServiceProvider.GetRequiredService<AppDbContext>();

            var list = context.Users.ToList();

            Console.WriteLine(JsonSerializer.Serialize(list));
        }
    }
}