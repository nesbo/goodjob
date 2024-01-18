using Kontravers.GoodJob.Data;
using Kontravers.GoodJob.Data.Repositories.Talent;
using Kontravers.GoodJob.Data.Repositories.Work;
using Kontravers.GoodJob.Domain;
using Kontravers.GoodJob.Domain.Messaging;
using Kontravers.GoodJob.Domain.Talent.Repositories;
using Kontravers.GoodJob.Domain.Talent.Services;
using Kontravers.GoodJob.Domain.Work.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Paramore.Brighter.Extensions.DependencyInjection;

namespace Kontravers.GoodJob.Infra.Shared;

public static class GoodJobServiceRegistrationsExtensions
{
    public static IServiceCollection AddGoodJobServices(this IServiceCollection services)
    {
        services.AddScoped<UpworkRssFeedFetcher>();
        services.AddScoped<RssFeedFetcher>();
        services.AddScoped<IClock, Clock>();
        services.AddScoped<IPersonQueryRepository, PersonQueryRepository>();
        services.AddScoped<IJobRepository, JobRepository>();
        services.AddScoped<ICommandPublisher, CommandPublisher>();
        services.AddScoped<IHttpClient, HttpClient>();
        services.AddScoped<IEmailSender, EmailSender>();

        services.AddDbContext<GoodJobDbContext>(options =>
        {
            options.UseNpgsql("Host=postgres;Database=goodjob;Username=postgres;Password=Password1!;Timezone=UTC");
        });
        
        services.AddSingleton<IUpworkCrawlerSettings>(new UpworkCrawlerSettings
        {
            CrawlIntervalInSeconds = 60,
            RssFeedFetchIntervalInSeconds = 60
        });

        return services;
    }
    
    public static IServiceCollection AddBrighterRegistrations(this IServiceCollection services)
    {
        services.AddBrighter().AutoFromAssemblies();
        return services;
    }
}