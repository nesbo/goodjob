namespace Kontravers.GoodJob.Domain;

public static class TimeExtensions
{
    public static string ToShortTimespanString(this TimeSpan span)
    {
        if (span.Days > 1)
        {
            return $"{span.TotalDays}D";
        }
        
        if (span.Days == 1)
        {
            return $"{span.TotalHours}H";
        }
        
        if (span.Hours > 1)
        {
            return $"{span.TotalHours}H";
        }
        
        if (span.Hours == 1)
        {
            return $"{span.TotalMinutes}M";
        }
        
        return span.Minutes >= 1 
            ? $"{span.TotalMinutes}M" 
            : $"{span.TotalSeconds}S";
    }
}