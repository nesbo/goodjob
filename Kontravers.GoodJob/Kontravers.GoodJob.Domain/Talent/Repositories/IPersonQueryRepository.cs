namespace Kontravers.GoodJob.Domain.Talent.Repositories;

public interface IPersonQueryRepository
{
    Task<Person> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Person>> ListAsync(string organisationId, CancellationToken cancellationToken);
    Task<Person[]> ListAllAsync(CancellationToken cancellationToken);
}