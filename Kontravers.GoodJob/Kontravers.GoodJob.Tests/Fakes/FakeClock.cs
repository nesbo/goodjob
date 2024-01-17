using Kontravers.GoodJob.Domain;

namespace Kontravers.GoodJob.Tests.Fakes;

public class FakeClock : IClock
{
    public DateTime UtcNow { get; } = DateTime.UtcNow;
}