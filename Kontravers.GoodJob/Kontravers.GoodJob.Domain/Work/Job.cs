using Kontravers.GoodJob.Domain.Work.Services;

namespace Kontravers.GoodJob.Domain.Work;

public class Job : IAggregate
{
    protected Job() { }
    
    public Job(int personId, string title, string url, string description, DateTime publishedAtUtc,
        string uuid, DateTime createdUtc, DateTime insertedUtc, JobSourceType source,
        int? preferredProfileId, int personFeedId, string? skills = null)
    {
        Title = title;
        Url = url;
        Description = description;
        PublishedAtUtc = publishedAtUtc;
        Uuid = uuid;
        CreatedUtc = createdUtc;
        InsertedUtc = insertedUtc;
        Source = source;
        PreferredProfileId = preferredProfileId;
        PersonId = personId;
        Skills = skills;
        Status = JobStatusType.Created;
        PersonFeedId = personFeedId;
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
    public DateTime PublishedAtLocal => PublishedAtUtc.ToLocalTime();
    public string? Skills { get; }
    public string Uuid { get; }
    public int PersonId { get; }
    public int? PreferredProfileId { get; }
    public int PersonFeedId { get; }

    public IReadOnlyCollection<JobProposal> JobProposals
    {
        get => _jobProposals;
        init => _jobProposals = value.ToList();
    }

    public bool HasProposals => _jobProposals.Any();

    private readonly List<JobProposal> _jobProposals = new ();

    public void AddJobProposal(DateTime createdUtc, DateTime insertedUtc, string proposalText,
        JobProposalGeneratorType generatorType)
    {
        var proposal = new JobProposal(createdUtc, insertedUtc, proposalText, PersonId, generatorType);
        _jobProposals.Add(proposal);
    }
}