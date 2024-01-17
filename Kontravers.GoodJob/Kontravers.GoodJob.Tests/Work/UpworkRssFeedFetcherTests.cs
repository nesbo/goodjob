using Kontravers.GoodJob.Domain.Work;
using Kontravers.GoodJob.Tests.Builders;
using Microsoft.Extensions.Logging.Abstractions;

namespace Kontravers.GoodJob.Tests.Work;

public class UpworkRssFeedFetcherTests
{
    private TestDataBuilder _testDataBuilder;

    [SetUp]
    public void Setup()
    {
        _testDataBuilder = new TestDataBuilder();
    }
    
    [Test]
    public async Task GivenUpworkRssFeedFetcher_WhenFetch_ThenReturnListOfUpworkRssFeed()
    {
        // Arrange
        var upworkRssFeedFetcher = new UpworkRssFeedFetcher(_testDataBuilder.PersonQueryRepository,
            new NullLogger<UpworkRssFeedFetcher>(),
            new FakeClock());
        
        // Act
        await upworkRssFeedFetcher.StartFetchingAllAsync(CancellationToken.None);
        
        // Assert
        
    }
}