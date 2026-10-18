# Keyed collections

`KeyedSnapshotCollection<'T, 'Key>` in the Projection package specializes the
core collection patching logic. It manages immutable snapshot lists while
preserving stable UI identity.

## `KeyedSnapshotCollection<'T, 'Key>`

### Members

| Member | Description |
| :--- | :--- |
| `Items` | Returns the underlying `ObservableCollection<'T>`. Binds in XAML to `Items`. |
| `Update(next)` | Synchronizes the collection with the provided list of snapshots. |

## Patch sequence

1. **Inheritance**: It inherits from `Elmish.Glue.Core.KeyedSnapshotCollection`.
2. **Keying**: You provide a key selector function in the constructor. This key is used to determine which items in the collection correspond to which items in the new snapshot list.
3. **Patching**: When `Update` is called, it uses the core `KeyedCollectionPatching` algorithm to:
    - **Add** new snapshots to the collection.
    - **Remove** snapshots that are no longer present.
    - **Move** existing snapshots to their new positions.
    - **Retain** an item only when the incoming snapshot is the same reference; otherwise replace the collection item for that key.

## Selection and container stability

`KeyedSnapshotCollection` keeps the collection instance stable, **not** each row object. For stable mutable row identity with changing data, use `ObservableCollectionExtensions.SyncWith` (or `ProjectionExtensions.SyncWith`) with a row projection and update it in place. Selection and container reuse depend on Avalonia's control and item identity; matching keys alone do not guarantee either. See [collection identity](../core/collections.html).

## Implementations

- [SnapshotHosts.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Avalonia.Glue.Projection/SnapshotHosts.fs)
- [KeyedCollectionPatching.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Glue.Core/KeyedCollectionPatching.fs)
