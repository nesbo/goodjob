namespace Kontravers.Upwork.Crawler.Domain.Upwork;

public interface IUpworkCrawlerSettings
{
    int CrawlIntervalInSeconds { get; }
    int RssFeedFetchIntervalInSeconds { get; }
}