using Kontravers.GoodJob.Domain.Work.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Kontravers.GoodJob.OpenAi;

public class JobProposalGeneratorFactory : IJobProposalGeneratorFactory
{
    private readonly IServiceProvider _serviceProvider;

    public JobProposalGeneratorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public IJobProposalGenerator Create(JobProposalGeneratorType jobProposalGeneratorType)
    {
        return jobProposalGeneratorType switch
        {
            JobProposalGeneratorType.ChatGpt35Turbo => _serviceProvider
                .GetRequiredService<Gpt35TurboJobProposalGenerator>(),
            _ => throw new ArgumentOutOfRangeException(nameof(jobProposalGeneratorType),
                jobProposalGeneratorType, null)
        };
    }
}