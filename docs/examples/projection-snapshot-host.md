---
title: Projection snapshot-host example
project: src/Elmish.Avalonia.Glue.Projection/Elmish.Avalonia.Glue.Projection.fsproj
---

# Projection snapshot-host example

This example shows a single immutable snapshot flowing through `SnapshotHost<'T>`.

## Run the snapshot transition

[Source file](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/tools/DocsExamples/Examples/ProjectionSnapshotHost.fs)

```fsharp run
open Elmish.Avalonia.Glue.Projection

type Snapshot =
    { Name: string
      Count: int }

let host = SnapshotHost({ Name = "Avery"; Count = 1 })
host.Update({ Name = "Avery"; Count = 2 })

printfn "Current.Name = %s" host.Current.Name
printfn "Current.Count = %d" host.Current.Count
```
