module DocsExamples.Program

open System
open System.IO

[<EntryPoint>]
let main args =
    let outputDir =
        args
        |> Array.windowed 2
        |> Array.tryFind (fun pair -> pair[0] = "--output")
        |> Option.map (fun pair -> pair[1])
        |> Option.defaultValue "docs/examples"

    Directory.CreateDirectory(outputDir) |> ignore

    DocsExamples.Examples.ProjectionSnapshotHost.run outputDir
    DocsExamples.Examples.ElmViewWriteBack.run outputDir
    DocsExamples.Examples.KeyedCollectionSync.run outputDir
    0
