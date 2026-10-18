# All-F# Avalonia UI

F# can own the model, Elmish update, view snapshots, host setup and even a custom
control. Keep ordinary AXAML for the visual tree and ordinary Avalonia bindings.

## Styled properties and attached properties

Do not declare Avalonia styled or attached properties in F# when they must be
resolved from AXAML. Avalonia's XAML compiler expects public static fields;
F# exposes compiled values as properties instead, causing `AVLN3000`.

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
`"Profile.Name"`). The generated setter dispatches a message rather than
mutating the immutable snapshot. Design-time hosts can use the same view shape
without starting Elmish.

Continue with the [ElmView guide](elmview.html) and the
[write-back example](../examples/elmview-write-back.html).
