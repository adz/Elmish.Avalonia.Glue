# Generated-shaped nodes

`GeneratedViewNode<'RootView, 'NodeView, 'Msg>` is the base class for nested
bindable objects in an ElmView tree. Avalonia binds to deep paths in immutable
F# view records and still receives change notifications.

Despite the type name, this package does not currently ship a generator. The
sample nodes are handwritten in a regular, mechanical shape that a future
generator could emit.

## `GeneratedViewNode<'RootView, 'NodeView, 'Msg>`

### Members

| Member | Description |
| :--- | :--- |
| `Snapshot` | Returns the current immutable view record for *this specific node*, plucked from the root view. |
| `Dispatch(message)` | Dispatches a message to the Elmish loop through the root host dispatcher. |

## Stable nested binding paths

Traditional MVVM often uses deep trees of mutable viewmodel objects. ElmView
uses a tree of records behind generated bindable nodes.

`GeneratedViewNode` acts as a stable proxy for a specific record in that tree:

1. **Stability**: The node instance itself is stable. If you have a `UserInputNode`, it remains the same object even as the `UserInput` record within the snapshot is replaced.
2. **Plucking**: The node is configured with a function that knows how to find its specific record within the root snapshot (e.g., `view => view.UserInput`).
3. **Notification**: When the root host receives a new snapshot, it triggers a refresh. Each node then raises `PropertyChanged` for the specific properties it exposes, allowing Avalonia to update the UI.

See the [nested-path recipe](../../guides/nested-elmview-paths.html) for the C#, F# and AXAML example.

## Identity and notification guarantees

- Deep bindings: supports XAML binding paths like `<TextBox Text="{Binding UserInput.Name}" />`.
- Identity preservation: keeps stable bindable nodes without hand-authored projection trees.
- Refresh propagation: notifications move from the root to child nodes.

## Implementation

- [ElmViewHosts.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Avalonia.Glue.ElmView/ElmViewHosts.fs)
