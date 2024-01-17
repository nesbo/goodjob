using Kontravers.GoodJob.Domain.Talent;
using Kontravers.GoodJob.Domain.Talent.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kontravers.GoodJob.Data.Repositories.Talent;

public class PersonQueryRepository : QueryRepositoryBase<Person>, IPersonQueryRepository
{
    public PersonQueryRepository(GoodJobDbContext dbContext) : base(dbContext)
    {
    }

    public Task<IEnumerable<Person>> ListAsync(string organisationId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Person[]> ListAllAsync(CancellationToken cancellationToken)
    {
        return await Query
            .Include(p => p.UpworkRssFeeds)
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);
    }
}