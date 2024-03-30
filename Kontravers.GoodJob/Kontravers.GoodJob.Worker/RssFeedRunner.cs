using Kontravers.GoodJob.Domain;
using Kontravers.GoodJob.Domain.Talent.Services;

namespace Kontravers.GoodJob.Worker;

public class RssFeedRunner : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RssFeedRunner> _logger;
    private Task _executingTask;
    private CancellationToken _stopToken;

    public RssFeedRunner(IServiceProvider serviceProvider, ILogger<RssFeedRunner> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("RSS feed runner started");
        _stopToken = stoppingToken;

        _executingTask = Task.Factory.StartNew(async () =>
        {
            while (!_stopToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();

                var upworkCrawlerSettings = scope.ServiceProvider.GetRequiredService<IUpworkCrawlerSettings>();
                var rssFeedFetcher = scope.ServiceProvider.GetRequiredService<RssFeedFetcher>();
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
        }, _stopToken);
        
        return Task.CompletedTask;
    }
}