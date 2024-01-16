using Kontravers.Upwork.Crawler.Domain;
using Kontravers.Upwork.Crawler.Domain.Upwork;
using Kontravers.Upwork.Crawler.Worker;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<RssFeedRunner>();

        services.AddScoped<UpworkRssFeedFetcher>();
        services.AddScoped<IRssFeedFetcher, RssFeedFetcher>();
        
        services.AddSingleton<IUpworkCrawlerSettings>(new UpworkCrawlerSettings
        {
            CrawlIntervalInSeconds = 60,
            RssFeedFetchIntervalInSeconds = 60
        });

        services.AddLogging();
    })
    .Build();

await host.RunAsync();