# Commands and subscriptions

Use messages to bring finite work and ongoing producers into Elmish; neither producer should mutate bound state. See [threading and dispatch guarantees](threading-dispatch.html) for the host boundary.

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

## Stop work with the owning view

Keep the <a href="../api/Elmish.Glue.Core.ElmishHostConnection`1.html">`ElmishHostConnection`</a> returned by `start` or `startAndBind`. Dispose
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
