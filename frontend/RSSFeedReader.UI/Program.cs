using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RSSFeedReader.UI;
using RSSFeedReader.UI.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["ApiBaseUrl"];
if (!Uri.TryCreate(apiBaseUrl, UriKind.Absolute, out var apiUri))
{
    throw new InvalidOperationException("ApiBaseUrl must be configured as an absolute URL.");
}

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = apiUri });
builder.Services.AddScoped<SubscriptionApiClient>();

await builder.Build().RunAsync();
