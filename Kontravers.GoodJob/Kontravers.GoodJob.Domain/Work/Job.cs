namespace Kontravers.GoodJob.Domain.Work;

public class Job : IAggregate
{
    protected Job() { }
    
    public Job(int personId, string title, string url, string description, DateTime publishedAtUtc,
        string budget, string uuid, DateTime createdUtc, DateTime insertedUtc, JobSourceType source,
        string? skills = null)
    {
        Title = title;
        Url = url;
        Description = description;
        PublishedAtUtc = publishedAtUtc;
        Budget = budget;
        Uuid = uuid;
        CreatedUtc = createdUtc;
        InsertedUtc = insertedUtc;
        Source = source;
        PersonId = personId;
        Skills = skills;
        Status = JobStatusType.Created;
    }

    public int Id { get; protected set; }
    public DateTime CreatedUtc { get; }
    public DateTime InsertedUtc { get; }
    public JobStatusType Status { get;  }
    public JobSourceType Source { get; }
    public string Title { get; }
    public string Url { get; }
    public string Description { get; }
    public DateTime PublishedAtUtc { get; }
    public string Budget { get; }
    public string? Skills { get; }
    public string Uuid { get; }
    public int PersonId { get; }
}