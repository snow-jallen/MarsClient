using Microsoft.Extensions.Logging;

namespace ConsoleClient;

public class MigrationApplier : IHostedService
{
    private readonly IServiceProvider service;
    private readonly ILogger<MigrationApplier> logger;

    public MigrationApplier(IServiceProvider service, ILogger<MigrationApplier> logger)
    {
        this.service = service;
        this.logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = service.CreateScope())
        {
            try
            {
                var context = scope.ServiceProvider.GetRequiredService<Context>();
                logger.LogInformation("Applying migrations...");
                context.Database.Migrate();
                logger.LogInformation("Migrations applied successfully!");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "***  Trouble applying migrations!");

                throw;
            }
            return Task.CompletedTask;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
