using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace RSSFeedReader.Api.Tests;

public sealed class SubscriptionsApiTests
{
    [Fact]
    public async Task GetInitiallyReturnsEmptyList()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var response = await client.GetFromJsonAsync<string[]>("/api/subscriptions");

        Assert.NotNull(response);
        Assert.Empty(response);
    }

    [Fact]
    public async Task PostAndGetPreserveOrderDuplicatesAndArbitraryValues()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var first = await client.PostAsJsonAsync("/api/subscriptions", new { url = "https://example.com/feed.xml" });
        var second = await client.PostAsJsonAsync("/api/subscriptions", new { url = "<b>not a URL</b>" });
        var duplicate = await client.PostAsJsonAsync("/api/subscriptions", new { url = "https://example.com/feed.xml" });
        var values = await client.GetFromJsonAsync<string[]>("/api/subscriptions");

        Assert.Equal(HttpStatusCode.OK, first.StatusCode);
        Assert.Equal(HttpStatusCode.OK, second.StatusCode);
        Assert.Equal(HttpStatusCode.OK, duplicate.StatusCode);
        Assert.Equal(
            new[] { "https://example.com/feed.xml", "<b>not a URL</b>", "https://example.com/feed.xml" },
            values);
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("{\"url\":null}")]
    [InlineData("{\"url\":\"\"}")]
    public async Task InvalidValuesAreRejectedWithoutChangingList(string requestBody)
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();
        await client.PostAsJsonAsync("/api/subscriptions", new { url = "existing" });

        using var content = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/api/subscriptions", content);
        var values = await client.GetFromJsonAsync<string[]>("/api/subscriptions");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(new[] { "existing" }, values);
    }

    [Fact]
    public async Task WhitespaceOnlyValueIsAcceptedWithoutTrimming()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/subscriptions", new { url = "  " });
        var values = await client.GetFromJsonAsync<string[]>("/api/subscriptions");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(new[] { "  " }, values);
    }
}
