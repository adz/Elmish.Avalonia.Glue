---
title: Build your first screen
---

# Build your first screen

This quickstart establishes the contract before choosing Projection or ElmView:
an immutable snapshot flows into a stable host, and an edit flows back as a
message. Use the sample suites for a complete application.

## Prerequisites

- .NET 10 SDK
- an Avalonia application using F# and Elmish
- an AXAML view with a `DataContext`

## Define state and messages

Put the state transition in F#. This standalone block is compiled by
FsLiveDocs.

```fsharp isolated
type Model = { Name: string }
type Msg = NameChanged of string

let init = { Name = "" }

let update message model =
    match message with
    | NameChanged value -> { model with Name = value }
```

## Keep the binding ordinary

Write normal AXAML. `Mode=TwoWay` is the signal that a host property must send
an edit back to Elmish.

```xml
<TextBox Text="{Binding Name, Mode=TwoWay}" />
```

## Choose a host family

Choose **Projection** if `Name` should be a member on a named CLR viewmodel.
Use it when the viewmodel is a useful public contract, has commands, or adapts
identity-sensitive controls.

Choose **ElmView** if an immutable F# record should define `Name` and the CLR
host can be mechanical. Register each editable path once beside host creation.

## Connect the host

The host remains the Avalonia `DataContext` for its lifetime. On every Elmish
update, pass the new model or view snapshot to `host.Update`. Give the host the
dispatcher once with `host.SetDispatch`.

The Avalonia integration package supplies `ElmishHost.startAndBind` so these
two calls occur on the UI thread. See the generated [API reference](../api.html)
for its public signature.

## Verify design time

Construct the same host shape with a realistic sample snapshot for preview.
Do not boot an Elmish runtime in the designer. Continue with [design-time
data](start/preview-and-design.html).

## Continue

- [Choose Projection](understand/projection-family.html)
- [Choose ElmView](understand/elmview-family.html)
- [Inspect runnable examples](../examples/index.html)
