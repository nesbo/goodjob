namespace Kontravers.GoodJob.Domain.Work.Repositories;

public interface IJobRepository
{
    Task<Job?> GetAsync(int id, CancellationToken cancellationToken);
    Task<Job?> GetAsync(int personId, string uuid, CancellationToken cancellationToken);
    Task AddAsync(Job jobStash, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task<bool> ExistsAsync(int personId, string commandUuid, CancellationToken cancellationToken);
}