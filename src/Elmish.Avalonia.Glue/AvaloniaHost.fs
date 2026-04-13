namespace Elmish.Avalonia.Glue

open System
open Avalonia.Threading
open Elmish

module ElmishHost =
    let private postToUiThread =
        Action<Action>(fun action ->
            Dispatcher.UIThread.Post(fun () -> action.Invoke()) |> ignore)

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
