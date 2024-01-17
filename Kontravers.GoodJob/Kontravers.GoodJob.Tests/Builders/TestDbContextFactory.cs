using Kontravers.GoodJob.Data;
using Microsoft.EntityFrameworkCore;

namespace Kontravers.GoodJob.Tests.Builders;

public class TestDbContextFactory
{
    private readonly string _inMemoryDatabaseName;
    private GoodJobDbContext? _context;

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
        var goodJobDbContext = new GoodJobDbContext(options);
        goodJobDbContext.Database.EnsureCreated();
        _context = goodJobDbContext;

        return goodJobDbContext;
    }

    public GoodJobDbContext Context => _context ?? CreateContext(_inMemoryDatabaseName);
}