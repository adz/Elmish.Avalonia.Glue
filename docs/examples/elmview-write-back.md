---
sidebar_position: 3
title: ElmView write-back example
---

# ElmView write-back example

This example shows `WriteBackBindings<'View,'Msg>` routing an edit through a generated host.

## Source

[Source file](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/tools/DocsExamples/Examples/ElmViewWriteBack.fs)

```fsharp
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
```

## Observed output

```text
View.UserInput.Name = Morgan
Host.Name = Morgan
```
