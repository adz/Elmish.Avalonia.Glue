---
title: What this library changes
---

# What this library changes

Elmish.Avalonia.Glue is for an Avalonia application that wants immutable F#
state without giving up normal desktop UI authoring.

## Keep the parts of Avalonia that already work

Keep `.axaml`, binding paths, `Mode=TwoWay`, compiled bindings, preview data,
and DevTools. A control still sees an ordinary CLR `DataContext`.

```xml
<TextBox Text="{Binding UserInput.Name, Mode=TwoWay}" />
<CheckBox IsChecked="{Binding UserInput.Newsletter, Mode=TwoWay}" />
```

## Move state transitions into F#

Your model is immutable. An event becomes a message, and `update` returns the
next model. The following standalone example is valid F# and is representative
of the state layer this library connects to Avalonia.

```fsharp isolated
type Model = { Name: string; Newsletter: bool }

type Msg = SetName of string | SetNewsletter of bool

let update message model =
    match message with
    | SetName name -> { model with Name = name }
    | SetNewsletter enabled -> { model with Newsletter = enabled }
```

## Add one deliberately boring bridge

The bridge publishes properties, reads the latest immutable snapshot, raises
change notifications, and dispatches a message when Avalonia writes a
`TwoWay` property. It owns no product state and does not interpret your UI.

Choose **Projection** for an explicit CLR contract. Choose **ElmView** when
the F# view record is the contract and the CLR host is mechanical.

Continue with [the quickstart](guides/start.html).
