# Design-time preview

A preview is useful only when it exercises the same binding paths as runtime.
Treat design data as part of the feature, not a screenshot fixture.

## Create realistic immutable data

Define a design snapshot in F# with useful lengths, empty states, validation
states, and representative collection rows.

```fsharp isolated
type ProfileView =
    { Name: string
      Email: string
      Validation: string }

let designProfile =
    { Name = "Ada Lovelace"
      Email = "ada@example.test"
      Validation = "Ready to save" }
```

FsLiveDocs compiles this example. The sample value needs no Elmish runtime.

## Construct the runtime-shaped host

Give the design host the snapshot in its constructor. For ElmView, use the
same root and node properties as the runtime host. For Projection, populate
the same projection properties runtime updates.

The preview must not use different binding paths, special controls, or a
second viewmodel hierarchy.

## Wire the designer DataContext

The [ElmView matrix AXAML](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/sample/Samples.ElmView/ExampleMatrixSample/src/ExampleMatrixSample.UI/Views/AppView.axaml) uses the following window child to instantiate the preview host without starting Elmish:

```xml
<Design.DataContext>
    <local:UserInputPreviewHost />
</Design.DataContext>
```

Declare `xmlns:local` for the UI views namespace and provide a public parameterless preview host whose constructor loads the design snapshot. Open `AppView.axaml` in the Avalonia previewer; the runtime `DataContext` is assigned separately at startup. Preview bindings must match runtime bindings.

## Preview states that expose layout problems

Include long text, validation messages, loading states, empty lists, and a few
rows. A single happy-path record rarely tests the layout you care about.

## Keep dispatch inactive

The designer should render without starting the full program. Commands may be inert. <a href="../api/Elmish.Avalonia.Glue.ElmView.GeneratedViewHost`2.html">`GeneratedViewHost`</a> starts with a no-op dispatch function, so registered write-back routes can be called without a running program; unmatched routes return `false`. [`BindableViewHost`](../reference/elmview/hosts.html#bindable-root) and <a href="../api/Elmish.Avalonia.Glue.Projection.SnapshotHost`1.html">`SnapshotHost`</a> have no built-in setters or dispatcher: handwritten setters/commands must supply their own safe no-op design-time behaviour. Other projection dispatch helpers may require `SetDispatch` before use.

The two sample suites link their design snapshots and hosts from
[sample applications](sample-applications.html).
