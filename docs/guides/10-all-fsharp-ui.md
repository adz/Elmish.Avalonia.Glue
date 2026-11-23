# All-F# Avalonia UI

F# can own the model, Elmish update, view snapshots, host setup and even a custom
control. Keep ordinary AXAML for the visual tree and ordinary Avalonia bindings.

## Styled properties and attached properties

For AXAML property registration limitations with Avalonia 12.0.0, see [F# compatibility](../reference/avalonia/index.html#f-styled-and-attached-property-compatibility).

Instead, subclass a control and render directly from its `DataContext` when a
custom rendering surface is needed. For simple appearance changes, bind an
ordinary class: `Classes.highlight="{Binding Flag}"` and style `.highlight`
in AXAML. Neither requires a custom attached property.

## F# ElmView facade

The host is a stable CLR facade over the immutable view record, and F# writes it the same way C# does: a subclass of
`RuntimeGeneratedViewHost` with one property per bound field. A writable property's setter dispatches a message
instead of changing the view; the next view comes back through `Update` and notifies every name in
`GeneratedPropertyNames`.

```fsharp
open Elmish.Avalonia.Glue.ElmView

type View = { Name: string; Greeting: string }
type Msg = SetName of string

type AppHost(initial: View) =
    inherit RuntimeGeneratedViewHost<View, Msg>(initial)
    member this.Name with get () = this.View.Name and set (value: string) = this.Dispatch(SetName value)
    member this.Greeting = this.View.Greeting
    override _.GeneratedPropertyNames = [ "Name"; "Greeting" ]
```

AXAML binds it as usual: `{Binding Name, Mode=TwoWay}` and `{Binding Greeting}`. Connect it with
`ElmishHost.startAndBind program (Action<View>(host.Update)) (Action<Action<Msg>>(host.SetDispatch))`. The package's
headless tests bind a `TextBox` this way and check that typing dispatches and the new view flows back.

### Routes by path

To keep the message mapping in one place, register routes and let each setter look its path up:

With the `View` and `Msg` above:

```fsharp
open System
open Elmish.Avalonia.Glue.ElmView

let configure =
    Action<WriteBackBindings<View, Msg>>(fun bindings ->
        bindings.ForPath<string>("Name").Dispatch(Func<string, Msg>(SetName)) |> ignore)

type RoutedHost(initial: View) =
    inherit RuntimeGeneratedViewHost<View, Msg>(initial, configure)
    member this.Name with get () = this.View.Name and set (value: string) = this.TryDispatchWriteBack("Name", value) |> ignore
    member this.Greeting = this.View.Greeting
    override _.GeneratedPropertyNames = [ "Name"; "Greeting" ]
```

Use the facade's property path for nested fields, for example `"Profile.Name"`. Design-time hosts can use the same
view shape without starting Elmish.

Continue with the [ElmView guide](elmview.html) and the
[write-back example](../examples/elmview-write-back.html).
