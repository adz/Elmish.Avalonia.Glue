---
sidebar_position: 4
---

# Shared substrate

`Elmish.Glue.Core` holds the concepts that do not belong specifically to
Avalonia styling, AXAML loading, or one authoring family.

Its job is to bridge immutable Elmish snapshots and stable .NET objects that
can raise change notifications.

## The problem: Stability vs. Immutability

Avalonia expects stable object identities that notify when properties change.
Elmish works by producing new immutable state snapshots.

If we simply replaced the `DataContext` with a new record on every update:

1. Existing bindings would often break or require expensive re-evaluation.
2. Control state (like scroll position, focus, or selection) might be lost.
3. Animations and transitions would be interrupted.

The shared substrate solves this with stable bindable shells over changing
immutable snapshots.

The shell remains the object Avalonia knows. The snapshot is the latest value
from Elmish.

## Core concepts

### BindableNode

The base class for anything that needs to notify the UI. It implements
`INotifyPropertyChanged` and provides a protected notification method.

### BindableSnapshotHost

The stable shell for a top-level view. It holds the current `Snapshot` and
notifies the UI when `Update` receives a new one.

- Stability: the host instance remains the same for the lifetime of the view.
- Identity: unchanged snapshot references can skip redundant refresh work.

### BindableSnapshotNode

Nested AXAML paths often need nested bindable objects. `BindableSnapshotNode`
represents a stable object for one part of the immutable snapshot tree.

- Projection: each node knows how to read its part of the root snapshot.
- Propagation: root updates refresh the node graph and raise property changes.

### KeyedSnapshotCollection

Lists are especially sensitive to identity. Replacing a whole list can force
Avalonia to discard row containers and recreate them.

`KeyedSnapshotCollection` patches an `ObservableCollection` from an immutable
list:

- It uses keys to identify which items were added, removed, or moved.
- It updates existing items in place when keys are already present.
- It preserves stable UI containers for selection and virtualization.

## Why This Is In Core

These pieces are standard .NET concepts: snapshots, property change
notification, dispatch storage, and keyed collection patching.

Avalonia-specific concerns stay in the higher packages. Core grows only for
framework-neutral concepts.

## Source

- [Snapshot substrate](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Glue.Core/SnapshotSubstrate.fs)
- [Collection patching](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Glue.Core/ObservableCollectionExtensions.fs)
- [Projection contracts](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Glue.Core/Projections.fs)
