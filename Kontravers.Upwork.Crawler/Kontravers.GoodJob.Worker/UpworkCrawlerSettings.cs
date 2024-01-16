using Kontravers.GoodJob.Domain;

namespace Kontravers.GoodJob.Worker;

public class UpworkCrawlerSettings : IUpworkCrawlerSettings
{
    public int CrawlIntervalInSeconds { get; set; }
    public int RssFeedFetchIntervalInSeconds { get; set; }
}