# Background work

Background work reaches the UI through messages. A timer, network request, or
file watcher must not mutate a projection, host, node, or bound collection.

## Keep messages at the concurrency boundary

The background source reports a fact. Elmish decides what that fact means for
the model. The normal host update path then publishes the next snapshot.

```text
background callback
→ dispatch ReadingReceived
→ Elmish update
→ new immutable model
→ new view snapshot
→ Avalonia UI-thread post
→ host.Update
→ PropertyChanged
→ AXAML refresh
```

This path gives every state change the same ordering and debugging story,
regardless of which thread produced the message.

## Define the state transition first

The model represents loading, success, and failure explicitly. Neither the
background operation nor the view decides how those states relate.

```fsharp
open System
open System.Timers
open Elmish

type Model =
    { IsLoading: bool
      LatestReading: int option
      Error: string option
      TickCount: int }

type Msg =
    | Refresh
    | ReadingReceived of int
    | ReadingFailed of string
    | ClockTick of DateTimeOffset
```

## Use a command for finite work

A command starts because `update` handled a message, then dispatches one of
the result messages. HTTP calls, database reads, and one-shot calculations fit
this shape.

```fsharp
let fetchReading () =
    async {
        do! Async.Sleep 10
        return 42
    }

let update msg model =
    match msg with
    | Refresh ->
        { model with IsLoading = true; Error = None },
        Cmd.OfAsync.either
            fetchReading
            ()
            ReadingReceived
            (fun error -> ReadingFailed error.Message)

    | ReadingReceived reading ->
        { model with
            IsLoading = false
            LatestReading = Some reading
            Error = None },
        Cmd.none

    | ReadingFailed error ->
        { model with IsLoading = false; Error = Some error }, Cmd.none

    | ClockTick _ ->
        { model with TickCount = model.TickCount + 1 }, Cmd.none
```

Do not set `host.IsLoading` before starting the request. Dispatch `Refresh` and
let the next snapshot publish the loading state.

## Use a subscription for an ongoing source

A subscription connects a long-lived producer such as a timer, device,
WebSocket, or file watcher. In Elmish's keyed subscription API, each factory returns an `IDisposable` that stops its producer; `Program.withSubscription` takes the list of `(key segments, factory)` pairs. Elmish tracks the stable key across model updates: keep one key per logical producer, rather than generating a new key each update. Dispose stops future timer events but callbacks already in flight may still dispatch; handle stale results in `update`.

```fsharp
let subscriptions _model =
    [ [ "clock-tick" ],
      fun dispatch ->
          let timer = new Timer(1000.0)
          timer.AutoReset <- true
          timer.Elapsed.Add(fun _ ->
              dispatch (ClockTick DateTimeOffset.Now))
          timer.Start()

          { new IDisposable with
              member _.Dispose() =
                  timer.Stop()
                  timer.Dispose() } ]
```

Attach it with `Program.withSubscription subscriptions`. Use a stable
subscription key so Elmish can identify the producer across model changes.

## Publish only through the Avalonia host

The subscription callback may run on a worker thread. Dispatch through the
Avalonia host: it queues messages in FIFO order and drains them on `Dispatcher.UIThread`, running
inline if already there and posting otherwise. This serializes the Elmish
message queue as well as the bound viewmodel updates. Dispatching directly
into an unprotected Elmish queue from multiple threads is not safe.

An idle UI-thread dispatch runs inline; if worker messages are waiting, it joins
that queue rather than overtaking them. A dispatch during a drain never re-enters
Elmish. The framework-neutral core host instead drains on the dispatching thread
that finds the queue idle: it guarantees serialization and FIFO order, not thread
affinity. Each drain handles only the queue snapshot it found at entry; later arrivals are scheduled on the thread pool (which calls the supplied `post`), unless another dispatcher takes the idle queue first, so
one producer does not have to drain an indefinitely growing queue. Update exceptions propagate to the draining caller and stop that batch; the next dispatch can resume remaining queued messages. A failure from `post` is not recovered here and can leave the queue waiting. Disposal queues a stop message, so messages already queued may run first. Core `startAndBind` delivers models on the draining thread.

The Avalonia host publishes the next model on the UI thread before raising
notifications or patching a bound collection.

Do not call `Dispatcher.UIThread.Post` inside every background callback. The
host boundary already owns UI-thread publication.

## Stop work with the owning view

Keep the `ElmishHostConnection` returned by `start` or `startAndBind`. Dispose
it when the window or application shuts down so Elmish disposes subscriptions.

For finite operations, decide how to handle cancellation and stale responses.
A request identifier in the model is often enough to ignore a result from an
older request.

## Projection and ElmView receive the same result

Projection receives the next model in `projection.Update`. ElmView receives
the next derived view record in `host.Update`. Neither style changes how the
background source dispatches messages.

See the HTTP and clock implementations in the
[sample applications](sample-applications.html), then use the generated
[host lifetime API](../api.html) for exact signatures.
