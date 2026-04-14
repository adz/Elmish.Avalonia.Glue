## Samples.ElmView

`Samples.ElmView` is the F#-first sample suite for this repository.

It demonstrates the `ElmView` architecture family:

- immutable F# view records are the authored UI schema
- AXAML binds to a small CLR host through normal compiled bindings
- design-time snapshots and runtime updates use the same `View` shape
- handwritten projection trees are not the default workflow

The current suite implements the same example matrix as
`Samples.Projection`:

- static layout / HTML-equivalent
- user input / forms
- random / dice
- HTTP
- time / clock
- files
- SVG-equivalent composition

### Design-Time Pattern

The sample's root host inherits `RuntimeViewHost<'View>` and starts from
`Core.App.getDesignView()`. That gives the AXAML view realistic preview data
before the runtime Elmish host is attached.

At runtime, `ElmishHost.startAndBind` updates that same host with live view
snapshots and connects the dispatch seam used by the view code-behind.

### When To Use This Suite As A Reference

Use these samples when you want to study:

- how far an Avalonia app can move UI shaping into F#
- how to keep AXAML normal without a handwritten projection tree
- how to preserve a strong preview story with immutable view snapshots
