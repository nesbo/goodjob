using Kontravers.GoodJob.Data;
using Kontravers.GoodJob.Infra.Shared;
using Kontravers.GoodJob.Worker;
using Microsoft.EntityFrameworkCore;

var configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .AddJsonFile("appsettings.json")
    .Build();

var builder = Host.CreateDefaultBuilder();
builder
    .ConfigureAppConfiguration(configurationBuilder =>
    {
        configurationBuilder.AddConfiguration(configuration);
    })
    .ConfigureServices(services =>
    {
        services
            .AddHostedService<RssFeedRunner>()
            .AddGoodJobServices()
            .AddLogging();
    });

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    await MigrationsHelper.RunDbMigrationsAsync<GoodJobDbContext>(scope);
}

await host.RunAsync();

