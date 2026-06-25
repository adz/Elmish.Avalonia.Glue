module DocsExamples.Examples.KeyedCollectionSync

open System
open System.Collections.ObjectModel
open System.IO
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

let run outputDir =
    let rows =
        ObservableCollection<RowVm>(
            [ RowVm({ Id = Guid.Parse("11111111-1111-1111-1111-111111111111"); Name = "A" })
              RowVm({ Id = Guid.Parse("22222222-2222-2222-2222-222222222222"); Name = "B" }) ])

    let firstBefore = rows[0]

    rows.SyncWith(
        [ { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"); Name = "B updated" }
          { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"); Name = "A updated" } ],
        Func<Row, Guid>(fun row -> row.Id),
        Func<RowVm, Guid>(fun vm -> vm.Id),
        Func<Row, RowVm>(fun row -> RowVm(row)),
        Action<RowVm, Row>(fun vm row -> (vm :> IProjection<Row>).Update(row)))

    let sourceUrl =
        "https://github.com/adz/Elmish.Avalonia.Glue/blob/main/tools/DocsExamples/Examples/KeyedCollectionSync.fs"

    let source =
        """
type Row =
    { Id: Guid
      Name: string }

type RowVm(row: Row) =
    let mutable row = row

    member _.Id = row.Id
    member _.Name = row.Name

    interface IProjection<Row> with
        member _.Update(next) = row <- next

rows.SyncWith(
    models,
    Func<Row, Guid>(fun row -> row.Id),
    Func<RowVm, Guid>(fun vm -> vm.Id),
    Func<Row, RowVm>(fun row -> RowVm(row)),
    Action<RowVm, Row>(fun vm row -> (vm :> IProjection<Row>).Update(row)))
"""

    let output =
        [ $"Count = {rows.Count}"
          $"Order = {rows[0].Name}, {rows[1].Name}"
          $"Identity preserved = {obj.ReferenceEquals(firstBefore, rows[1])}" ]
        |> String.concat Environment.NewLine

    File.WriteAllText(Path.Combine(outputDir, "keyed-collection-sync.md"), $"""---
sidebar_position: 4
title: Keyed collection patching example
---

# Keyed collection patching example

This example shows `ObservableCollectionExtensions.SyncWith` preserving item identity.

## Source

[Source file]({sourceUrl})

```fsharp
{source.Trim()}
```

## Observed output

```text
{output}
```
""")
