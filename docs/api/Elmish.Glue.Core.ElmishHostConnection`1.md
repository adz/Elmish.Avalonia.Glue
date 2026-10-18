# Elmish.Glue.Core.ElmishHostConnection`1

## `ElmishHostConnection<'Msg>`

A running Elmish program with `IDisposable` and a stable dispatch surface.

### Members

| Member | Description |
| :--- | :--- |
| `Dispatch(message)` | Dispatches a message to the running Elmish program. |
| `Dispose()` | Stops the Elmish program and cleans up any subscriptions. |


## Disposal contract

After disposal, `Dispatch` silently ignores new user messages; already queued messages may still drain before the queued termination message. Disposal can post asynchronously from a worker thread; keep the UI dispatcher running until shutdown completes. See <a href="/api/Elmish.Glue.Core.ElmishHostConnection`1.html">host lifetime</a>.
