using Kontravers.GoodJob.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kontravers.GoodJob.Data.Repositories;

public abstract class RepositoryBase<TAggregate> where TAggregate : class, IAggregate
{
    protected IQueryable<TAggregate> Query => DbContext.Set<TAggregate>();
    protected DbSet<TAggregate> AggregateSet => DbContext.Set<TAggregate>();
    private GoodJobDbContext DbContext { get; }
    
    protected RepositoryBase(GoodJobDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public Task AddAsync(TAggregate aggregate, CancellationToken cancellationToken)
    {
        AggregateSet.Add(aggregate);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return DbContext.SaveChangesAsync(cancellationToken);
    }
}