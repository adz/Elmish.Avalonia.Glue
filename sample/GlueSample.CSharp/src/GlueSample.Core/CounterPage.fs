module GlueSample.Core.CounterPage

open System
open Elmish

type LogEntry = { Id: Guid; Message: string; Time: DateTimeOffset }

type Model =
    {
        Count : int
        Log   : LogEntry list
    }

type Msg =
    | Increment
    | Decrement
    | Reset

let init () = { Count = 0; Log = [] }, Cmd.none

let private entry text =
    { Id = Guid.NewGuid(); Message = text; Time = DateTimeOffset.Now }

let update msg model =
    match msg with
    | Increment ->
        let n = model.Count + 1
        { model with Count = n; Log = entry $"-> {n}" :: model.Log }, Cmd.none
    | Decrement ->
        let n = model.Count - 1
        { model with Count = n; Log = entry $"-> {n}" :: model.Log }, Cmd.none
    | Reset ->
        { model with Count = 0; Log = [] }, Cmd.none
        // { model with Count = 0; Log = entry "reset" :: model.Log }, Cmd.none

let program =
    Program.mkProgram init update (fun _ _ -> ())
