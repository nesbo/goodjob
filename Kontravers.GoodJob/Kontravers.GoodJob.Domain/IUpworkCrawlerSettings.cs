namespace Kontravers.GoodJob.Domain;

public interface IUpworkCrawlerSettings
{
    int CrawlIntervalInSeconds { get; }
    int RssFeedFetchIntervalInSeconds { get; }
}