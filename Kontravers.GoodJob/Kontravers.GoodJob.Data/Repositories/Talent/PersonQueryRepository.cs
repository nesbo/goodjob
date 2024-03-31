using Kontravers.GoodJob.Domain.Talent;
using Kontravers.GoodJob.Domain.Talent.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kontravers.GoodJob.Data.Repositories.Talent;

public class PersonQueryRepository(GoodJobDbContext dbContext)
    : QueryRepositoryBase<Person>(dbContext), IPersonQueryRepository
{
    public Task<Person[]> ListAllAsync(CancellationToken cancellationToken, bool includeDisabled = false)
    {
        var query = Query
            .Include(p => p.UpworkRssFeeds)
            .AsNoTracking();
        
        if (!includeDisabled)
        {
            query = query.Where(p => p.IsEnabled == true);
        }
        
        return query.ToArrayAsync(cancellationToken);
    }

    public Task<Person?> GetByUserId(string userId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Task.FromResult((Person?)null);
        }
        
        return Query
            .Include(p => p.UpworkRssFeeds)
            .Include(p => p.Profiles)
            .SingleOrDefaultAsync(p => p.UserId == userId, cancellationToken);
    }

    public override Task<Person?> GetAsync(int id, CancellationToken cancellationToken)
    {
        return Query
            .Include(p => p.UpworkRssFeeds)
            .Include(p => p.Profiles)
            .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}