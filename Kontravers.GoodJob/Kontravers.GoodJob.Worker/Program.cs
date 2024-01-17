using Kontravers.GoodJob.Data;
using Kontravers.GoodJob.Data.Repositories.Talent;
using Kontravers.GoodJob.Domain;
using Kontravers.GoodJob.Domain.Talent.Repositories;
using Kontravers.GoodJob.Domain.Work;
using Kontravers.GoodJob.Worker;
using Microsoft.EntityFrameworkCore;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<RssFeedRunner>();

        services.AddScoped<UpworkRssFeedFetcher>();
        services.AddScoped<IRssFeedFetcher, RssFeedFetcher>();
        services.AddScoped<IClock, Clock>();
        services.AddScoped<IPersonQueryRepository, PersonQueryRepository>();

        services.AddDbContext<GoodJobDbContext>(options =>
        {
            options.UseNpgsql("Host=postgres;Database=goodjob;Username=postgres;Password=Password1!;Timezone=UTC");
        });
        
        services.AddSingleton<IUpworkCrawlerSettings>(new UpworkCrawlerSettings
        {
            CrawlIntervalInSeconds = 60,
            RssFeedFetchIntervalInSeconds = 60
        });

        services.AddLogging();
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    await RunDbMigrationsAsync(scope);
}

await host.RunAsync();

static async Task RunDbMigrationsAsync(IServiceScope serviceScope)
{
    var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<Program>>();

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
            var db = serviceScope.ServiceProvider.GetRequiredService<GoodJobDbContext>();
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