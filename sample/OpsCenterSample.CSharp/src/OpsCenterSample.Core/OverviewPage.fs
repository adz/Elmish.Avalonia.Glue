module OpsCenterSample.Core.OverviewPage

open System
open Elmish
open OpsCenterSample.Core.Ui

type Activity =
    {
        Id: Guid
        Title: string
        Detail: string
        Time: DateTimeOffset
    }

type Model =
    {
        Revenue: int
        OpenOrders: int
        LowStockItems: int
        TeamLoad: int
        Activity: Activity list
    }

type Msg =
    | RefreshSnapshot

type ActivityView =
    {
        Id: Guid
        Title: string
        Detail: string
        TimeText: string
    }

type View =
    {
        Highlights: MetricCard list
        Activity: ActivityView list
    }

let private activity title detail minutesAgo =
    {
        Id = Guid.NewGuid()
        Title = title
        Detail = detail
        Time = DateTimeOffset.Now.AddMinutes(float -minutesAgo)
    }

let init () : Model * Cmd<Msg> =
    {
        Revenue = 184200
        OpenOrders = 42
        LowStockItems = 7
        TeamLoad = 83
        Activity =
            [
                activity "North dock caught up" "Picking backlog cleared for lane B." 7
                activity "Rush order approved" "Monarch Labs requested same-day packing." 19
                activity "Cycle count completed" "Aisle C variances dropped to zero." 33
            ]
    },
    Cmd.none

let update msg (model: Model) : Model * Cmd<Msg> =
    match msg with
    | RefreshSnapshot ->
        let nextRevenue = model.Revenue + 1200
        let nextOrders = max 5 (model.OpenOrders - 1)
        let nextLowStock = if model.LowStockItems > 2 then model.LowStockItems - 1 else model.LowStockItems + 2
        let nextLoad = if model.TeamLoad > 88 then 81 else model.TeamLoad + 2

        {
            model with
                Revenue = nextRevenue
                OpenOrders = nextOrders
                LowStockItems = nextLowStock
                TeamLoad = nextLoad
                Activity =
                    activity "Snapshot refreshed" $"Revenue moved to ${nextRevenue:N0}." 0
                    :: model.Activity
                    |> List.truncate 6
        },
        Cmd.none

let toView (model: Model) : View =
    {
        Highlights =
            [
                { Key = "revenue"; Label = "Live revenue"; Value = $"${model.Revenue:N0}"; Hint = "Up after the morning wave." }
                { Key = "orders"; Label = "Open orders"; Value = string model.OpenOrders; Hint = "Ready for packing or dispatch." }
                { Key = "stock"; Label = "Low stock"; Value = string model.LowStockItems; Hint = "Items below target level." }
                { Key = "load"; Label = "Team load"; Value = sprintf "%d%%" model.TeamLoad; Hint = "Current planned capacity." }
            ]
        Activity =
            model.Activity
            |> List.map (fun item ->
                {
                    Id = item.Id
                    Title = item.Title
                    Detail = item.Detail
                    TimeText = item.Time.ToString("HH:mm")
                })
    }
