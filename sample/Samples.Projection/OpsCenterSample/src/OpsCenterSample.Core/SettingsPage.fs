module OpsCenterSample.Core.SettingsPage

open Elmish

type Accent =
    | Moss
    | Ember
    | Harbor

type Model =
    {
        Notifications: bool
        CompactLists: bool
        Accent: Accent
    }

type Msg =
    | ToggleNotifications
    | ToggleCompactLists
    | SetAccent of Accent

type View =
    {
        NotificationsEnabled: bool
        CompactLists: bool
        Accent: string
    }

let init () : Model * Cmd<Msg> =
    {
        Notifications = true
        CompactLists = false
        Accent = Moss
    },
    Cmd.none

let update msg (model: Model) : Model * Cmd<Msg> =
    match msg with
    | ToggleNotifications ->
        { model with Notifications = not model.Notifications }, Cmd.none
    | ToggleCompactLists ->
        { model with CompactLists = not model.CompactLists }, Cmd.none
    | SetAccent accent ->
        { model with Accent = accent }, Cmd.none

let toView (model: Model) : View =
    {
        NotificationsEnabled = model.Notifications
        CompactLists = model.CompactLists
        Accent =
            match model.Accent with
            | Moss -> "Moss"
            | Ember -> "Ember"
            | Harbor -> "Harbor"
    }
