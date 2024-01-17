using Kontravers.GoodJob.Data;
using Microsoft.EntityFrameworkCore;

namespace Kontravers.GoodJob.Tests.Builders;

public class TestDbContextFactory
{
    private readonly string _inMemoryDatabaseName;

    public TestDbContextFactory(string? inMemoryDatabaseName = null)
    {
        _inMemoryDatabaseName = inMemoryDatabaseName ?? Guid.NewGuid().ToString();
    }

    private GoodJobDbContext CreateContext(
        string inMemoryDatabaseName)
    {
        var dbOptionsBuilder = new DbContextOptionsBuilder();

        dbOptionsBuilder.UseInMemoryDatabase(inMemoryDatabaseName);
        

        var options = dbOptionsBuilder.Options;
        var connectDbContext = new GoodJobDbContext(options);
        connectDbContext.Database.EnsureCreated();

        return connectDbContext;
    }

    public GoodJobDbContext Context => CreateContext(_inMemoryDatabaseName);
}