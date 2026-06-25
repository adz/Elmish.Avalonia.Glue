---
sidebar_position: 2
title: Projection snapshot-host example
---

# Projection snapshot-host example

This example shows a single immutable snapshot flowing through `SnapshotHost<'T>`.

## Source

[Source file](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/tools/DocsExamples/Examples/ProjectionSnapshotHost.fs)

```fsharp
type Snapshot =
    { Name: string
      Count: int }

let host = SnapshotHost({ Name = "Avery"; Count = 1 })
host.Update({ Name = "Avery"; Count = 2 })

printfn "Current.Name = %s" host.Current.Name
printfn "Current.Count = %d" host.Current.Count
```

## Observed output

```text
Current.Name = Avery
Current.Count = 2
```
