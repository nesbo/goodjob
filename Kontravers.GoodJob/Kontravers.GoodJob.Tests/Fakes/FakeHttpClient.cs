using Kontravers.GoodJob.Domain;

namespace Kontravers.GoodJob.Tests.Talent;

public class FakeHttpClient : IHttpClient
{
    private readonly string _response;

    public FakeHttpClient(string response)
    {
        _response = response;
    }

    public Task<string> GetStringAsync(string baseUrl, string relativeUrl, CancellationToken cancellationToken)
    {
        return Task.FromResult(_response);
    }
}