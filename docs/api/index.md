# API reference

The generated reference combines XML documentation from the assemblies with
the longer explanations under `docs/api`.

## Find the layer first

| If you need to… | Start with… |
| --- | --- |
| host immutable snapshots without a UI framework | `Elmish.Glue.Core` |
| marshal Elmish updates onto Avalonia's UI thread | `Elmish.Avalonia.Glue` |
| expose an explicit CLR viewmodel | `Elmish.Avalonia.Glue.Projection` |
| bind through immutable F# view records | `Elmish.Avalonia.Glue.ElmView` |

The types are deliberately layered. Application code normally starts in an
Avalonia authoring package. Use Core directly when you are building an adapter
or need its collection and notification primitives.

## Follow a snapshot through the API

1. `ElmishHost` starts the program and supplies dispatch.
2. A root host receives each immutable snapshot.
3. `BindableSnapshotHost` raises root notifications.
4. Nested nodes refresh their exposed properties.
5. `WriteBackBindings` maps an editable value to a message.
6. Keyed collection helpers preserve identity where required.

Open a package or type below for exact signatures and member documentation.
