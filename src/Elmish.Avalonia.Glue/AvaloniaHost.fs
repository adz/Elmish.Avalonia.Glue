namespace Elmish.Avalonia.Glue

open System
open Avalonia.Threading
open Elmish

module ElmishHost =
    /// Executes an action immediately on the Avalonia UI thread, or posts it
    /// to that thread when called from another thread. Suitable for runtime
    /// options that accept a Post callback.
    let post (action : unit -> unit) : unit =
        if Dispatcher.UIThread.CheckAccess() then
            action ()
        else
            Dispatcher.UIThread.Post(action) |> ignore

    let private postToUiThread =
        Action<Action>(fun action -> post action.Invoke)

    let start
        (program : Program<unit, 'Model, 'Msg, unit>)
        (onUpdate : Action<'Model>)
        : ElmishHostConnection<'Msg> =

        Elmish.Glue.Core.ElmishHost.startWithPost postToUiThread program onUpdate

    let startAndBind
        (program : Program<unit, 'Model, 'Msg, unit>)
        (onUpdate : Action<'Model>)
        (setDispatch : Action<Action<'Msg>>)
        : ElmishHostConnection<'Msg> =

        Elmish.Glue.Core.ElmishHost.startAndBindWithPost postToUiThread program onUpdate setDispatch
