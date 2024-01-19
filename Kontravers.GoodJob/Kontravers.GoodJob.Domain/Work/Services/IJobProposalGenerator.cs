using Kontravers.GoodJob.Domain.Talent;

namespace Kontravers.GoodJob.Domain.Work.Services;

public interface IJobProposalGenerator
{
    Task GenerateAsync(Person person, Job job, CancellationToken cancellationToken);
}