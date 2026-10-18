# Bind nested ElmView paths

Expose a stable node for each nested record so AXAML can bind through ordinary CLR properties while Elmish replaces immutable snapshots. The [node contract](../reference/elmview/nodes.html) describes notification and identity.

## Define a node

Generated-shaped hosts define nested node classes in this shape:

```csharp
public sealed class UserInputNode : GeneratedViewNode<AppView, FormView, Msg>
{
    private readonly AppHost _host;

    public UserInputNode(AppHost host)
        : base(
            () => host.View,           // Get root snapshot
            host.Dispatch,             // Dispatch message
            host.RegisterChildNode,    // Register for refresh
            view => view.UserInput,    // Pluck nested view
            new[] { nameof(Name) })    // Properties to notify
    {
        _host = host;
    }

    public string Name
    {
        get => Snapshot.Name;
        set => _host.TryDispatchWriteBack("UserInput.Name", value);
    }
}
```

`AppHost` must derive from `GeneratedViewHost<AppView, Msg>` (or its runtime variant); its public `RegisterChildNode` method registers this node during construction. `FormView` is the type of the `UserInput` field, not a second property name. The binding segment before `.Name` comes from the root host property:

```csharp
public UserInputNode UserInput => _userInput;
```

That property normally mirrors a root F# view-record field:

```fsharp isolated
type FormView = { Name: string }
type AppView = { UserInput: FormView }
```

Avalonia then resolves `<TextBox Text="{Binding UserInput.Name}" />` by reading
`AppHost.UserInput`, then `UserInputNode.Name`.

