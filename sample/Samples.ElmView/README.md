## Samples.ElmView

`Samples.ElmView` is the F#-first sample suite for this repository.

It is the sample suite closest to Elmish authoring: plain immutable F# records
shape the screen while AXAML remains the view format.

It demonstrates the `ElmView` architecture family:

- immutable F# view records are the authored UI schema
- AXAML binds through generated CLR-facing root and child nodes with
  binding paths
- editable controls use Avalonia `Mode=TwoWay` bindings
- write-back mapping is centralized in the host instead of encoded as F#
  binding metadata
- design-time snapshots and runtime updates use the same generated host shape
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

The sample's root host inherits `RuntimeGeneratedViewHost<'View,'Msg>` and
starts from `Core.App.getDesignView()`. That gives the AXAML view realistic
preview data before the runtime Elmish host is attached.

The host registers write-back routes once, near construction, with mappings
such as `bindings.For(x => x.UserInput.Name).Dispatch(Msg.NewSetName)`.

At runtime, `ElmishHost.startAndBind` updates that same host with live view
snapshots. Generated writable properties dispatch through those registered
routes, while button-like actions still use the small imperative seams the
view exposes.

Preview and runtime share the same bindable shape. The design snapshot changes
the data, not the binding model.

### When To Use This Suite As A Reference

Use these samples when you want to study:

- how far an Avalonia app can move UI shaping into F#
- how to keep AXAML standard without a handwritten projection tree or F# binding
  annotations
- how to preserve a strong preview story with immutable view snapshots
