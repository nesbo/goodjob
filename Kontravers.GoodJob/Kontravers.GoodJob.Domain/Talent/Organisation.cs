namespace Kontravers.GoodJob.Domain.Talent;

public class Organisation : IAggregate
{
    protected Organisation() { }
    
    public Organisation(int id, string name, string? description,
        DateTime createdUtc, DateTime insertedUtc)
    {
        Id = id;
        Name = name;
        Description = description;
        CreatedUtc = createdUtc;
        InsertedUtc = insertedUtc;
    }

    public int Id { get; protected set; }
    public DateTime CreatedUtc { get; }
    public DateTime InsertedUtc { get; }
    public string Name { get; }
    public string? Description { get; }
}