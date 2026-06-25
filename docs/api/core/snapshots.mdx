---
sidebar_position: 3
---

# Snapshots

The snapshot substrate provides the base classes for creating stable, bindable shells around immutable data snapshots.

## `BindableNode`

The foundation for all bindable objects in the system. It provides a standard implementation of `INotifyPropertyChanged`.

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
| `Update(nextSnapshot: 'Snapshot)` | Updates the host with a new snapshot. If the new snapshot is different (by reference) from the current one, it raises `PropertyChanged` for the property name provided to the constructor and calls `OnSnapshotUpdated`. |
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

## Source

- [SnapshotSubstrate.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Glue.Core/SnapshotSubstrate.fs)
