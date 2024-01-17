namespace Kontravers.GoodJob.Domain;

public interface IHttpClient
{
    Task<string> GetStringAsync(string baseUrl, string relativeUrl, CancellationToken cancellationToken);
}