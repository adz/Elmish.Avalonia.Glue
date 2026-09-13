---
title: API reference guide
---

# API reference guide

FsLiveDocs generates the member-level reference from the public assemblies.
The pages in this directory are the map: read them first to understand why a
type exists, then follow their links into the generated API for signatures.

## Read the reference by responsibility

| Package | Start here when you need | Main types |
| --- | --- | --- |
| `Elmish.Glue.Core` | framework-neutral notification, dispatch, or patching | `BindableSnapshotHost`, `BindableSnapshotNode`, `ElmishHost`, `KeyedSnapshotCollection` |
| `Elmish.Avalonia.Glue` | UI-thread binding to an Avalonia application | `ElmishHost` and compatibility aliases |
| `Elmish.Avalonia.Glue.Projection` | explicit CLR hosts or identity-aware lists | `SnapshotHost`, `KeyedSnapshotCollection` |
| `Elmish.Avalonia.Glue.ElmView` | F# record-backed hosts and editable paths | `GeneratedViewHost`, `GeneratedViewNode`, `WriteBackBindings` |

## Follow the lifecycle

1. Start an Elmish program through the Avalonia host bridge.
2. Keep one stable host as the view `DataContext`.
3. Send every new snapshot to `Update`.
4. Let the host notify the bound properties and nested nodes.
5. For editable ElmView paths, map a setter value to one message.
6. For collections that must retain UI identity, patch by key.

## Use the generated member reference

The generated [API member index](../api.html) is exhaustive. Its package and
namespace pages carry the narrative from the authored `docs/api` Markdown,
then link to every public member. Start there when a type name alone does not
explain where it belongs.
