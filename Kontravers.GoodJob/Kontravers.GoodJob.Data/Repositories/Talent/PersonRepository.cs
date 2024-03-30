using Kontravers.GoodJob.Domain.Talent;
using Kontravers.GoodJob.Domain.Talent.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kontravers.GoodJob.Data.Repositories.Talent;

public class PersonRepository : RepositoryBase<Person>, IPersonRepository
{
    public PersonRepository(GoodJobDbContext goodJobDbContext) : base(goodJobDbContext) { }
    
    public Task<Person?> GetAsync(int personId, CancellationToken cancellationToken)
    {
        return Query
            .Include(p => p.UpworkRssFeeds)
            .Include(p => p.Profiles)
            .SingleOrDefaultAsync(p => p.Id == personId, cancellationToken);
    }

    public Task<bool> ExistsForOrganisationAsync(string email, int organizationId, CancellationToken cancellationToken)
    {
        return Query
            .AnyAsync(p => p.Email == email && p.OrganisationId == organizationId,
                cancellationToken);
    }
}