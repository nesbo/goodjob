namespace Kontravers.GoodJob.Domain.Work;

public class Job : IEntity
{
    public Job(int jobStashId, string title, string url, string description, DateTime publishedAtUtc,
        string budget, string? skills = null)
    {
        JobStashId = jobStashId;
        Title = title;
        Url = url;
        Description = description;
        PublishedAtUtc = publishedAtUtc;
        Budget = budget;
        Skills = skills;
        Status = JobStatusType.Created;
    }

    public int Id { get; protected set; }
    public int JobStashId { get; }
    public JobStatusType Status { get; private set; }
    public string Title { get; }
    public string Url { get; }
    public string Description { get; }
    public DateTime PublishedAtUtc { get; }
    public string Budget { get; }
    public string? Skills { get; private set; }
}