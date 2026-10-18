# Snapshots

Snapshot base classes provide stable, bindable shells around immutable data.

## `BindableNode`

Base class for bindable objects with an `INotifyPropertyChanged` implementation.

### Members

| Member | Description |
| :--- | :--- |
| `PropertyChanged` | The standard `INotifyPropertyChanged` event. |
| `NotifyPropertyChanged(propertyName: string)` | Triggers the `PropertyChanged` event for the specified property. |

## `BindableSnapshotHost<'Snapshot>`

An abstract base class for top-level view hosts. It manages the current snapshot and handles update notifications.

### Members

| Member | Description |
| :--- | :--- |
| `Snapshot` | Returns the current immutable snapshot. |
| `Update(nextSnapshot: 'Snapshot)` | Updates the host with a new snapshot. For reference-type snapshots (including `null`), if `ReferenceEquals(current, next)` is false, it stores `next`, synchronously raises `PropertyChanged` for the constructor-supplied name, then calls `OnSnapshotUpdated(previous, next)`. Same-reference updates do nothing, even if that object was mutated. Exceptions from listeners prevent the hook from running; there is no rollback. |
| `OnSnapshotUpdated(previous, next)` | An overridable hook called after the snapshot has been updated and notifications have been raised. |

## `BindableSnapshotNode<'RootSnapshot, 'NodeSnapshot>`

An abstract base class for nested bindable objects. These nodes "pluck" a piece of the root snapshot and expose it to the UI.

### Members

| Member | Description |
| :--- | :--- |
| `Snapshot` | Returns the current node-specific snapshot, plucked from the root. |
| `RegisterChildNode(child: IBindableSnapshotNode)` | Registers a child node to be included in subtree refreshes. |
| `RefreshSubtree()` | Triggers `PropertyChanged` for all properties exposed by this node and recursively calls `RefreshSubtree` on all registered children. |

## `IBindableSnapshotNode`

An interface that allows parent and child nodes to communicate during a refresh cycle.

### Members

| Member | Description |
| :--- | :--- |
| `RefreshSubtree()` | Propagates a refresh signal down the node tree. |

## Implementation

- [SnapshotSubstrate.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Glue.Core/SnapshotSubstrate.fs)
