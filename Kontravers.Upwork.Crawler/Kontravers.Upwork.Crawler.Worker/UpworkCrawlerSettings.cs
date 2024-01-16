using Kontravers.Upwork.Crawler.Domain;
using Kontravers.Upwork.Crawler.Domain.Upwork;

namespace Kontravers.Upwork.Crawler.Worker;

public class UpworkCrawlerSettings : IUpworkCrawlerSettings
{
    public int CrawlIntervalInSeconds { get; set; }
    public int RssFeedFetchIntervalInSeconds { get; set; }
}