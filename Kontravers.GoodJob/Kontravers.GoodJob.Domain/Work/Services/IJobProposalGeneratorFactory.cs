namespace Kontravers.GoodJob.Domain.Work.Services;

public interface IJobProposalGeneratorFactory
{
    IJobProposalGenerator Create(JobProposalGeneratorType jobProposalGeneratorType);
}