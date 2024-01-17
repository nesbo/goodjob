namespace Kontravers.GoodJob.Domain;

public class Clock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}