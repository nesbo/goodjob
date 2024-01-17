namespace Kontravers.GoodJob.Domain.Work;

public class JobStash : IAggregate
{
    public int Id { get; protected set; }

    public ICollection<Job> Jobs { get; private set; } = new List<Job>();
    public DateTime CreatedUtc { get; private set; }
    public string PersonId { get; private set; }
    
    public JobStash(string personId, DateTime createdUtc)
    {
        PersonId = personId;
        CreatedUtc = createdUtc;
    }
}