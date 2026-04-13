module OpsCenterSample.Core.TeamPage

open System
open Elmish
open OpsCenterSample.Core.Ui

type Member =
    {
        Id: Guid
        Name: string
        Focus: string
        Load: int
        OnCall: bool
    }

type Model =
    {
        Members: Member list
    }

type Msg =
    | EaseLoad of Guid
    | AddLoad of Guid
    | RotateOnCall

type MemberView =
    {
        Id: Guid
        Name: string
        Focus: string
        Load: string
        Availability: string
    }

type View =
    {
        Summary: MetricCard list
        Members: MemberView list
    }

let private member' name focus load onCall =
    { Id = Guid.NewGuid(); Name = name; Focus = focus; Load = load; OnCall = onCall }

let init () : Model * Cmd<Msg> =
    {
        Members =
            [
                member' "Mina" "Inbound" 78 true
                member' "Jonah" "Scheduling" 64 false
                member' "Pia" "Counts" 57 false
                member' "Lewis" "Dispatch" 72 false
            ]
    },
    Cmd.none

let private adjust id delta (model: Model) : Model =
    {
        model with
            Members =
                model.Members
                |> List.map (fun m ->
                    if m.Id = id then { m with Load = max 20 (min 95 (m.Load + delta)) }
                    else m)
    }

let update msg (model: Model) : Model * Cmd<Msg> =
    match msg with
    | EaseLoad id -> adjust id -8 model, Cmd.none
    | AddLoad id -> adjust id 8 model, Cmd.none
    | RotateOnCall ->
        let members =
            match model.Members with
            | [] -> []
            | head :: tail ->
                let ordered = tail @ [ head ]
                ordered |> List.mapi (fun i m -> { m with OnCall = i = 0 })

        { model with Members = members }, Cmd.none

let toView (model: Model) : View =
    let onCallName =
        model.Members
        |> List.tryFind (fun (person: Member) -> person.OnCall)
        |> Option.map (fun person -> person.Name)
        |> Option.defaultValue "Unassigned"

    let memberCount = model.Members.Length

    let avgLoad =
        match memberCount with
        | 0 -> 0
        | count ->
            model.Members
            |> List.sumBy (fun (person: Member) -> person.Load)
            |> fun total -> total / count

    {
        Summary =
            [
                { Key = "headcount"; Label = "Headcount"; Value = string memberCount; Hint = "Active operators in rotation." }
                { Key = "load"; Label = "Average load"; Value = sprintf "%d%%" avgLoad; Hint = "Across the current shift." }
                { Key = "oncall"; Label = "On call"; Value = onCallName; Hint = "Escalation contact for the next hour." }
            ]
        Members =
            model.Members
            |> List.map (fun (person: Member) ->
                {
                    Id = person.Id
                    Name = person.Name
                    Focus = person.Focus
                    Load = sprintf "%d%%" person.Load
                    Availability = if person.OnCall then "On call" else "Available"
                })
    }
