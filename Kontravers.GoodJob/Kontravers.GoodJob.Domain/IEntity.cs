namespace Kontravers.GoodJob.Domain;

public interface IEntity
{
    int Id { get; }
    DateTime CreatedUtc { get; }
    DateTime InsertedUtc { get; }
}