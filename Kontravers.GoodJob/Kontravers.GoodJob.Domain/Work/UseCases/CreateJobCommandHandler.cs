using Kontravers.GoodJob.Domain.Messaging.Commands;
using Kontravers.GoodJob.Domain.Talent;
using Kontravers.GoodJob.Domain.Talent.Repositories;
using Kontravers.GoodJob.Domain.Work.Repositories;
using Kontravers.GoodJob.Domain.Work.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Paramore.Brighter;

namespace Kontravers.GoodJob.Domain.Work.UseCases;

public class CreateJobCommandHandler : RequestHandlerAsync<CreateJobCommand>
{
    private readonly IServiceProvider _serviceProvider;
    private static readonly TimeSpan IgnoreOlderThan = TimeSpan.FromDays(1);

    public CreateJobCommandHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override async Task<CreateJobCommand> HandleAsync(
        CreateJobCommand command, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<CreateJobCommandHandler>>();
        var jobRepository = scope.ServiceProvider.GetRequiredService<IJobRepository>();
        var personQueryRepository = scope.ServiceProvider.GetRequiredService<IPersonQueryRepository>();
        var clock = scope.ServiceProvider.GetRequiredService<IClock>();
        
        logger.LogTrace("Starting processing CreateJobCommand for person [{PersonId}], with [{Uuid}]",
            command.PersonId, command.Uuid);
        
        if (clock.UtcNow - command.PublishedAtUtc > IgnoreOlderThan)
        {
            logger.LogInformation("Job title [{JobTitle}] is older than [{IgnoreOlderThan}]. Ignoring.",
                command.Title, IgnoreOlderThan);
            return await base.HandleAsync(command, cancellationToken);
        }

        var personId = int.Parse(command.PersonId);
        var jobExists = await jobRepository.ExistsAsync(personId, command.Uuid, cancellationToken);
        
        if (jobExists)
        {
            logger.LogInformation("Job with [{Uuid}] already exists for person [{PersonId}]",
                command.Uuid, command.PersonId);
            return await base.HandleAsync(command, cancellationToken);
        }
        
        var job = new Job(personId, command.Title.ReplaceLineEndings(""), command.Url,
            command.Description, command.PublishedAtUtc, command.Uuid,
            command.CreatedUtc, clock.UtcNow, JobSourceType.Upwork, 
            command.PreferredProfileId, command.PersonFeedId);
        
        var person = await personQueryRepository.GetAsync(personId, cancellationToken);
        if (person is null)
        {
            logger.LogError("Person with id [{PersonId}] not found", command.PersonId);
            return await base.HandleAsync(command, cancellationToken);
        }
        
        await GenerateProposalIfEnabledAsync(scope, command, job, person, cancellationToken);
        await SendJobEmailIfEnabledAsync(scope, command, personId, job, person, cancellationToken);

        logger.LogTrace("Saving job [{JobId}] to database", job.Id);
        await jobRepository.AddAsync(job, cancellationToken);
        await jobRepository.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Job [{JobId}] saved to database", job.Id);
        
        return await base.HandleAsync(command, cancellationToken);
    }

    private static Task GenerateProposalIfEnabledAsync(IServiceScope scope, CreateJobCommand command,
        Job job, Person person, CancellationToken cancellationToken)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<CreateJobCommandHandler>>();
        if (command.JobSource == JobSourceCommandType.Upwork)
        {
            var personUpworkRssFeed = person.UpworkRssFeeds
                .Single(f => f.Id == command.PersonFeedId);
            if (!personUpworkRssFeed.AutoGenerateProposals)
            {
                logger.LogInformation("Auto-generating proposals is disabled for person [{PersonId}] and feed [{FeedId}]",
                    person.Id, personUpworkRssFeed.Id);
                return Task.CompletedTask;
            }
        }
        
        var proposalGeneratorFactory = scope.ServiceProvider.GetRequiredService<IJobProposalGeneratorFactory>();
        var proposalGenerator = proposalGeneratorFactory.Create(JobProposalGeneratorType.ChatGpt35Turbo);
        return proposalGenerator.GenerateAsync(person, job, cancellationToken);
    }

    private static async Task SendJobEmailIfEnabledAsync(IServiceScope scope,
        CreateJobCommand command, int personId, Job job, Person person, CancellationToken cancellationToken)
    {
        if (command.JobSource == JobSourceCommandType.Upwork)
        {
            if (!person.UpworkRssFeeds.Single(f=> f.Id == command.PersonFeedId).AutoSendEmail)
            {
                return;
            }
        }
        
        var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<CreateJobCommandHandler>>();
        logger.LogTrace("Sending job email to person [{PersonId}], job title [{JobTitle}]",
            personId, job.Title);

        try
        {
            await emailSender.SendJobEmailAsync(person, job, cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error sending job email to person [{PersonId}], job title [{JobTitle}]",
                personId, job.Title);
            throw;
        }
        
        logger.LogInformation("Email sent to person [{PersonId}], job title [{JobTitle}]",
            personId, job.Title);
    }
}