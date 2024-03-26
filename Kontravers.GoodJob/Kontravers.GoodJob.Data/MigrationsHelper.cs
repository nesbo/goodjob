using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Kontravers.GoodJob.Data;

public class MigrationsHelper
{
    public static async Task RunDbMigrationsAsync<TDbContext>(IServiceScope serviceScope) where TDbContext : DbContext
    {
        var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<MigrationsHelper>>();
    
        static int Fibonacci(int n)
        {
            if (n is 0 or 1)
            {
                return n;
            }
    
            return Fibonacci(n - 1) + Fibonacci(n - 2);
        }
    
        static Task ExponentialBackoffAsync(int attempt)
        {
            var waitInterval = TimeSpan.FromSeconds(Fibonacci(attempt));
            return Task.Delay(waitInterval);
        }
    
        const int maxAttempts = 5;
        var attempt = 1;
        var success = false;
        Exception? startupException = null;
    
        while (attempt < maxAttempts && !success)
        {
            try
            {
                logger.LogInformation("Attempting db migration run on startup. Attempt [{StartupMigrationAttempt}]",
                    attempt);
                var db = serviceScope.ServiceProvider.GetRequiredService<TDbContext>();
                await db.Database.MigrateAsync();
                logger.LogInformation("Db migration run successful");
                success = true;
                startupException = null;
            }
            catch (Exception ex)
            {
                startupException = ex;
                logger.LogError(ex, "Error accessing db for startup migration run on attempt [{StartupMigrationAttempt}]",
                    attempt);
                attempt++;
                await ExponentialBackoffAsync(attempt);
            }
        }
    
        if (attempt >= maxAttempts && startupException != null)
        {
            logger.LogCritical("Db migration run failed. Service run FAILED");
            throw new Exception("Critical stop", startupException);
        }
    }
}