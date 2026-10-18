# Error handling

One short recipe for each failure this library can surface. Behaviours below
are verified against the source and the test suite in `tests/`; links point to
the exact contract.

## Duplicate key in `SyncWith`

`SyncWith` validates keys in two passes, and the failure point differs.

- **Duplicate key in the incoming snapshot** throws `ArgumentException`
  (parameter `targets`) *before* any collection change. The message names the
  key, for example `Duplicate key '1'`. The collection is left exactly as it
  was.
- **Duplicate key already in the target collection** throws
  `InvalidOperationException` *after* obsolete items may have been removed.
  The patch removes missing keys as it walks the current collection, so a
  duplicate can be detected part-way through. Treat the collection as
  partially patched after this exception.

Keys must be unique before you call `SyncWith`. Validate the incoming snapshot
in `update` (for example with `List.distinctBy fst` plus a length check) when a
key source can repeat. See the [keyed collections guide](keyed-collections.html),
the runnable [patching example](../examples/keyed-collection-sync.html), and the
[collections reference](../reference/core/collections.html).

## Missing or duplicate write-back route

- **Missing route** is silent: `TryDispatchWriteBack` (and the underlying
  `WriteBackBindings.TryDispatch`) returns `false` and dispatches nothing. The
  snapshot is not mutated. A `TwoWay` write against an unregistered path
  appears to "do nothing".
- **Duplicate route** throws `InvalidOperationException` at registration time
  (in the host constructor), with
  `A write-back binding for '...' is already registered.` The path names the
  duplicate.
- **Invalid selector** throws `ArgumentException` during `For(...)` when the
  expression is not a simple property-access path, and `ArgumentNullException`
  for a null selector.

Register each writable path exactly once, and use the exact path the facade
exposes (for example `"UserInput.Name"` for `ForPath`, or
`x => x.UserInput.Name` for `For`). See the [write-back reference](../reference/elmview/write-back.html)
and the [route recipe](route-editable-elmview-properties.html).

## Binding errors

Binding errors are Avalonia's, not this library's. They surface as debug-output
binding diagnostics or AXAML compiler errors, not as host exceptions.

- A compiled binding (`x:CompileBindings="True"`) whose path does not exist on
  the `x:DataType` type fails the AXAML compile.
- A `TwoWay` binding to a getter-only property has no setter to call, so no
  message is dispatched and the control may revert to the last published value.
- A property that never raises `PropertyChanged` leaves the control stale after
  the snapshot changes.

Fix the binding path, make the property writable, and confirm the property is
notified. The [troubleshooting table](troubleshooting.html) maps the common
symptoms to their fixes.

## Dispatcher not set or disposed

- **Dispatcher not set** is a no-op. `GeneratedViewHost.Dispatch`,
  `FSharpProjectionBase.Dispatch`, and `Dispatcher.Send` start as an `ignore`
  function. A `TwoWay` write before `SetDispatch` (for example in a design-time
  host) dispatches nothing and mutates nothing. This is intentional for
  preview; see [design preview](design-time-preview.html).
- **Connection disposed** drops messages. `ElmishHostConnection.Dispatch`
  checks the disposed flag and ignores any message after `Dispose()`. Disposal
  also sends the internal stop message so Elmish disposes subscriptions.

Keep the connection stored on the application or window and dispose it with
the owning view. If work dispatches after teardown, stop the producer first or
guard the dispatch. See the [lifetime reference](../reference/core/host-lifetime.html).

## Exceptions in update, subscription, or post

- **Update exceptions propagate** to the draining caller. With the core
  `ElmishHost.start`, that is the dispatching thread; with the Avalonia
  `ElmishHost.start`, that is the UI thread. The drain's `finally` releases the
  gate, so queued messages remain and the next dispatch resumes them. Wrap the
  program with `Program.withErrorHandler` to record failures and keep the loop
  alive instead.
- **Subscription exceptions** happen on the subscription's own thread. The host
  does not catch them. Catch inside the subscription and dispatch an error
  message, or let Elmish's error handler observe the dispatch failure.
- **Post exceptions** happen when `Elmish.Avalonia.Glue.ElmishHost.post` runs
  inline (UI-thread exceptions surface inline) or when
  `Dispatcher.UIThread.Post` cannot queue to a shut-down dispatcher (which can
  throw on the posting thread).

See the [threading guarantees](threading-dispatch.html) and the
[dispatcher bridge](../reference/avalonia/dispatcher.html).

## Stale async results

An async command can finish after a later request or after the view is gone.
Give each request an identity and ignore results that no longer apply.

```fsharp
open Elmish

type Model =
    { RequestId: int
      Latest: int option }

type Msg =
    | Refresh
    | Received of requestId: int * value: int

let update msg model =
    match msg with
    | Refresh ->
        let nextId = model.RequestId + 1

        { model with RequestId = nextId; Latest = None },
        Cmd.OfFunc.perform (fun () -> nextId, 42) () Received

    | Received (requestId, value) when requestId = model.RequestId ->
        { model with Latest = Some value }, Cmd.none

    | Received _ ->
        model, Cmd.none
```

A result from an older request carries the old id and is ignored. Disposal of
the host stops subscriptions but does not cancel an already-running command, so
the id guard (or a cancelled-request flag) is also the defence for work that
finishes after teardown. See [commands and subscriptions](background-commands.html).
