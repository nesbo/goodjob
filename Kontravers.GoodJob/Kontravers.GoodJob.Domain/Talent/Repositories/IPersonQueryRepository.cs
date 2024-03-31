namespace Kontravers.GoodJob.Domain.Talent.Repositories;

public interface IPersonQueryRepository
{
    Task<Person?> GetAsync(int id, CancellationToken cancellationToken);
    
    Task<Person[]> ListAllAsync(CancellationToken cancellationToken, bool includeDisabled = false);
    Task<Person?> GetByUserId(string userId, CancellationToken cancellationToken);
}