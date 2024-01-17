using Kontravers.GoodJob.Data;
using Kontravers.GoodJob.Domain;
using Kontravers.GoodJob.Domain.Work;
using Kontravers.GoodJob.Worker;
using Microsoft.EntityFrameworkCore;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<RssFeedRunner>();

        services.AddScoped<UpworkRssFeedFetcher>();
        services.AddScoped<IRssFeedFetcher, RssFeedFetcher>();

        services.AddDbContext<GoodJobDbContext>(options =>
        {
            options.UseNpgsql("Host=localhost;Database=goodjob;Username=postgres;Password=postgres");
        });
        
        services.AddSingleton<IUpworkCrawlerSettings>(new UpworkCrawlerSettings
        {
            CrawlIntervalInSeconds = 60,
            RssFeedFetchIntervalInSeconds = 60
        });

        services.AddLogging();
    })
    .Build();

await host.RunAsync();