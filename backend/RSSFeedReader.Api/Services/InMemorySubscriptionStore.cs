namespace RSSFeedReader.Api.Services;

public sealed class InMemorySubscriptionStore
{
    private readonly object sync = new();
    private readonly List<string> subscriptions = [];

    public void Add(string url)
    {
        ArgumentException.ThrowIfNullOrEmpty(url);

        lock (sync)
        {
            subscriptions.Add(url);
        }
    }

    public string[] GetAll()
    {
        lock (sync)
        {
            return [.. subscriptions];
        }
    }
}
