namespace Kontravers.GoodJob.Domain.Upwork;

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