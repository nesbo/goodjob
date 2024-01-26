using Kontravers.GoodJob.Data;
using Kontravers.GoodJob.Data.Repositories.Talent;
using Kontravers.GoodJob.Data.Repositories.Work;
using Kontravers.GoodJob.Domain;
using Kontravers.GoodJob.Domain.Messaging;
using Kontravers.GoodJob.Domain.Talent.Repositories;
using Kontravers.GoodJob.Domain.Talent.Services;
using Kontravers.GoodJob.Domain.Talent.UseCases;
using Kontravers.GoodJob.Domain.Work.Repositories;
using Kontravers.GoodJob.Domain.Work.Services;
using Kontravers.GoodJob.OpenAi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Paramore.Brighter.Extensions.DependencyInjection;

namespace Kontravers.GoodJob.Infra.Shared;

public static class GoodJobServiceRegistrationsExtensions
{
    public static IServiceCollection AddGoodJobServices(this IServiceCollection services)
    {
        var isDevelopment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") == Environments.Development;
        services.AddScoped<UpworkRssFeedFetcher>();
        services.AddScoped<RssFeedFetcher>();
        services.AddScoped<IClock, Clock>();
        services.AddScoped<IPersonQueryRepository, PersonQueryRepository>();
        services.AddScoped<IJobRepository, JobRepository>();
        services.AddScoped<ICommandPublisher, CommandPublisher>();
        services.AddScoped<IHttpClient, HttpClient>();
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<Gpt35TurboJobProposalGenerator>();
        services.AddScoped<IJobProposalGeneratorFactory, JobProposalGeneratorFactory>();
        services.AddScoped<ListPersonsQueryHandler>();
        services.AddScoped<GetPersonQueryHandler>();
        services.AddScoped<GetPersonUpworkRssFeedQueryHandler>();

        services.AddDbContext<GoodJobDbContext>(options =>
        {
            var connectionString = isDevelopment 
                ? "Host=localhost;Database=goodjob;Username=postgres;Password=Password1!;Timezone=UTC"
                : "Host=postgres;Database=goodjob;Username=postgres;Password=Password1!;Timezone=UTC";
            
            options.UseNpgsql(connectionString);
        });
        
        services.AddSingleton<IUpworkCrawlerSettings>(new UpworkCrawlerSettings
        {
            CrawlIntervalInSeconds = 60,
            RssFeedFetchIntervalInSeconds = 60
        });

        services.AddBrighter().AutoFromAssemblies();

        return services;
    }
}