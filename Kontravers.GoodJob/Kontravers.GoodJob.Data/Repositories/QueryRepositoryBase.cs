using Kontravers.GoodJob.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kontravers.GoodJob.Data.Repositories;

public abstract class QueryRepositoryBase<TAggregate> where TAggregate : class, IAggregate
{
    protected IQueryable<TAggregate> Query => DbContext.Set<TAggregate>()
        .AsNoTracking();

    private GoodJobDbContext DbContext { get; }

    protected QueryRepositoryBase(GoodJobDbContext dbContext)
    {
        DbContext = dbContext;
    }
    
    public virtual Task<TAggregate?> GetAsync(int id, CancellationToken cancellationToken)
    {
        return Query
            .SingleOrDefaultAsync(a => a.Id == id, cancellationToken);
    }
}