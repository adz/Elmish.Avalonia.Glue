namespace Elmish.Avalonia.Glue

open System
open System.Threading
open Elmish
open Avalonia.Threading

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

/// Starts an Elmish program and connects it to a ViewModel.
///
/// After every model update the onUpdate callback is posted to the Avalonia
/// UI thread. Typically onUpdate calls vm.Update(model).
///
/// Use this module when an Avalonia application wants to keep Elmish as the
/// source of truth while projecting state into ordinary ViewModels.
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

    let start
        (program : Program<unit, 'Model, 'Msg, unit>)
        (onUpdate : Action<'Model>)
        : ElmishHostConnection<'Msg> =

        let mutable dispatch = Action<HostMsg<'Msg>>(ignore)
        let mutable disposed = 0

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
                Dispatcher.UIThread.Post(fun () -> onUpdate.Invoke(model)))
        |> Program.run

        new ElmishHostConnection<'Msg>(dispatchUser, dispose)

    let startAndBind
        (program : Program<unit, 'Model, 'Msg, unit>)
        (onUpdate : Action<'Model>)
        (setDispatch : Action<Action<'Msg>>)
        : ElmishHostConnection<'Msg> =

        let host = start program onUpdate
        setDispatch.Invoke(host.Dispatch)
        host
