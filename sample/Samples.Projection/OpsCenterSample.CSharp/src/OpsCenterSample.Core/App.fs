module OpsCenterSample.Core.App

open Elmish

type Page =
    | Overview
    | Orders
    | Inventory
    | Team
    | Settings

type PageInfo =
    {
        Page: Page
        Key: string
        Title: string
        Subtitle: string
        NavTitle: string
        NavCaption: string
    }

type Model =
    {
        CurrentPage: Page
        Overview: OverviewPage.Model
        Orders: OrdersPage.Model
        Inventory: InventoryPage.Model
        Team: TeamPage.Model
        Settings: SettingsPage.Model
    }

type Msg =
    | Navigate of Page
    | OverviewMsg of OverviewPage.Msg
    | OrdersMsg of OrdersPage.Msg
    | InventoryMsg of InventoryPage.Msg
    | TeamMsg of TeamPage.Msg
    | SettingsMsg of SettingsPage.Msg

let init () : Model * Cmd<Msg> =
    let overview, overviewCmd = OverviewPage.init ()
    let orders, ordersCmd = OrdersPage.init ()
    let inventory, inventoryCmd = InventoryPage.init ()
    let team, teamCmd = TeamPage.init ()
    let settings, settingsCmd = SettingsPage.init ()

    {
        CurrentPage = Overview
        Overview = overview
        Orders = orders
        Inventory = inventory
        Team = team
        Settings = settings
    },
    Cmd.batch
        [
            Cmd.map OverviewMsg overviewCmd
            Cmd.map OrdersMsg ordersCmd
            Cmd.map InventoryMsg inventoryCmd
            Cmd.map TeamMsg teamCmd
            Cmd.map SettingsMsg settingsCmd
        ]

let update msg (model: Model) : Model * Cmd<Msg> =
    match msg with
    | Navigate page ->
        { model with CurrentPage = page }, Cmd.none
    | OverviewMsg subMsg ->
        let next, cmd = OverviewPage.update subMsg model.Overview
        { model with Overview = next }, Cmd.map OverviewMsg cmd
    | OrdersMsg subMsg ->
        let next, cmd = OrdersPage.update subMsg model.Orders
        { model with Orders = next }, Cmd.map OrdersMsg cmd
    | InventoryMsg subMsg ->
        let next, cmd = InventoryPage.update subMsg model.Inventory
        { model with Inventory = next }, Cmd.map InventoryMsg cmd
    | TeamMsg subMsg ->
        let next, cmd = TeamPage.update subMsg model.Team
        { model with Team = next }, Cmd.map TeamMsg cmd
    | SettingsMsg subMsg ->
        let next, cmd = SettingsPage.update subMsg model.Settings
        { model with Settings = next }, Cmd.map SettingsMsg cmd

let pageInfos =
    [
        { Page = Overview; Key = "OVR"; Title = "Overview"; Subtitle = "A live operational summary projected from the Elmish model."; NavTitle = "Overview"; NavCaption = "Live snapshot" }
        { Page = Orders; Key = "ORD"; Title = "Orders"; Subtitle = "Active dispatch work with row-level commands and filtering."; NavTitle = "Orders"; NavCaption = "Dispatch queue" }
        { Page = Inventory; Key = "INV"; Title = "Inventory"; Subtitle = "Tracked consumables with stable list projections."; NavTitle = "Inventory"; NavCaption = "Replenishment" }
        { Page = Team; Key = "TEAM"; Title = "Team"; Subtitle = "Load balancing and on-call rotation for the current shift."; NavTitle = "Team"; NavCaption = "Shift load" }
        { Page = Settings; Key = "CFG"; Title = "Settings"; Subtitle = "UI-only preferences still driven through Elmish messages."; NavTitle = "Settings"; NavCaption = "Operator prefs" }
    ]

let pageInfo page =
    pageInfos
    |> List.find (fun info -> info.Page = page)

let program =
    Program.mkProgram init update (fun _ _ -> ())
