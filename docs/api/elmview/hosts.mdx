---
sidebar_position: 2
---

# Generated hosts

The `GeneratedViewHost` classes provide the bindable root for ElmView. They
bridge immutable F# view records and the Avalonia `DataContext`.

## `GeneratedViewHost<'View, 'Msg>`

The abstract base class for all ElmView root hosts.

### Members

| Member | Description |
| :--- | :--- |
| `View` | Returns the current immutable view record snapshot. |
| `SetDispatch(dispatcher)` | Connects the host to the Elmish loop. |
| `Dispatch(message)` | Dispatches a message to the Elmish loop. |
| `WriteBackBindings` | Returns the `WriteBackBindings` collection for this host. |
| `TryDispatchWriteBack(path, value)` | Attempts to dispatch a message for a property change at the specified path. This is used by generated property setters. |
| `GeneratedPropertyNames` | Returns the property names exposed by the host. These properties are included in `PropertyChanged` notifications during refresh. |

## Host Variants

ElmView provides host types for runtime and design-time while preserving one
bindable shape.

### `RuntimeGeneratedViewHost<'View, 'Msg>`

Used when the application is running. It is started and bound through
`ElmishHost.startAndBind`.

### `DesignGeneratedViewHost<'View, 'Msg>`

Used for Avalonia design-time preview. It is initialized with a sample
snapshot so the designer can render the same bindable shape used at runtime.

## How it works

1. **Initialization**: The host is constructed with an initial snapshot and an optional configuration action for `WriteBackBindings`.
2. **Binding**: In AXAML, the host is set as the `DataContext`.
3. **Update**: When the Elmish program produces a new view snapshot, `host.Update(newSnapshot)` is called.
4. **Refresh**: The host raises `PropertyChanged` for its `View` property and all names in `GeneratedPropertyNames`, then calls `RefreshSubtree()` on all nested nodes.

## Source

- [ElmViewHosts.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Avalonia.Glue.ElmView/ElmViewHosts.fs)
