using Kontravers.GoodJob.Data.Repositories.Talent;
using Kontravers.GoodJob.Domain.Talent.Repositories;

namespace Kontravers.GoodJob.Tests.Builders;

public class TestDataBuilder
{
    private readonly TestDbContextFactory _dbContextFactory;

    public TestDataBuilder(string? databaseName = null)
    {
        _dbContextFactory = new TestDbContextFactory(databaseName);
    }
    
    public IPersonQueryRepository PersonQueryRepository => new PersonQueryRepository(_dbContextFactory.Context);
}