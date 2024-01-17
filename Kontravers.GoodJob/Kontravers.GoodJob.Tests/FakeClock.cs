using Kontravers.GoodJob.Domain;

namespace Kontravers.GoodJob.Tests;

public class FakeClock : IClock
{
    public DateTime UtcNow { get; } = DateTime.UtcNow;
}