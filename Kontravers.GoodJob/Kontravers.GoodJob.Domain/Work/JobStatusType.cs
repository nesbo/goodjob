namespace Kontravers.GoodJob.Domain.Work;

public enum JobStatusType : byte
{
    None = 0,
    Created,
    NotRelevant,
    InProgress,
    NotApplied,
    Applied
}