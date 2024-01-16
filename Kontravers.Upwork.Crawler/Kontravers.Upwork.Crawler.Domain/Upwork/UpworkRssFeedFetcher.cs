namespace Kontravers.Upwork.Crawler.Domain.Upwork;

public class UpworkRssFeedFetcher
{
    public UpworkRssFeedFetcher()
    {
        
    }
    
    public Task StartFetchingAllAsync(CancellationToken cancellationToken)
    { 
        return Task.CompletedTask;
    }
}