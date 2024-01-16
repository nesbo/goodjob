using Kontravers.GoodJob.Domain.Upwork;

namespace Kontravers.GoodJob.Worker;

public class UpworkCrawlerSettings : IUpworkCrawlerSettings
{
    public int CrawlIntervalInSeconds { get; set; }
    public int RssFeedFetchIntervalInSeconds { get; set; }
}