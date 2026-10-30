using RSSFeedReader.Api.Services;

namespace RSSFeedReader.Api.Tests;

public sealed class InMemorySubscriptionStoreTests
{
    [Fact]
    public void StartsEmpty()
    {
        var store = new InMemorySubscriptionStore();

        Assert.Empty(store.GetAll());
    }

    [Fact]
    public void PreservesOrderDuplicatesAndExactValues()
    {
        var store = new InMemorySubscriptionStore();

        store.Add(" first ");
        store.Add("second");
        store.Add(" first ");

        Assert.Equal(new[] { " first ", "second", " first " }, store.GetAll());
    }

    [Fact]
    public void ReturnsSnapshotRatherThanMutableBackingCollection()
    {
        var store = new InMemorySubscriptionStore();
        store.Add("first");

        var snapshot = store.GetAll();
        store.Add("second");

        Assert.Equal(new[] { "first" }, snapshot);
        Assert.Equal(new[] { "first", "second" }, store.GetAll());
    }
}
