module DocsExamples.Examples.ProjectionSnapshotHost

open System
open System.IO
open Elmish.Avalonia.Glue.Projection

type Snapshot =
    { Name: string
      Count: int }

let run outputDir =
    let host = SnapshotHost({ Name = "Avery"; Count = 1 })
    host.Update({ Name = "Avery"; Count = 2 })

    let sourceUrl =
        "https://github.com/adz/Elmish.Avalonia.Glue/blob/main/tools/DocsExamples/Examples/ProjectionSnapshotHost.fs"

    let source =
        """
open Elmish.Avalonia.Glue.Projection

type Snapshot =
    { Name: string
      Count: int }

let host = SnapshotHost({ Name = "Avery"; Count = 1 })
host.Update({ Name = "Avery"; Count = 2 })

printfn "Current.Name = %s" host.Current.Name
printfn "Current.Count = %d" host.Current.Count
"""

    let output =
        [ $"Current.Name = {host.Current.Name}"
          $"Current.Count = {host.Current.Count}" ]
        |> String.concat Environment.NewLine

    File.WriteAllText(Path.Combine(outputDir, "projection-snapshot-host.md"), $"""---
title: Projection snapshot-host example
project: src/Elmish.Avalonia.Glue.Projection/Elmish.Avalonia.Glue.Projection.fsproj
---

# Projection snapshot-host example

This example shows a single immutable snapshot flowing through `SnapshotHost<'T>`.

## Run the snapshot transition

[Source file]({sourceUrl})

```fsharp run
{source.Trim()}
```
""")
