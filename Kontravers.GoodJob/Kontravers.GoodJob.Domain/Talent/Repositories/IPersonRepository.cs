namespace Kontravers.GoodJob.Domain.Talent.Repositories;

public interface IPersonRepository : IRepository<Person>
{
    Task<Person?> GetAsync(int personId, CancellationToken cancellationToken);
    Task<bool> ExistsForOrganisationAsync(string email, int organizationId, CancellationToken cancellationToken);
    Task<Person?> GetByUserIdAsync(string userId, CancellationToken cancellationToken);
}