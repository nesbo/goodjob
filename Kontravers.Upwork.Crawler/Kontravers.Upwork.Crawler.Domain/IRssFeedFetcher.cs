namespace Kontravers.Upwork.Crawler.Domain;

public interface IRssFeedFetcher
{
    Task StartFetchingAllAsync(CancellationToken cancellationToken);
}