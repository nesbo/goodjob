namespace Kontravers.GoodJob.Domain.Talent.Repositories;

public interface IPersonRepository : IRepository<Person>
{
    Task<Person?> GetAsync(int personId, CancellationToken cancellationToken);
}