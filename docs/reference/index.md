# API reference

The generated reference combines XML documentation from the assemblies with
the longer explanations under `docs/reference`.

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

1. <a href="/api/Elmish.Avalonia.Glue.ElmishHost.html">`Elmish.Avalonia.Glue.ElmishHost`</a> starts the program and supplies dispatch.
2. A root host receives each immutable snapshot.
3. <a href="/api/Elmish.Glue.Core.BindableSnapshotHost`1.html">`BindableSnapshotHost`</a> raises root notifications.
4. Nested nodes refresh their exposed properties.
5. <a href="/api/Elmish.Avalonia.Glue.ElmView.WriteBackBindings`2.html">`WriteBackBindings`</a> maps an editable value to a message.
6. Keyed collection helpers preserve identity where required; see [collection patching](core/collections.html).

Open a package or type below for exact signatures and member documentation.

## Package boundaries

| Package | Responsibility |
| --- | --- |
| `Elmish.Glue.Core` | framework-neutral hosts, notifications, dispatch, and keyed patching |
| `Elmish.Avalonia.Glue` | Avalonia UI-thread delivery and compatibility surface |
| `Elmish.Avalonia.Glue.Projection` | explicit projection and snapshot-host helpers |
| `Elmish.Avalonia.Glue.ElmView` | view-record hosts, nodes, and write-back routing |

