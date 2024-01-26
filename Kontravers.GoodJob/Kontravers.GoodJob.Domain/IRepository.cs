namespace Kontravers.GoodJob.Domain;

public interface IRepository<T> where T : class, IAggregate
{
    Task AddAsync(T aggregate, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}