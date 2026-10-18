module DocsExamples.Examples.MinimalProjectionScreen

open System
open System.ComponentModel
open System.IO
open Elmish.Glue.Core

type Model =
    { Name: string
      Greeting: string }

type Msg =
    | NameChanged of string

let initial =
    { Name = "Ada"; Greeting = "Hello, Ada!" }

let update msg model =
    match msg with
    | NameChanged name ->
        { model with
            Name = name
            Greeting = $"Hello, {name}!" }

type GreetingProjection() =
    inherit ProjectionNode<Model, Msg>()

    let mutable name = ""
    let mutable greeting = ""
    let mutable updating = false

    member this.Name
        with get () = name
        and set value =
            if this.Change(&name, value, nameof this.Name) && not updating then
                this.Dispatch.Invoke(NameChanged value)

    member this.Greeting = greeting

    override this.Update(model) =
        updating <- true
        this.Change(&name, model.Name, nameof this.Name) |> ignore
        this.Change(&greeting, model.Greeting, nameof this.Greeting) |> ignore
        updating <- false

let run outputDir =
    let projection = GreetingProjection()
    let mutable model = initial
    let changed = ResizeArray<string>()

    (projection :> INotifyPropertyChanged).PropertyChanged.Add(fun args ->
        changed.Add(args.PropertyName |> Option.ofObj |> Option.defaultValue ""))

    projection.Update(model)

    (projection :> IDispatchTarget<Msg>).SetDispatch(Action<Msg>(fun msg ->
        let next = update msg model
        model <- next
        projection.Update(next)))

    projection.Name <- "Grace"

    let sourceUrl =
        "https://github.com/adz/Elmish.Avalonia.Glue/blob/main/tools/DocsExamples/Examples/MinimalProjectionScreen.fs"

    let source =
        """
open System
open System.ComponentModel
open Elmish.Glue.Core

type Model =
    { Name: string
      Greeting: string }

type Msg =
    | NameChanged of string

let initial =
    { Name = "Ada"; Greeting = "Hello, Ada!" }

let update msg model =
    match msg with
    | NameChanged name ->
        { model with
            Name = name
            Greeting = $"Hello, {name}!" }

type GreetingProjection() =
    inherit ProjectionNode<Model, Msg>()

    let mutable name = ""
    let mutable greeting = ""
    let mutable updating = false

    member this.Name
        with get () = name
        and set value =
            if this.Change(&name, value, nameof this.Name) && not updating then
                this.Dispatch.Invoke(NameChanged value)

    member this.Greeting = greeting

    override this.Update(model) =
        updating <- true
        this.Change(&name, model.Name, nameof this.Name) |> ignore
        this.Change(&greeting, model.Greeting, nameof this.Greeting) |> ignore
        updating <- false

let projection = GreetingProjection()
let mutable model = initial
let changed = ResizeArray<string>()

(projection :> INotifyPropertyChanged).PropertyChanged.Add(fun args ->
    changed.Add(args.PropertyName |> Option.ofObj |> Option.defaultValue ""))

projection.Update(model)

(projection :> IDispatchTarget<Msg>).SetDispatch(Action<Msg>(fun msg ->
    let next = update msg model
    model <- next
    projection.Update(next)))

projection.Name <- "Grace"

printfn "Model.Name = %s" model.Name
printfn "Model.Greeting = %s" model.Greeting
printfn "Projection.Greeting = %s" projection.Greeting
printfn "Notifications = %A" (Seq.toList changed)
"""

    let output =
        [ $"Model.Name = {model.Name}"
          $"Model.Greeting = {model.Greeting}"
          $"Projection.Greeting = {projection.Greeting}"
          $"Notifications = {Seq.toList changed}" ]
        |> String.concat Environment.NewLine

    File.WriteAllText(Path.Combine(outputDir, "minimal-projection-screen.md"), $"""---
title: Minimal Projection screen example
project: src/Elmish.Glue.Core/Elmish.Glue.Core.fsproj
---

# Minimal Projection screen example

This example runs the Projection two-way round trip against `ProjectionNode`: a writable `Name` setter dispatches `NameChanged`, the update returns a new model, and the projection raises `PropertyChanged` for both the edit and the applied snapshot.

## Run the two-way edit

[Source file]({sourceUrl})

```fsharp run
{source.Trim()}
```
""")
