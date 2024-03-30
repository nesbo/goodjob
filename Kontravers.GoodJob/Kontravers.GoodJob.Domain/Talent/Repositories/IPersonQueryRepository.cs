namespace Kontravers.GoodJob.Domain.Talent.Repositories;

public interface IPersonQueryRepository
{
    Task<Person?> GetAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Person>> ListAsync(string organisationId, CancellationToken cancellationToken);
    Task<Person[]> ListAllAsync(CancellationToken cancellationToken, bool includeDisabled = false);
}