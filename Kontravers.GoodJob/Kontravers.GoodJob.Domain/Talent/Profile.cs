namespace Kontravers.GoodJob.Domain.Talent;

public class Profile : IEntity
{
    protected Profile() { }
    
    public Profile(DateTime createdUtc, DateTime insertedUtc,
        string title, string description, string? skills)
    {
        CreatedUtc = createdUtc;
        InsertedUtc = insertedUtc;
        Title = title;
        Description = description;
        Skills = skills;
    }

    public int Id { get; protected set; }
    public DateTime CreatedUtc { get; }
    public DateTime InsertedUtc { get; }
    public string Title { get; }
    public string Description { get; }
    public int PersonId { get; protected set; }
    public string? Skills { get; }
}