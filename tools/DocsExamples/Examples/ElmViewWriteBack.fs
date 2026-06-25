module DocsExamples.Examples.ElmViewWriteBack

open System
open System.IO
open Elmish.Avalonia.Glue.ElmView

type View =
    { UserInput: UserInput }

and UserInput =
    { Name: string }

type Msg =
    | SetName of string

type Host(initialView: View) =
    inherit RuntimeGeneratedViewHost<View, Msg>(initialView, Action<WriteBackBindings<View, Msg>>(fun bindings ->
        bindings.For(fun (x: View) -> x.UserInput.Name).Dispatch(Func<string, Msg>(SetName)) |> ignore))

    member this.Name
        with get () = this.View.UserInput.Name
        and set value = this.TryDispatchWriteBack("UserInput.Name", value) |> ignore

let run outputDir =
    let host = Host({ UserInput = { Name = "Avery" } })
    host.SetDispatch(Action<Msg>(fun msg ->
        match msg with
        | SetName name ->
            host.Update(
                { host.View with
                    UserInput = { host.View.UserInput with Name = name } })))
    host.Name <- "Morgan"

    let sourceUrl =
        "https://github.com/adz/Elmish.Avalonia.Glue/blob/main/tools/DocsExamples/Examples/ElmViewWriteBack.fs"

    let source =
        """
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
            bindings.For(fun (x: View) -> x.UserInput.Name).Dispatch(Func<string, Msg>(SetName)) |> ignore))
"""

    let output =
        [ $"View.UserInput.Name = {host.View.UserInput.Name}"
          $"Host.Name = {host.Name}" ]
        |> String.concat Environment.NewLine

    File.WriteAllText(Path.Combine(outputDir, "elmview-write-back.md"), $"""---
sidebar_position: 3
title: ElmView write-back example
---

# ElmView write-back example

This example shows `WriteBackBindings<'View,'Msg>` routing an edit through a generated host.

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
