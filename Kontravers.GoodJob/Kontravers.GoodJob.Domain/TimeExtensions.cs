namespace Kontravers.GoodJob.Domain;

public static class TimeExtensions
{
    public static string ToShortTimespanString(this TimeSpan span)
    {
        if (span.Days > 1)
        {
            return $"{Convert.ToInt32(span.TotalDays)}D";
        }
        
        if (span.Days == 1)
        {
            return $"{Convert.ToInt32(span.TotalHours)}H";
        }
        
        if (span.Hours > 1)
        {
            return $"{Convert.ToInt32(span.TotalHours)}H";
        }
        
        if (span.Hours == 1)
        {
            return $"{Convert.ToInt32(span.TotalMinutes)}M";
        }
        
        return span.Minutes >= 1 
            ? $"{Convert.ToInt32(span.TotalMinutes)}M" 
            : $"{Convert.ToInt32(span.TotalSeconds)}S";
    }
}