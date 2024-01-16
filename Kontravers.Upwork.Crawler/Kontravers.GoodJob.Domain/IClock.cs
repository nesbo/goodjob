namespace Kontravers.GoodJob.Domain;

public interface IClock
{
    DateTime UtcNow { get; }
}