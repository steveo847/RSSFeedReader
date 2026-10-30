# Subscriptions API Contract

Base path: `/api/subscriptions`

The API is for the local single-user MVP. It does not retrieve or parse feed
content. Submitted values are data, not instructions or markup.

## `GET /api/subscriptions`

Returns all subscriptions in insertion order.

**Success: `200 OK`**

```json
[
  "https://example.com/feed.xml",
  "https://example.org/atom.xml"
]
```

An empty collection is represented as `[]`.

## `POST /api/subscriptions`

Appends one subscription. Duplicate values are accepted. Values are preserved
exactly; no URL validation or trimming is performed.

**Request: `application/json`**

```json
{
  "url": "https://example.com/feed.xml"
}
```

**Success: `200 OK`**

```json
{
  "url": "https://example.com/feed.xml"
}
```

**Invalid request: `400 Bad Request`**

A missing/null URL or an empty string is rejected. Whitespace-only strings are
non-empty and are accepted. An invalid request does not change the collection.

The response body uses the standard ASP.NET Core validation problem format
when model validation supplies details.
