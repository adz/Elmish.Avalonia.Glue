module OpsCenterSample.Core.OrdersPage

open System
open Elmish
open OpsCenterSample.Core.Ui

type OrderStatus =
    | Draft
    | Packed
    | Shipped

type Order =
    {
        Id: Guid
        Customer: string
        Route: string
        Status: OrderStatus
        Total: decimal
    }

type Model =
    {
        ShowActiveOnly: bool
        Orders: Order list
    }

type Msg =
    | ToggleActiveOnly
    | AdvanceOrder of Guid
    | AddRushOrder

type OrderView =
    {
        Id: Guid
        Customer: string
        Route: string
        Status: string
        Total: string
        AdvanceLabel: string
    }

type View =
    {
        ShowActiveOnly: bool
        Summary: MetricCard list
        Orders: OrderView list
    }

let private order customer route status total : Order =
    { Id = Guid.NewGuid(); Customer = customer; Route = route; Status = status; Total = total }

let init () : Model * Cmd<Msg> =
    {
        ShowActiveOnly = true
        Orders =
            [
                order "Monarch Labs" "Adelaide metro" Packed 1840m
                order "Blue Canyon" "Port Wakefield" Draft 920m
                order "Atelier North" "Barossa" Shipped 1260m
                order "Kite Foods" "Murray Bridge" Packed 670m
            ]
    },
    Cmd.none

let private nextStatus =
    function
    | Draft -> Packed
    | Packed -> Shipped
    | Shipped -> Shipped

let update msg (model: Model) : Model * Cmd<Msg> =
    match msg with
    | ToggleActiveOnly ->
        { model with ShowActiveOnly = not model.ShowActiveOnly }, Cmd.none
    | AddRushOrder ->
        let rush = order "Rush intake" "Airport lane" Draft 1480m
        { model with Orders = rush :: model.Orders }, Cmd.none
    | AdvanceOrder id ->
        let orders =
            model.Orders
            |> List.map (fun o ->
                if o.Id = id then { o with Status = nextStatus o.Status }
                else o)

        { model with Orders = orders }, Cmd.none

let private statusText =
    function
    | Draft -> "Draft"
    | Packed -> "Packed"
    | Shipped -> "Shipped"

let private advanceLabel =
    function
    | Draft -> "Pack"
    | Packed -> "Ship"
    | Shipped -> "Done"

let toView (model: Model) : View =
    let activeCount =
        model.Orders
        |> List.filter (fun (order: Order) -> order.Status <> Shipped)
        |> List.length

    let packedCount =
        model.Orders
        |> List.filter (fun (order: Order) -> order.Status = Packed)
        |> List.length

    let pipelineValue =
        model.Orders
        |> List.sumBy (fun (order: Order) -> order.Total)

    let visibleOrders =
        model.Orders
        |> List.filter (fun (order: Order) ->
            not model.ShowActiveOnly || order.Status <> Shipped)

    {
        ShowActiveOnly = model.ShowActiveOnly
        Summary =
            [
                { Key = "active"; Label = "Active lanes"; Value = string activeCount; Hint = "Draft and packed orders still moving." }
                { Key = "packed"; Label = "Packed today"; Value = string packedCount; Hint = "Waiting on the final dispatch pass." }
                { Key = "value"; Label = "Pipeline value"; Value = $"${pipelineValue:N0}"; Hint = "Across every order in the queue." }
            ]
        Orders =
            visibleOrders
            |> List.map (fun (order: Order) ->
                {
                    Id = order.Id
                    Customer = order.Customer
                    Route = order.Route
                    Status = statusText order.Status
                    Total = $"${order.Total:N0}"
                    AdvanceLabel = advanceLabel order.Status
                })
    }
