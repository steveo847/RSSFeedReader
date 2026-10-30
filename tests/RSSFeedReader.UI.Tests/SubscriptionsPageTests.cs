using System.Net;
using System.Text;
using System.Text.Json;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using RSSFeedReader.UI.Pages;
using RSSFeedReader.UI.Services;

namespace RSSFeedReader.UI.Tests;

public sealed class SubscriptionsPageTests
{
    [Fact]
    public void AddsByFormSubmissionAndRendersValuesAsText()
    {
        using var context = new TestContext();
        var handler = new SubscriptionHandler();
        context.Services.AddScoped(_ => new SubscriptionApiClient(
            new HttpClient(handler) { BaseAddress = new Uri("http://localhost/api/") }));

        var page = context.RenderComponent<Subscriptions>();
        var input = page.Find("#subscription-url");

        Assert.Equal("Subscription URL", page.Find("label[for='subscription-url']").TextContent);
        input.Input("<script>alert(1)</script>");
        page.Find("form").Submit();

        Assert.Equal("<script>alert(1)</script>", page.Find("ul li").TextContent);
        Assert.Equal("Subscription added.", page.Find("[role='status']").TextContent);
        Assert.Equal(1, handler.PostCount);
        Assert.Contains("&lt;script&gt;", page.Markup);
        Assert.DoesNotContain("<script>", page.Markup);
    }

    [Fact]
    public void KeepsDuplicateEntriesAndDoesNotSubmitEmptyInput()
    {
        using var context = new TestContext();
        var handler = new SubscriptionHandler();
        context.Services.AddScoped(_ => new SubscriptionApiClient(
            new HttpClient(handler) { BaseAddress = new Uri("http://localhost/api/") }));

        var page = context.RenderComponent<Subscriptions>();
        var input = page.Find("#subscription-url");

        input.Input("duplicate");
        page.Find("form").Submit();
        input.Input("duplicate");
        page.Find("form").Submit();
        input.Input(string.Empty);
        page.Find("form").Submit();

        Assert.Equal(new[] { "duplicate", "duplicate" }, page.FindAll("ul li").Select(item => item.TextContent));
        Assert.Equal(2, handler.PostCount);
    }

    [Fact]
    public void AcceptsKeyboardOperableFormMarkup()
    {
        using var context = new TestContext();
        context.Services.AddScoped(_ => new SubscriptionApiClient(
            new HttpClient(new SubscriptionHandler()) { BaseAddress = new Uri("http://localhost/api/") }));

        var page = context.RenderComponent<Subscriptions>();

        Assert.NotNull(page.Find("form button[type='submit']"));
        Assert.NotNull(page.Find("input#subscription-url"));
    }

    private sealed class SubscriptionHandler : HttpMessageHandler
    {
        private readonly List<string> subscriptions = [];

        public int PostCount { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Get)
            {
                return JsonResponse(subscriptions);
            }

            if (request.Method == HttpMethod.Post)
            {
                PostCount++;
                var body = await request.Content!.ReadAsStringAsync(cancellationToken);
                var url = JsonDocument.Parse(body).RootElement.GetProperty("url").GetString()!;
                subscriptions.Add(url);
                return JsonResponse(new { url });
            }

            return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
        }

        private static HttpResponseMessage JsonResponse<T>(T value) => new(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json")
        };
    }
}
