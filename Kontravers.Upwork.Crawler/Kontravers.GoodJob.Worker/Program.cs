using Kontravers.GoodJob.Domain;
using Kontravers.GoodJob.Domain.Upwork;
using Kontravers.GoodJob.Worker;

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