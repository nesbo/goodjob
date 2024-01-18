using Kontravers.GoodJob.Domain.Messaging.Commands;
using Kontravers.GoodJob.Domain.Talent.Repositories;
using Kontravers.GoodJob.Domain.Work.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Paramore.Brighter;

namespace Kontravers.GoodJob.Domain.Work.UseCases;

public class CreateJobCommandHandler : RequestHandlerAsync<CreateJobCommand>
{
    private readonly IServiceProvider _serviceProvider;

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
        
        logger.LogInformation("Starting processing CreateJobCommand for person [{PersonId}], with [{Uuid}]",
            command.PersonId, command.Uuid);

        var personId = int.Parse(command.PersonId);
        var jobExists = await jobRepository.ExistsAsync(personId, command.Uuid, cancellationToken);
        
        if (jobExists)
        {
            logger.LogWarning("Job with [{Uuid}] already exists for person [{PersonId}]",
                command.Uuid, command.PersonId);
            return await base.HandleAsync(command, cancellationToken);
        }
        
        var job = new Job(personId, command.Title.ReplaceLineEndings(""), command.Url,
            command.Description, command.PublishedAtUtc, "UnknownBudget", command.Uuid,
            command.CreatedUtc, clock.UtcNow, JobSourceType.Upwork);
        
        var person = await personQueryRepository.GetAsync(personId, cancellationToken);
        if (person is null)
        {
            logger.LogError("Person with id [{PersonId}] not found", command.PersonId);
            return await base.HandleAsync(command, cancellationToken);
        }
        
        var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
        logger.LogInformation("Sending job email to person [{PersonId}], job title [{JobTitle}]",
            personId, job.Title);

        try
        {
            await emailSender.SendJobEmailAsync(person, job, cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error sending job email to person [{PersonId}], job title [{JobTitle}]",
                personId, job.Title);
            return await base.HandleAsync(command, cancellationToken);
        }
        
        logger.LogInformation("Email sent to person [{PersonId}], job title [{JobTitle}]",
            personId, job.Title);
        
        logger.LogTrace("Saving job [{JobId}] to database", job.Id);
        await jobRepository.AddAsync(job, cancellationToken);
        await jobRepository.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Job [{JobId}] saved to database", job.Id);
        
        return await base.HandleAsync(command, cancellationToken);
    }
}