using Kontravers.Upwork.Crawler.Domain;
using Kontravers.Upwork.Crawler.Domain.Upwork;

namespace Kontravers.Upwork.Crawler.Worker;

public class RssFeedRunner : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RssFeedRunner> _logger;

    public RssFeedRunner(IServiceProvider serviceProvider, ILogger<RssFeedRunner> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("RSS feed runner started");

        return Task.Factory.StartNew(async () =>
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();

                var upworkCrawlerSettings = scope.ServiceProvider.GetRequiredService<IUpworkCrawlerSettings>();
                var rssFeedFetcher = scope.ServiceProvider.GetRequiredService<IRssFeedFetcher>();
                try
                {
                    _logger.LogTrace("Fetching RSS feeds");
                    await rssFeedFetcher.StartFetchingAllAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "RSS fetch failed");
                }
                
                await Task.Delay(
                    TimeSpan.FromSeconds(upworkCrawlerSettings.RssFeedFetchIntervalInSeconds), stoppingToken);
            }

            _logger.LogInformation("RSS feed runner stopped");
        }, stoppingToken);
    }
}