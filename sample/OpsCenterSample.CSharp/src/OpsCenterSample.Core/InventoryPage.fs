module OpsCenterSample.Core.InventoryPage

open System
open Elmish
open OpsCenterSample.Core.Ui

type Item =
    {
        Id: Guid
        Name: string
        InStock: int
        Target: int
    }

type Model =
    {
        ShowLowOnly: bool
        Items: Item list
    }

type Msg =
    | ToggleLowOnly
    | Restock of Guid
    | PickOne of Guid

type ItemView =
    {
        Id: Guid
        Name: string
        Stock: string
        Gap: string
    }

type View =
    {
        ShowLowOnly: bool
        Summary: MetricCard list
        Items: ItemView list
    }

let private item name inStock target : Item =
    { Id = Guid.NewGuid(); Name = name; InStock = inStock; Target = target }

let init () : Model * Cmd<Msg> =
    {
        ShowLowOnly = false
        Items =
            [
                item "Wrap film" 18 40
                item "Cold packs" 52 60
                item "Label rolls" 11 35
                item "Mailer cartons" 64 70
            ]
    },
    Cmd.none

let update msg (model: Model) : Model * Cmd<Msg> =
    let updateItem id f =
        model.Items |> List.map (fun i -> if i.Id = id then f i else i)

    match msg with
    | ToggleLowOnly ->
        { model with ShowLowOnly = not model.ShowLowOnly }, Cmd.none
    | Restock id ->
        { model with Items = updateItem id (fun i -> { i with InStock = min i.Target (i.InStock + 12) }) }, Cmd.none
    | PickOne id ->
        { model with Items = updateItem id (fun i -> { i with InStock = max 0 (i.InStock - 1) }) }, Cmd.none

let toView (model: Model) : View =
    let belowTarget =
        model.Items
        |> List.filter (fun (item: Item) -> item.InStock < item.Target)
        |> List.length

    let needed =
        model.Items
        |> List.sumBy (fun (item: Item) -> item.Target - item.InStock)

    let visibleItems =
        model.Items
        |> List.filter (fun (item: Item) ->
            not model.ShowLowOnly || item.InStock < item.Target)

    {
        ShowLowOnly = model.ShowLowOnly
        Summary =
            [
                { Key = "tracked"; Label = "Tracked SKUs"; Value = string model.Items.Length; Hint = "Critical consumables only." }
                { Key = "below"; Label = "Below target"; Value = string belowTarget; Hint = "Need a replenishment pass." }
                { Key = "needed"; Label = "Units short"; Value = string needed; Hint = "Gap against preferred stock levels." }
            ]
        Items =
            visibleItems
            |> List.map (fun (item: Item) ->
                {
                    Id = item.Id
                    Name = item.Name
                    Stock = $"{item.InStock} / {item.Target}"
                    Gap =
                        if item.InStock >= item.Target then "On target"
                        else $"{item.Target - item.InStock} short"
                })
    }
