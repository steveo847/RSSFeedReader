# Research: RSS Subscription Management

## Decision 1: Keep the documented API and UI responsibilities

- **Decision**: The API owns the subscription collection and exposes add/list
  operations. The browser UI submits values and displays the returned list.
- **Rationale**: This follows the repository's chosen ASP.NET Core Web API +
  Blazor WebAssembly architecture and its stated MVP responsibilities.
- **Alternatives considered**: Keeping all state only in the browser would
  conflict with the documented backend responsibility and API communication
  goal.

## Decision 2: Use a process-lifetime in-memory store

- **Decision**: Keep subscriptions in a singleton in-memory store, clear on API
  process restart, and protect concurrent additions/reads with synchronization.
  Return snapshots rather than exposing the mutable backing collection.
- **Rationale**: A per-request service would lose data between the add and list
  requests. The MVP is explicitly single-user and local, so process-wide state
  is the simplest fit; synchronization makes concurrent HTTP access safe.
- **Alternatives considered**: Persistent storage violates the MVP scope.
  Browser-only state diverges from the documented API-owned collection.

## Decision 3: Define a small add/list HTTP contract

- **Decision**: `GET /api/subscriptions` returns the ordered string list;
  `POST /api/subscriptions` accepts a JSON URL value, appends it, and returns
  the created value. Reject missing/null or empty input; preserve all other
  values exactly, including whitespace-only strings and duplicates.
- **Rationale**: The specification accepts any non-empty value and explicitly
  allows duplicates. Preserving values avoids adding trimming or URL validation
  that the MVP does not require.
- **Alternatives considered**: URL syntax checks, trimming, duplicate
  suppression, and item identifiers add behavior beyond the spec.

## Decision 4: Display submitted values as encoded text

- **Decision**: Render subscription values through normal text output, never
  raw HTML; provide a labeled form, keyboard submission, and accessible status
  feedback.
- **Rationale**: This satisfies the constitution's untrusted-input and
  keyboard-usability principles while keeping the UI simple.
- **Alternatives considered**: Interpreting submitted values as markup is unsafe
  and outside the subscription-list purpose.

## Decision 5: Use .NET 10 and focused automated validation

- **Decision**: Target .NET 10 for new projects; test store rules, API behavior,
  and the browser workflow at their appropriate boundaries.
- **Rationale**: The available SDK is 10.0.403 and .NET 10 is the recommended
  supported LTS target for new development. The repository has no source
  project or target framework yet, so implementation must establish the
  project target.
- **Alternatives considered**: .NET 8 or 9 have nearer support end dates as of
  this plan's date.

## Sources and context

- `StakeholderDocuments/ProjectGoals.md`: MVP scope, single-user local usage,
  in-memory subscriptions, and deferred functionality.
- `StakeholderDocuments/TechStack.md`: API/UI responsibilities, target paths,
  local ports, and configuration expectations.
- `specs/001-manage-rss-subscriptions/spec.md`: user scenarios and acceptance
  requirements.
- `.specify/memory/constitution.md`: security, quality, maintainability, and UI
  requirements.
- [.NET support policy](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core)
- [ASP.NET Core API controller guidance](https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-10.0#automatic-http-400-responses)
- [.NET dependency injection service lifetimes](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection/service-lifetimes)
- [ASP.NET Core integration testing](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-10.0)
- [WAI form labels](https://www.w3.org/WAI/tutorials/forms/labels/)
- [WAI status announcements](https://www.w3.org/WAI/WCAG22/Techniques/aria/ARIA22)
- [ASP.NET Core cross-site scripting prevention](https://learn.microsoft.com/en-us/aspnet/core/security/cross-site-scripting?view=aspnetcore-10.0)

No blocking technical unknowns remain for planning.
