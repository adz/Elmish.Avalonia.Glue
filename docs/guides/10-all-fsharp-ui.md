# All-F# Avalonia UI

F# can own the model, Elmish update, view snapshots, host setup and even a custom
control. Keep ordinary AXAML for the visual tree and ordinary Avalonia bindings.

## Styled properties and attached properties

For AXAML property registration limitations with Avalonia 12.0.0, see [F# compatibility](../api/avalonia/index.html#f-styled-and-attached-property-compatibility).

Instead, subclass a control and render directly from its `DataContext` when a
custom rendering surface is needed. For simple appearance changes, bind an
ordinary class: `Classes.highlight="{Binding Flag}"` and style `.highlight`
in AXAML. Neither requires a custom attached property.

## F# ElmView write-back

An F# host can register writable paths without C# expression-tree selectors:

```fsharp
open System
open Elmish.Avalonia.Glue.ElmView

type View = { Name: string }
type Msg = SetName of string

let configure =
    Action<WriteBackBindings<View, Msg>>(fun bindings ->
        bindings.ForPath<string>("Name").Dispatch(Func<string, Msg>(SetName)) |> ignore)

let host = RuntimeGeneratedViewHost<View, Msg>({ Name = "" }, configure)
```

Use the exact generated facade property path (for nested fields, for example
`"Profile.Name"`). The snippet registers a route but does **not** expose a bindable `Name` facade: `RuntimeGeneratedViewHost` does not generate CLR properties. To bind `{Binding Name, Mode=TwoWay}`, subclass it and add a public `Name` getter reading `View.Name` and setter calling `TryDispatchWriteBack("Name", value)`, and override `GeneratedPropertyNames` to include `Name`. The handwritten setter dispatches rather than mutating the snapshot. Design-time hosts can use the same view shape
without starting Elmish.

Continue with the [ElmView guide](elmview.html) and the
[write-back example](../examples/elmview-write-back.html).
