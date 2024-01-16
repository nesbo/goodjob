using Kontravers.GoodJob.Domain.Talent;
using Kontravers.GoodJob.Domain.Talent.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kontravers.GoodJob.Data.Repositories.Talent;

public class PersonQueryRepository : IPersonQueryRepository
{
    private readonly GoodJobDbContext _dbContext;

    public PersonQueryRepository(GoodJobDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task<Person> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Person>> ListAsync(string organisationId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Person[]> ListAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext
            .Set<Person>()
            .Include(p => p.UpworkRssFeeds)
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);
    }
}