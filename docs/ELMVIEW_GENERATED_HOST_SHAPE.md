# ElmView Generated Host Shape

Task 12 defines the shape that ElmView V2 code generation should target.

## Root Host

The generated root host is the Avalonia `DataContext`. It owns:

- the current immutable root snapshot
- a dispatch function for Elmish messages
- generated root properties for scalar fields
- generated root properties that return nested bindable child nodes

The intended runtime base is `GeneratedViewHost<'View,'Msg>`.

Generated root properties follow this pattern:

- getters read from `this.View`
- setters do not mutate the snapshot
- writable setters dispatch explicit Elmish messages

## Nested Child Nodes

Nested records are exposed as stable child-node instances rather than recreated
objects. The intended runtime base is `GeneratedViewNode<'RootView,'NodeView,'Msg>`.

Generated child-node properties follow this pattern:

- getters read from the current immutable node snapshot
- setters dispatch messages through the root host
- the node instance stays stable while the underlying snapshot changes

That keeps normal AXAML paths such as `UserInput.Name` valid without requiring
the immutable F# records themselves to implement binding interfaces.

## Update Behavior

When the root host receives a new immutable snapshot:

- the host updates its stored root snapshot
- the host raises `PropertyChanged` for generated root properties
- each registered child node raises `PropertyChanged` for its generated
  properties

This keeps binding paths ordinary while the runtime remains snapshot-based and
identity-aware underneath.

## Generated Shape Sketch

```fsharp
type AppHost(initialView: AppView) as this =
    inherit RuntimeGeneratedViewHost<AppView, Msg>(initialView)

    let userInput =
        UserInputNode(this)

    override _.GeneratedPropertyNames =
        [ "HeaderTitle"; "UserInput" ]

    member this.HeaderTitle = this.View.HeaderTitle
    member _.UserInput = userInput

and UserInputNode(host: AppHost) as this =
    inherit GeneratedViewNode<AppView, FormView, Msg>(
        (fun () -> host.View),
        host.Dispatch,
        host.RegisterChildNode,
        (fun root -> root.UserInput),
        [ "Name"; "Email"; "Newsletter" ])

    member this.Name
        with get () = this.Snapshot.Name
        and set value = this.Dispatch(Msg.SetName value)
```

Task 12 only defines the host and node shape. Centralized mapping syntax and
sample conversion remain later tasks.
