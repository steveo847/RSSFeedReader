# Tasks: RSS Subscription Management

**Input**: Design documents from
`/specs/001-manage-rss-subscriptions/`

**Prerequisites**: plan.md, spec.md, research.md, data-model.md,
contracts/subscriptions-api.md, quickstart.md

**Tests**: Automated tests are included because the project constitution requires
tests for changed behavior.

**Organization**: Tasks are grouped by user story. This feature has one P1
user story; shared setup and prerequisites are separate.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Tasks touch different files and can proceed independently once
  their phase prerequisites are complete.
- **[Story]**: Story label for user-story tasks; setup and foundational tasks
  have no story label.
- Every task names the file or project path to change.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Create the .NET solution and independent project skeletons.

- [ ] T001 Create the .NET 10 solution and SDK pin in `RSSFeedReader.sln` and `global.json`.
- [ ] T002 [P] Create the ASP.NET Core Web API project targeting .NET 10 in `backend\RSSFeedReader.Api\RSSFeedReader.Api.csproj`.
- [ ] T003 [P] Create the Blazor WebAssembly project targeting .NET 10 in `frontend\RSSFeedReader.UI\RSSFeedReader.UI.csproj`.
- [ ] T004 [P] Create the API xUnit test project in `tests\RSSFeedReader.Api.Tests\RSSFeedReader.Api.Tests.csproj`.
- [ ] T005 [P] Create the Blazor component test project with bUnit in `tests\RSSFeedReader.UI.Tests\RSSFeedReader.UI.Tests.csproj`.
- [ ] T006 Add the API, UI, and both test projects to `RSSFeedReader.sln`; reference `backend\RSSFeedReader.Api\RSSFeedReader.Api.csproj` from `tests\RSSFeedReader.Api.Tests\RSSFeedReader.Api.Tests.csproj` and `frontend\RSSFeedReader.UI\RSSFeedReader.UI.csproj` from `tests\RSSFeedReader.UI.Tests\RSSFeedReader.UI.Tests.csproj`.

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Align local API/UI integration and clear template routes before
implementing the subscription workflow.

**⚠️ CRITICAL**: Complete this phase before User Story 1.

- [ ] T007 Configure the backend development port, explicit frontend CORS origins, and API routes in `backend\RSSFeedReader.Api\Properties\launchSettings.json` and `backend\RSSFeedReader.Api\Program.cs`.
- [ ] T008 Configure the frontend API base URL for the backend development port in `frontend\RSSFeedReader.UI\wwwroot\appsettings.json` and `frontend\RSSFeedReader.UI\Program.cs`.
- [ ] T009 Remove conflicting template demo routes and navigation links, leaving the intended subscription landing route in `frontend\RSSFeedReader.UI\Pages\` and `frontend\RSSFeedReader.UI\Layout\NavMenu.razor`.

**Checkpoint**: Solution projects build; local API/UI URLs and CORS agree; the
landing route is unambiguous.

---

## Phase 3: User Story 1 - Add and View Subscriptions (Priority: P1) 🎯 MVP

**Goal**: Let the single local user add any non-empty value and immediately
view the ordered list, without fetching feeds or persisting values.

**Independent Test**: Start with an empty list, add multiple values including a
duplicate and a non-URL string, and confirm the exact values appear in order.
Confirm empty values are not added, submitted markup is displayed as text, no
feed request occurs, and restarting the API clears the list.

### Tests for User Story 1

- [ ] T010 [P] [US1] Add store unit tests for initial empty state, insertion order, duplicate acceptance, exact-value preservation, empty-value rejection, and snapshot reads in `tests\RSSFeedReader.Api.Tests\InMemorySubscriptionStoreTests.cs`.
- [ ] T011 [P] [US1] Add API integration tests for empty GET, successful POST/GET, duplicate values, arbitrary non-URL values, missing/null/empty rejection, unchanged state after invalid input, and fresh-host reset in `tests\RSSFeedReader.Api.Tests\SubscriptionsApiTests.cs`.
- [ ] T012 [P] [US1] Add component tests for labeled keyboard submission, immediate list updates, empty input, duplicate values, and encoded text rendering in `tests\RSSFeedReader.UI.Tests\SubscriptionsPageTests.cs`.

### Implementation for User Story 1

- [ ] T013 [P] [US1] Define the subscription request and response shape in `backend\RSSFeedReader.Api\Models\SubscriptionRequest.cs`; reject `Url` only when null or length zero, preserve whitespace-only values exactly, and do not trim or validate URL syntax.
- [ ] T014 [US1] Implement the process-lifetime ordered subscription store with synchronized add and snapshot reads in `backend\RSSFeedReader.Api\Services\InMemorySubscriptionStore.cs`.
- [ ] T015 [US1] Register the singleton store and implement `GET /api/subscriptions` and `POST /api/subscriptions` per `specs\001-manage-rss-subscriptions\contracts\subscriptions-api.md` in `backend\RSSFeedReader.Api\Program.cs`.
- [ ] T016 [US1] Implement the typed API client for listing and adding subscriptions in `frontend\RSSFeedReader.UI\Services\SubscriptionApiClient.cs`.
- [ ] T017 [US1] Implement the subscription landing page with labeled input, keyboard-operable submit, immediate ordered list updates, accessible status feedback, and text-only rendering in `frontend\RSSFeedReader.UI\Pages\Subscriptions.razor`.
- [ ] T018 [US1] Run `dotnet test` and `dotnet build` for `backend\RSSFeedReader.Api\RSSFeedReader.Api.csproj`, `frontend\RSSFeedReader.UI\RSSFeedReader.UI.csproj`, `tests\RSSFeedReader.Api.Tests\RSSFeedReader.Api.Tests.csproj`, and `tests\RSSFeedReader.UI.Tests\RSSFeedReader.UI.Tests.csproj`; complete every scenario in `specs\001-manage-rss-subscriptions\quickstart.md` and fix failures in the corresponding source or test files.

**Checkpoint**: User Story 1 is independently testable and fulfills the MVP
scope.

---

## Phase 4: Polish & Cross-Cutting Concerns

**Purpose**: Verify the integrated MVP and keep setup documentation aligned.

- [ ] T019 [P] Update local run instructions, ports, API base URL, CORS origins, and verification commands in `StakeholderDocuments\TechStack.md`.
- [ ] T020 Review `backend\RSSFeedReader.Api\` and `frontend\RSSFeedReader.UI\` against the scope and security requirements in `.specify\memory\constitution.md`; remove any feed fetching, URL validation, persistence, or raw-markup rendering not required by the MVP.

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No prerequisite; T001 first, then T002-T005 may run in
  parallel; T006 follows project creation.
- **Foundational (Phase 2)**: Depends on Phase 1; T007-T009 complete before
  story implementation.
- **User Story 1 (Phase 3)**: Depends on Phase 2. T010-T012 can be developed in
  parallel as test files; implementation follows the relevant tests.
- **Polish (Phase 4)**: Depends on a complete and passing User Story 1.

### User Story Dependencies

- **User Story 1 (P1)**: Starts after the foundational phase; no other user
  stories exist or are required.

### Within User Story 1

- Write tests before implementation and verify that they fail for the expected
  missing behavior.
- T013 defines the request contract before the store and endpoint consume it.
- T014 implements the store before T015 exposes it through HTTP.
- T015 provides the API consumed by T016; T016 provides the UI service consumed
  by T017.
- T018 validates the complete user journey after implementation.

### Parallel Opportunities

- **Setup**: T002, T003, T004, and T005 create separate projects after T001.
- **User Story 1 tests**: T010, T011, and T012 are separate test files and can
  be authored in parallel after the test projects exist.
- **User Story 1 implementation**: T013 can proceed alongside test authoring;
  T016 can be prepared independently of the backend store/API implementation
  after the client contract is agreed. T014 and T015 remain sequential.
- **Polish**: T019 and T020 touch separate files and can run in parallel after
  T018.

## Parallel Example: User Story 1

```text
After T006-T009, author these separate tests concurrently:
Task: T010 store unit tests in tests\RSSFeedReader.Api.Tests\InMemorySubscriptionStoreTests.cs
Task: T011 API integration tests in tests\RSSFeedReader.Api.Tests\SubscriptionsApiTests.cs
Task: T012 component tests in tests\RSSFeedReader.UI.Tests\SubscriptionsPageTests.cs

After the request contract is settled, the API and UI client work can proceed
in parallel on their separate project files; complete the API endpoint before
running end-to-end UI validation.
```

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1 setup.
2. Complete Phase 2 foundational configuration and route cleanup.
3. Implement and test User Story 1.
4. Run the independent API/UI tests and full quickstart acceptance scenarios.
5. Stop at the MVP checkpoint; do not add feed fetching, persistence, removal,
   or background polling.

### Incremental Delivery

The specification contains only User Story 1. Deliver that complete P1 story as
the MVP, then treat any later capabilities as separately specified work.

## Notes

- [P] means tasks have no shared-file edits or incomplete-task dependency.
- Keep each story task limited to its listed files and stated requirements.
- Do not add a feed HTTP client or feed parsing package for this MVP.
