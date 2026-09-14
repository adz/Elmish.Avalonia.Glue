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

This block is compiled by FsLiveDocs. The view function is pure, so runtime
and preview can use the same shape.

## Bind to ordinary properties

AXAML remains standard:

```xml
<TextBox Text="{Binding Profile.Name, Mode=TwoWay}" />
<TextBox Text="{Binding Profile.Email, Mode=TwoWay}" />
<TextBlock Text="{Binding Profile.Validation}" />
<Button IsEnabled="{Binding Profile.CanSave}" Content="Save" />
```

The root host exposes `Profile`. A stable profile node exposes the four fields
from the latest `ProfileView` snapshot.

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

A node getter reads `Snapshot.Name`. Its setter calls
`TryDispatchWriteBack("Profile.Name", value)`. The setter never mutates the
snapshot.

See the compiled [ElmView write-back example](../examples/elmview-write-back.html)
and the complete sample host linked from [sample applications](sample-applications.html).
