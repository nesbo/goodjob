namespace Kontravers.GoodJob.Domain.Upwork;

public interface IUpworkCrawlerSettings
{
    int CrawlIntervalInSeconds { get; }
    int RssFeedFetchIntervalInSeconds { get; }
}