namespace Elmish.Glue.Core

open System
open System.Threading
open System.Collections.Generic
open Elmish

/// Owns a running Elmish program and exposes a stable dispatch surface.
///
/// Hold onto the returned connection for as long as the owning window,
/// application, or view needs the Elmish loop to stay alive. Dispose the
/// connection when that owner shuts down so Elmish subscriptions can
/// terminate cleanly.
[<Sealed>]
type ElmishHostConnection<'Msg> internal (dispatch : Action<'Msg>, dispose : Action) =
    /// Gets a stable dispatcher that forwards messages into the running
    /// Elmish loop until the connection is disposed.
    member _.Dispatch = dispatch

    /// Stops the hosted Elmish loop by sending an internal termination
    /// message through the dispatch pipeline.
    interface IDisposable with
        member _.Dispose() = dispose.Invoke()

/// Starts an Elmish program and connects it to a host-controlled update sink.
///
/// After every model update the provided callback is invoked through the
/// supplied posting function. Higher-level UI packages can use this to route
/// updates through a framework-specific UI thread or synchronization context.
module ElmishHost =

    type private HostMsg<'Msg> =
        | User of 'Msg
        | Stop

    let private wrapProgram (program : Program<unit, 'Model, 'Msg, unit>) =
        program
        |> Program.map
            (fun init () ->
                let model, cmd = init ()
                model, Cmd.map User cmd)
            (fun update msg model ->
                match msg with
                | User userMsg ->
                    let nextModel, cmd = update userMsg model
                    nextModel, Cmd.map User cmd
                | Stop ->
                    model, Cmd.none)
            (fun view model dispatch ->
                view model (User >> dispatch))
            (fun setState model dispatch ->
                setState model (User >> dispatch))
            (fun subscribe model ->
                subscribe model
                |> Sub.map "ElmishHost" User)
            (fun (terminate, onTerminate) ->
                (function
                 | User msg -> terminate msg
                 | Stop -> true),
                onTerminate)

    let startWithPost
        (post : Action<Action>)
        (program : Program<unit, 'Model, 'Msg, unit>)
        (onUpdate : Action<'Model>)
        : ElmishHostConnection<'Msg> =

        let mutable dispatch = Action<HostMsg<'Msg>>(ignore)
        let mutable disposed = 0
        let dispatchGate = obj ()
        let pending = Queue<HostMsg<'Msg>>()
        let mutable draining = false

        let rec drain (send : HostMsg<'Msg> -> unit) () =
            let next =
                lock dispatchGate (fun () ->
                    if pending.Count = 0 then
                        draining <- false
                        None
                    else
                        Some(pending.Dequeue()))
            match next with
            | Some msg ->
                send msg
                drain send ()
            | None -> ()

        let enqueue send msg =
            let schedule =
                lock dispatchGate (fun () ->
                    pending.Enqueue(msg)
                    if draining then false
                    else
                        draining <- true
                        true)
            if schedule then post.Invoke(Action(drain send))

        let dispatchUser =
            Action<'Msg>(fun msg ->
                if Volatile.Read(&disposed) = 0 then
                    dispatch.Invoke(User msg))

        let dispose =
            Action(fun () ->
                if Interlocked.Exchange(&disposed, 1) = 0 then
                    dispatch.Invoke(Stop))

        program
        |> wrapProgram
        |> Program.withSetState (fun model d ->
            dispatch <- Action<HostMsg<'Msg>>(fun msg -> d msg)
            if Volatile.Read(&disposed) = 0 then
                post.Invoke(Action(fun () -> onUpdate.Invoke(model))))
        |> Program.runWithDispatch (fun dispatch msg -> enqueue dispatch msg) ()

        new ElmishHostConnection<'Msg>(dispatchUser, dispose)

    /// Serializes dispatch in FIFO order. The idle dispatching thread drains
    /// messages; no thread affinity is guaranteed. Model delivery runs on that thread.
    let start
        (program : Program<unit, 'Model, 'Msg, unit>)
        (onUpdate : Action<'Model>)
        : ElmishHostConnection<'Msg> =
        startWithPost (Action<Action>(fun action -> action.Invoke())) program onUpdate

    let startAndBindWithPost
        (post : Action<Action>)
        (program : Program<unit, 'Model, 'Msg, unit>)
        (onUpdate : Action<'Model>)
        (setDispatch : Action<Action<'Msg>>)
        : ElmishHostConnection<'Msg> =

        let host = startWithPost post program onUpdate
        setDispatch.Invoke(host.Dispatch)
        host

    /// Dispatch and model delivery run on the dispatching thread, with FIFO
    /// serialization but no thread affinity.
    let startAndBind
        (program : Program<unit, 'Model, 'Msg, unit>)
        (onUpdate : Action<'Model>)
        (setDispatch : Action<Action<'Msg>>)
        : ElmishHostConnection<'Msg> =

        startAndBindWithPost (Action<Action>(fun action -> action.Invoke())) program onUpdate setDispatch
