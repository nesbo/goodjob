using Kontravers.GoodJob.Data.Repositories.Talent;
using Kontravers.GoodJob.Domain.Talent;
using Kontravers.GoodJob.Domain.Talent.Repositories;

namespace Kontravers.GoodJob.Tests.Builders;

public class TestDataBuilder
{
    private readonly TestDbContextFactory _dbContextFactory;
    private const string DefaultPersonEmail = "person@goodjob.com";

    public TestDataBuilder(string? databaseName = null)
    {
        _dbContextFactory = new TestDbContextFactory(databaseName);
    }
    
    public IPersonQueryRepository PersonQueryRepository => new PersonQueryRepository(_dbContextFactory.Context);

    public TestDataBuilder BuildDefaultPerson(DateTime? createdUtc = null, string? email = null)
    {
        createdUtc ??= DateTime.UtcNow;
        email ??= DefaultPersonEmail;
        var person = new Person(true, email, "John Doe", 1,
             createdUtc.Value, createdUtc.Value);
        
        person.AddUpworkRssFeed("https://www.upwork.com/", "ab/feed/jobs/rss?sort=recency&paging=0%3B1",
            DateTime.MinValue, 5, createdUtc.Value, createdUtc.Value,
            true,true);

        _dbContextFactory.Context.Add(person);
        _dbContextFactory.Context.SaveChanges();
        
        return this;
    }
}