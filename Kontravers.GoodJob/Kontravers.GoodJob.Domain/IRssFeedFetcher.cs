namespace Kontravers.GoodJob.Domain;

public interface IRssFeedFetcher
{
    Task StartFetchingAllAsync(CancellationToken cancellationToken);
}