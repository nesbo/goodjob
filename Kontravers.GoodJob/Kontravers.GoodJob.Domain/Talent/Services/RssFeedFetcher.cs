using Microsoft.Extensions.Logging;

namespace Kontravers.GoodJob.Domain.Talent.Services;

public class RssFeedFetcher
{
    private readonly UpworkRssFeedFetcher _upworkRssFeedFetcher;
    private readonly ILogger<RssFeedFetcher> _logger;

    public RssFeedFetcher(UpworkRssFeedFetcher upworkRssFeedFetcher, ILogger<RssFeedFetcher> logger)
    {
        _upworkRssFeedFetcher = upworkRssFeedFetcher;
        _logger = logger;
    }
    
    public async Task StartFetchingAllAsync(CancellationToken cancellationToken)
    {
        _logger.LogTrace("Fetching RSS feeds in parallel");
        
        var upworkRssFeedFetcherTask = _upworkRssFeedFetcher.StartFetchingAllAsync(cancellationToken);
        try
        {
            await Task.WhenAll(upworkRssFeedFetcherTask);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fetching RSS feeds in parallel failed");
            return;
        }
        
        _logger.LogInformation("Fetching RSS feeds in parallel completed");
    }
}