<!--
Sync Impact Report
Version change: 1.0.0 -> 1.0.1
Modified principles: III. Verified Quality (made security-related test expectations explicit).
Added sections: None.
Removed sections: None.
Follow-up TODOs: Confirm the original ratification date.
-->
# RSS Feed Reader Constitution

## Core Principles

### I. Secure by Default
Treat subscription URLs and any future feed data as untrusted input. The MVP stores and displays
the user-provided URL but MUST NOT make network requests or render feed content. When feed fetching
is authorized, requests MUST be limited to HTTP and HTTPS, use bounded timeouts, and prevent access
to loopback, private, and link-local network destinations, including after redirects. User- and
feed-provided text MUST be encoded for its display context. This keeps the minimal MVP safe without
silently introducing network or content-rendering risks.

### II. Maintainable by Design
Keep responsibilities separated between the ASP.NET Core Web API and Blazor WebAssembly UI, and
follow the existing .NET and C# conventions. Keep changes focused, use configuration rather than
hard-coded environment endpoints, and add dependencies or architectural layers only when a
requirement needs them. Preserve coordinated API URL, port, and CORS configuration. These practices
support incremental development without burdening the proof of concept with speculative complexity.

### III. Verified Quality
Every new or changed behavior MUST have automated tests at the appropriate unit or integration
level. Tests MUST check the stated behavior, including the observable result of adding and listing a
subscription. When a feature adds network or feed-content handling, tests MUST cover the required
URL restrictions, bounded requests, output encoding, and specified failure behavior. Run the
relevant tests and build before declaring a change complete, and fix failures caused by the change.
Review changes for correctness, clarity, and regressions; a passing build alone does not establish
feature correctness.

### IV. Scope Discipline
The MVP is limited to adding a subscription URL and displaying the subscription list, with
in-memory storage. Do not add feed fetching, parsing, persistence, removal, or background polling
unless a later feature specification explicitly authorizes that scope. Implement the smallest
solution that satisfies the accepted requirement, and identify deferred work rather than expanding
the MVP implicitly.

### V. Clear, Usable UI
The subscription interface MUST identify the URL input, make the add action operable by keyboard,
and visibly reflect a successfully added subscription. Keep the presentation simple and functional;
do not spend MVP effort on visual polish that does not improve the required workflow. This ensures
the proof of concept demonstrates its purpose clearly.

## Project Constraints

- The application is a single-user, locally run proof of concept developed for Windows, macOS, and
  Linux.
- Use an ASP.NET Core Web API backend and Blazor WebAssembly frontend. The backend manages
  subscriptions; the frontend provides the subscription input and list.
- Keep MVP subscriptions in memory. Do not add network feed operations or persistence in the MVP.
- Configure the frontend API base URL and backend CORS origins to match the actual local
  development ports. Do not use a permissive wildcard CORS policy where the intended frontend
  origin is known.
- Future capabilities, including feed parsing, persistence, and background operations, require
  their own explicit feature scope and appropriate security and quality controls.

## Development Workflow & Quality Gates

- Before implementation, confirm the change fits the MVP or an explicitly approved later feature.
- For UI work, check that only the intended page owns each route and remove conflicting template
  demo routes before implementing the MVP page.
- For API/UI integration, verify the backend port, frontend API base URL, CORS origin, and browser
  connectivity together.
- Add or update automated tests for changed behavior, then run the focused tests and relevant
  project builds. Report any validation that could not be run.
- Review the change against this constitution, the approved feature scope, and the security
  principle before completion.

## Governance

This constitution governs project work and takes precedence over conflicting implementation
guidance. Amendments MUST be reviewed and documented with their rationale, affected principles,
and any required migration or follow-up work. Project changes MUST satisfy the principles and
quality gates above; any exception MUST be explicit, narrowly scoped, and justified in the change
review.

Use semantic versioning for this constitution: MAJOR for backward-incompatible governance changes
or principle removals/redefinitions, MINOR for adding a principle or materially expanding
governance, and PATCH for clarifications and non-semantic wording changes. Review compliance during
feature planning and change review, and verify that tests and builds appropriate to the change have
passed before acceptance.

**Version**: 1.0.1 | **Ratified**: TODO(RATIFICATION_DATE): Original adoption date is unknown. | **Last Amended**: 2026-10-29
