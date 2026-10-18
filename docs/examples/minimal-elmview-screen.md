---
title: Minimal ElmView screen example
project: src/Elmish.Avalonia.Glue.ElmView/Elmish.Avalonia.Glue.ElmView.fsproj
---

# Minimal ElmView screen example

This example runs the ElmView two-way round trip against `RuntimeGeneratedViewHost`: a handwritten `Name` facade dispatches through a registered write-back route, `update` returns a new view record, and the host raises `PropertyChanged` for `View`, `Name`, and `Greeting`.

## Run the two-way edit

[Source file](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/tools/DocsExamples/Examples/MinimalElmViewScreen.fs)

```fsharp run
open System
open System.ComponentModel
open Elmish.Avalonia.Glue.ElmView

type View =
    { Name: string
      Greeting: string }

type Msg =
    | NameChanged of string

type Host(initialView: View) =
    inherit RuntimeGeneratedViewHost<View, Msg>(
        initialView,
        Action<WriteBackBindings<View, Msg>>(fun bindings ->
            bindings.ForPath<string>("Name").Dispatch(Func<string, Msg>(NameChanged)) |> ignore))

    override _.GeneratedPropertyNames = [ "Name"; "Greeting" ]

    member this.Name
        with get () = this.View.Name
        and set value = this.TryDispatchWriteBack("Name", value) |> ignore

    member this.Greeting = this.View.Greeting

let host = Host({ Name = "Ada"; Greeting = "Hello, Ada!" })
let changed = ResizeArray<string>()

(host :> INotifyPropertyChanged).PropertyChanged.Add(fun args ->
    changed.Add(args.PropertyName |> Option.ofObj |> Option.defaultValue ""))

host.SetDispatch(Action<Msg>(fun msg ->
    match msg with
    | NameChanged name ->
        host.Update(
            { host.View with
                Name = name
                Greeting = $"Hello, {name}!" })))

host.Name <- "Grace"

printfn "View.Name = %s" host.View.Name
printfn "View.Greeting = %s" host.View.Greeting
printfn "Host.Greeting = %s" host.Greeting
printfn "Notifications = %A" (Seq.toList changed)
```
