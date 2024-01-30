using Kontravers.GoodJob.Domain.Talent.Commands;

namespace Kontravers.GoodJob.Domain.Talent;

public class Profile : IEntity
{
    protected Profile() { }
    
    public Profile(DateTime createdUtc, DateTime insertedUtc, string title, string description, string? skills)
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
    public string Title { get; private set; }
    public string Description { get; private set; }
    public int PersonId { get; protected set; }
    public string? Skills { get; private set; }

    public void Update(UpdatePersonProfileCommand command)
    {
        Title = command.Title;
        Description = command.Description;
        Skills = command.Skills;
    }
}