---
title: ElmView write-back example
project: src/Elmish.Avalonia.Glue.ElmView/Elmish.Avalonia.Glue.ElmView.fsproj
---

# ElmView write-back example

This example shows `WriteBackBindings<'View,'Msg>` routing an edit through a handwritten generated-shaped host. Its flat CLR `Host.Name` facade uses the nested record route `UserInput.Name`; an AXAML binding to this host would be `{Binding Name, Mode=TwoWay}`, not `{Binding UserInput.Name}`. For a nested AXAML path, expose a stable `UserInput` node with its own `Name` property as in the [nodes reference](../api/elmview/nodes.html).

## Run one edit

[Source file](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/tools/DocsExamples/Examples/ElmViewWriteBack.fs)

```fsharp run
open System
open Elmish.Avalonia.Glue.ElmView

type View =
    { UserInput: UserInput }

and UserInput =
    { Name: string }

type Msg =
    | SetName of string

type Host(initialView: View) =
    inherit RuntimeGeneratedViewHost<View, Msg>(
        initialView,
        Action<WriteBackBindings<View, Msg>>(fun bindings ->
            bindings.ForPath<string>("UserInput.Name").Dispatch(Func<string, Msg>(SetName)) |> ignore))

    member this.Name
        with get () = this.View.UserInput.Name
        and set value = this.TryDispatchWriteBack("UserInput.Name", value) |> ignore

let host = Host({ UserInput = { Name = "Avery" } })
host.SetDispatch(Action<Msg>(fun msg ->
    match msg with
    | SetName name ->
        host.Update({ host.View with UserInput = { Name = name } })))

host.Name <- "Morgan"
printfn "View.UserInput.Name = %s" host.View.UserInput.Name
printfn "Host.Name = %s" host.Name
```
