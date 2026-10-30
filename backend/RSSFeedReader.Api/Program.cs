var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("LocalFrontend", policy =>
        policy.WithOrigins("http://localhost:5213")
            .AllowAnyHeader()
            .AllowAnyMethod());
});
builder.Services.AddSingleton<RSSFeedReader.Api.Services.InMemorySubscriptionStore>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("LocalFrontend");

app.MapGet("/api/subscriptions", (RSSFeedReader.Api.Services.InMemorySubscriptionStore store) =>
    Results.Ok(store.GetAll()));

app.MapPost("/api/subscriptions", AddSubscription);

app.Run();

static IResult AddSubscription(
    RSSFeedReader.Api.Models.SubscriptionRequest? request,
    RSSFeedReader.Api.Services.InMemorySubscriptionStore store)
{
    if (request?.Url is null || request.Url.Length == 0)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["url"] = ["A non-empty URL is required."]
        });
    }

    store.Add(request.Url);
    return Results.Ok(new RSSFeedReader.Api.Models.SubscriptionResponse(request.Url));
}

public partial class Program
{
}
