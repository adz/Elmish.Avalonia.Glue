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

A projection implements `IProjection<TModel>`. Its `Update` method receives the
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

The complete sample projections use the repository's observable base classes
and are compiled as part of the sample solution.

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

`SnapshotHost<T>` exposes one immutable value through `Current`. Bind directly
to `Current.Name` when most values need no adaptation.

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
