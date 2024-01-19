using Kontravers.GoodJob.Domain.Work.Services;

namespace Kontravers.GoodJob.Domain.Work;

public class JobProposal : IEntity
{
    protected JobProposal() { }
    
    public JobProposal(DateTime createdUtc, DateTime insertedUtc, string text, int personId,
        JobProposalGeneratorType generatorType)
    {
        CreatedUtc = createdUtc;
        InsertedUtc = insertedUtc;
        Text = text;
        PersonId = personId;
        GeneratorType = generatorType;
    }

    public int Id { get; protected set; }
    public DateTime CreatedUtc { get; }
    public DateTime InsertedUtc { get; }
    public string Text { get; } = null!;
    public int PersonId { get; }
    public int JobId { get; protected set; }
    public bool IsValid { get; private set; }
    public JobProposalGeneratorType GeneratorType { get; }
}