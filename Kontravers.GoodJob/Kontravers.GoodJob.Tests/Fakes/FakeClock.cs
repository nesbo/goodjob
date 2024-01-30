using Kontravers.GoodJob.Domain;

namespace Kontravers.GoodJob.Tests.Fakes;

public class FakeClock : IClock
{
    private readonly DateTime _utcNow;

    public FakeClock(DateTime utcNow)
    {
        _utcNow = utcNow;
    }

    public FakeClock()
    {
        _utcNow = DateTime.UtcNow;
    }

    public DateTime UtcNow => _utcNow;
}