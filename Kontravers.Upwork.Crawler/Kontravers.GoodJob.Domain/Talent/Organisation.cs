namespace Kontravers.GoodJob.Domain.Talent;

public class Organisation : IAggregate
{
    public Organisation(int id, string name, string? description, IEnumerable<string> personIds)
    {
        Id = id;
        Name = name;
        Description = description;
        PersonIds = personIds;
    }

    public int Id { get; protected set; }
    
    public string Name { get; }
    
    public string? Description { get; }
    
    public IEnumerable<string> PersonIds { get; }
}