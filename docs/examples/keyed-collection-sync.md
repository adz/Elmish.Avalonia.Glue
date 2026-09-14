---
title: Keyed collection patching example
project: src/Elmish.Glue.Core/Elmish.Glue.Core.fsproj
---

# Keyed collection patching example

This example shows `ObservableCollectionExtensions.SyncWith` preserving item identity.

## Run the patch

[Source file](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/tools/DocsExamples/Examples/KeyedCollectionSync.fs)

```fsharp run
open System
open System.Collections.ObjectModel
open Elmish.Glue.Core

type Row =
    { Id: Guid
      Name: string }

type RowVm(row: Row) =
    let mutable row = row

    member _.Id = row.Id
    member _.Name = row.Name

    interface IProjection<Row> with
        member _.Update(next) = row <- next

let rows =
    ObservableCollection<RowVm>(
        [ RowVm({ Id = Guid.Parse("11111111-1111-1111-1111-111111111111"); Name = "A" })
          RowVm({ Id = Guid.Parse("22222222-2222-2222-2222-222222222222"); Name = "B" }) ])

let firstBefore = rows[0]
let models =
    [ { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"); Name = "B updated" }
      { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"); Name = "A updated" } ]

rows.SyncWith(
    models,
    Func<Row, Guid>(fun row -> row.Id),
    Func<RowVm, Guid>(fun vm -> vm.Id),
    Func<Row, RowVm>(fun row -> RowVm(row)),
    Action<RowVm, Row>(fun vm row -> (vm :> IProjection<Row>).Update(row)))

printfn "Count = %d" rows.Count
printfn "Order = %s, %s" rows[0].Name rows[1].Name
printfn "Identity preserved = %b" (obj.ReferenceEquals(firstBefore, rows[1]))
```
