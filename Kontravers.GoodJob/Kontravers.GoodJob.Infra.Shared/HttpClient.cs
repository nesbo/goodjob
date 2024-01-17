using Kontravers.GoodJob.Domain;
using RestSharp;

namespace Kontravers.GoodJob.Infra.Shared;

public class HttpClient : IHttpClient
{
    public async Task<string> GetStringAsync(string baseUrl, string relativeUrl, CancellationToken cancellationToken)
    {
        using var client = new RestClient(baseUrl);
        var request = new RestRequest(relativeUrl);
        
        var response = await client.ExecuteAsync(request, cancellationToken);
        
        if (!response.IsSuccessful)
        {
            throw new Exception($"Error fetching {baseUrl}{relativeUrl}: {response.ErrorMessage}");
        }
        
        return response.Content!;
    }
}