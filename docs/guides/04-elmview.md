# ElmView

ElmView makes an immutable F# record the screen schema. Avalonia binds through
a stable CLR host whose code follows that record mechanically.

## Choose ElmView when F# should describe the screen

ElmView works well when:

- displayed values are derived naturally in F#;
- most bindings are simple record fields;
- editable fields map cleanly to Elmish messages;
- you want the review surface to be F# plus AXAML.

Use Projection when the CLR viewmodel carries meaningful behaviour rather than
mechanical binding glue.

## Derive a view record

Keep the application model focused on domain state. Derive a record containing
exactly what the screen needs:

```fsharp isolated
type Profile = { Name: string; Email: string }

type ProfileView =
    { Name: string
      Email: string
      Validation: string
      CanSave: bool }

let view profile =
    let valid = profile.Name <> "" && profile.Email.Contains "@"
    { Name = profile.Name
      Email = profile.Email
      Validation = if valid then "Ready to save" else "Complete both fields"
      CanSave = valid }
```

FsLiveDocs compiles this block. The view function is pure, so runtime
and preview can use the same shape.

## Bind to ordinary properties

AXAML remains standard:

```xml
<TextBox Text="{Binding Profile.Name, Mode=TwoWay}" />
<TextBox Text="{Binding Profile.Email, Mode=TwoWay}" />
<TextBlock Text="{Binding Profile.Validation}" />
<Button IsEnabled="{Binding Profile.CanSave}" Content="Save" />
```

Here `Profile` is a root CLR property returning a stable node for the `ProfileView` record; it is not declared by the snippet above. The node exposes the four fields from the latest snapshot. See the [node contract](../api/elmview/nodes.html) for the constructor and root property.

## Register editable paths once

Map each `TwoWay` property to the message that represents the edit:

```csharp
bindings.For(x => x.Profile.Name).Dispatch(Msg.NewNameChanged);
bindings.For(x => x.Profile.Email).Dispatch(Msg.NewEmailChanged);
```

`Validation` and `CanSave` have no route because Avalonia only reads them.

## Understand the generated-shaped host

The package supplies `GeneratedViewHost` and `GeneratedViewNode` base classes.
The current sample writes the mechanical host explicitly; the package does not
yet emit it from the record automatically.

A node getter reads `Snapshot.Name`. Its setter calls the root host's `TryDispatchWriteBack("Profile.Name", value)` (the node base does not expose this method). The setter never mutates the snapshot. An unmatched path returns `false` and dispatches nothing; duplicate routes throw at registration. See [write-back routing](../api/elmview/write-back.html).

See the compiled [ElmView write-back example](../examples/elmview-write-back.html)
and the complete sample host linked from [sample applications](sample-applications.html).
