---
sidebar_position: 4
title: Keyed collection patching example
---

# Keyed collection patching example

This example shows `ObservableCollectionExtensions.SyncWith` preserving item identity.

## Source

[Source file](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/tools/DocsExamples/Examples/KeyedCollectionSync.fs)

```fsharp
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
```

## Observed output

```text
Count = 2
Order = B updated, A updated
Identity preserved = True
```
