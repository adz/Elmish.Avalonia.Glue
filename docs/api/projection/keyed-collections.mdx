---
sidebar_position: 3
---

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

## How it works

1. **Inheritance**: It inherits from `Elmish.Glue.Core.KeyedSnapshotCollection`.
2. **Keying**: You provide a key selector function in the constructor. This key is used to determine which items in the collection correspond to which items in the new snapshot list.
3. **Patching**: When `Update` is called, it uses the core `KeyedCollectionPatching` algorithm to:
    - **Add** new snapshots to the collection.
    - **Remove** snapshots that are no longer present.
    - **Move** existing snapshots to their new positions.
    - **Update** items in place if they are already present (by reference or value).

## Why use it in a Projection?

For Projection-style viewmodels that display lists, `KeyedSnapshotCollection`
keeps row identity stable:

- Stable rows: Avalonia can keep the same UI containers for retained items.
- Selection preservation: selected rows remain selected while their data changes.
- Smoother updates: retained rows are patched instead of replacing the whole collection.

## Source

- [SnapshotHosts.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Avalonia.Glue.Projection/SnapshotHosts.fs)
- [KeyedCollectionPatching.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Glue.Core/KeyedCollectionPatching.fs)
