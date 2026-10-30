# Quickstart: Validate RSS Subscription Management

## Prerequisites

- .NET 10 SDK
- A supported browser
- The API and UI projects created at the paths in `plan.md`

## Start the application

1. Confirm the API URL, UI origin, API base URL, and CORS allowed origin agree
   with the launch profiles and configuration.
2. From the repository root, start the API:

   ```powershell
   dotnet run --project backend\RSSFeedReader.Api
   ```

3. In a second terminal, start the UI:

   ```powershell
   dotnet run --project frontend\RSSFeedReader.UI
   ```

4. Open the UI URL printed by the frontend process.

## Validate the user flow

1. Confirm the subscription list starts empty.
2. Use only the keyboard to focus the labeled input, enter a feed URL, and
   submit the form.
3. Confirm the value appears immediately and is announced as status feedback.
4. Submit a second value, then repeat one value; confirm all entries appear in
   submission order and duplicates remain separate.
5. Submit an empty value; confirm no entry is added.
6. Submit a non-empty value that is not a valid feed URL; confirm it is
   accepted without a network request.
7. Where safe to test, enter a string containing markup characters and confirm
   it is displayed as text rather than interpreted as markup.
8. Confirm no feed items or feed content appear and no feed request is made.
9. Stop and restart the API process; confirm the list is empty.

## Automated validation

From the repository root, run the focused test projects and builds:

```powershell
dotnet test tests\RSSFeedReader.Api.Tests
dotnet test tests\RSSFeedReader.UI.Tests
dotnet build backend\RSSFeedReader.Api
dotnet build frontend\RSSFeedReader.UI
```

Expected results: all tests and builds pass; API tests prove empty-list,
insertion-order, duplicate, exact-value, and empty-input behavior; the UI
workflow proves keyboard submission and safe visible text rendering.

See [the API contract](contracts/subscriptions-api.md) and
[the data model](data-model.md) for precise behavior.
