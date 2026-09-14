# Get started

This tutorial builds the mental model for one editable field. It does not ask
you to design a whole application first.

## Start with an immutable transition

Model the value and the user intent in F#:

```fsharp isolated
type Model = { Name: string }
type Msg = NameChanged of string

let initial = { Name = "Ada" }

let update msg model =
    match msg with
    | NameChanged name -> { model with Name = name }

let changed = update (NameChanged "Grace") initial
assert (changed.Name = "Grace")
assert (initial.Name = "Ada")
```

FsLiveDocs compiles this block. The assertions make the important property
visible: an edit creates a new value; it does not mutate `initial`.

## Define the binding contract

Write the Avalonia view as you normally would:

```xml
<StackPanel Spacing="8">
  <TextBlock Text="Name" />
  <TextBox Text="{Binding Name, Mode=TwoWay}" />
</StackPanel>
```

The binding path creates a two-part contract. The `DataContext` needs a
readable and writable `Name` property, and that property must notify Avalonia
when the snapshot changes.

## Put a stable object between the two systems

Do not replace the window's `DataContext` after every message. Keep one host
instance and update the immutable value behind it.

The host getter returns the current snapshot value. Its setter dispatches
`NameChanged`. When Elmish returns the next snapshot, `host.Update` raises
`PropertyChanged` for `Name`.

## Choose the host shape

With **Projection**, you write a named viewmodel with `Name`, `Update`, and a
dispatch connection. This is explicit and familiar to XAML tooling.

With **ElmView**, the F# view record contains `Name`. A generated-shaped host
exposes that field and maps the writable path to `NameChanged`.

Continue with [the architecture](architecture.html), then choose the guide
for the style you want to try first.
