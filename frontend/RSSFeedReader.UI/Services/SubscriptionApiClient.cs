using System.Net.Http.Json;

namespace RSSFeedReader.UI.Services;

public sealed class SubscriptionApiClient(HttpClient httpClient)
{
    public async Task<string[]> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var values = await httpClient.GetFromJsonAsync<string[]>(
            "subscriptions",
            cancellationToken);

        return values ?? throw new InvalidDataException("The subscriptions API returned no list.");
    }

    public async Task<string> AddAsync(string url, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync(
            "subscriptions",
            new SubscriptionRequest(url),
            cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<SubscriptionResponse>(
            cancellationToken: cancellationToken);

        return result?.Url ?? throw new InvalidDataException("The subscriptions API returned no subscription.");
    }

    private sealed record SubscriptionRequest(string Url);

    private sealed record SubscriptionResponse(string Url);
}
