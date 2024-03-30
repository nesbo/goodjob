using Kontravers.GoodJob.Data;
using Kontravers.GoodJob.Infra.Shared;
using Kontravers.GoodJob.Worker;

var builder = Host.CreateDefaultBuilder();
builder
    .ConfigureAppConfiguration((context, configurationBuilder) =>
    {
        var environment = context.HostingEnvironment;
        
        configurationBuilder
            .AddCommandLine(args)
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{environment.EnvironmentName}.json")
            .AddEnvironmentVariables();
    })
    .ConfigureServices(services =>
    {
        services
            .AddHostedService<RssFeedRunner>()
            .AddHostedService<BrighterRunner>()
            .AddGoodJobServices()
            .AddBrighterRegistrations()
            .AddLogging();
    });

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    await MigrationsHelper.RunDbMigrationsAsync<GoodJobDbContext>(scope);
}

await host.RunAsync();