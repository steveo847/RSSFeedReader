# Data Model: RSS Subscription Management

## Subscription

Represents one value submitted by the user and shown in the current
subscription list.

| Field | Type | Rules |
|---|---|---|
| URL | String | Required; must not be null or empty. No URL syntax, reachability, or feed validation. Preserve the submitted value exactly. |

### Collection behavior

- Subscriptions are held in insertion order for the lifetime of the API process.
- Duplicate values are separate entries.
- A list request returns a snapshot, not a mutable reference to the store.
- The list is cleared when the API process restarts.
- No feed items or other feed data are part of this feature.

## State changes

1. Initial process state is an empty collection.
2. A valid add appends one subscription.
3. A list operation returns the current ordered collection without changing it.
4. An empty or missing value is rejected without changing the collection.
5. Process restart returns the collection to its initial empty state.
