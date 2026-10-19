# Projection

Projection gives Avalonia an explicit CLR viewmodel. Elmish still owns state;
the projection copies the latest values into a stable binding surface.

## Choose Projection for an intentional CLR contract

Projection works well when the viewmodel is useful in its own right:

- a screen has many commands or derived properties;
- C# developers own the AXAML-facing contract;
- a control needs stable mutable rows;
- you want every binding path visible in one named type.

Do not choose it merely because Avalonia uses bindings. ElmView also exposes
normal binding paths with less handwritten projection code.

## Project a model into properties

A projection implements <a href="../api/Elmish.Glue.Core.IProjection`1.html">`IProjection<TModel>`</a>. Its `Update` method receives the
latest immutable model and changes only the CLR-facing properties.

```csharp
public sealed class ProfileProjection : IProjection<ProfileModel>
{
    public string Name { get; private set; } = "";

    public void Update(ProfileModel model)
    {
        if (Name == model.Name) return;
        Name = model.Name;
        PropertyChanged?.Invoke(this,
            new PropertyChangedEventArgs(nameof(Name)));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
```

This snippet illustrates the read-only update side only: `private set` cannot accept a `TwoWay` write. For editing, add a public setter that dispatches `NameChanged` without changing the model, and connect dispatch at startup. See [sample projections](sample-applications.html) for the complete binding surface.

## Send user intent back to Elmish

A writable projection property dispatches a message. It does not commit the
new value as application state.

```text
TextBox writes Name
→ projection dispatches NameChanged
→ Elmish update returns the next model
→ projection.Update publishes the accepted value
```

Guard model-driven updates from dispatch. Otherwise an incoming model can look
like another user edit and create a feedback loop.

## Prefer a snapshot host for shallow screens

<a href="../api/Elmish.Avalonia.Glue.Projection.SnapshotHost`1.html">`SnapshotHost<T>`</a> exposes one immutable value through `Current`. Bind directly
to `Current.Name` for read-only values when most values need no adaptation. An immutable record field under `Current` cannot accept a `TwoWay` write: expose a separate writable host property that dispatches instead.

Add named commands or derived CLR properties only where they improve the
screen contract. This avoids a deep mutable projection tree.

See the runnable [snapshot-host example](../examples/projection-snapshot-host.html).

## Use row projections when identity matters

For `DataGrid`, selection, inline editing, or virtualization, keep a stable row
projection per key. Patch the collection as new immutable rows arrive.

The [keyed collections guide](keyed-collections.html) explains this boundary.

## Keep responsibilities separated

Put validation and state transitions in F#. Put binding names, commands, and
framework adaptation in the projection. Put layout and control behaviour in
AXAML.

If most projection members only repeat an F# record field, try
[ElmView](elmview.html).
