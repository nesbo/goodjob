using Kontravers.GoodJob.Domain.Work;
using Kontravers.GoodJob.Domain.Work.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kontravers.GoodJob.Data.Repositories.Work;

public class JobRepository : RepositoryBase<Job>, IJobRepository
{
    public JobRepository(GoodJobDbContext dbContext) : base(dbContext)
    {
    }

    public Task<Job?> GetAsync(int id, CancellationToken cancellationToken)
    {
        return Query.SingleOrDefaultAsync(job => job.Id == id, cancellationToken);
    }

    public Task<Job?> GetAsync(int personId, string uuid, CancellationToken cancellationToken)
    {
        return Query
            .SingleOrDefaultAsync(job => job.PersonId == personId && job.Uuid == uuid
                , cancellationToken);
    }

    public async Task<bool> ExistsAsync(int personId, string commandUuid, CancellationToken cancellationToken)
    {
        return await Query
            .CountAsync(j=> j.PersonId == personId && j.Uuid == commandUuid,
                cancellationToken) > 0;
    }
}