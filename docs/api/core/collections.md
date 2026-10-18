# Collections

Collection utilities sync mutable `ObservableCollection`s with immutable lists while preserving item identity.

## `ObservableCollectionExtensions`

These extensions provide the primary way to sync collections in a Projection-style viewmodel.

### `SyncWith`

Synchronizes an `ObservableCollection<'ViewModel>` with an `IReadOnlyList<'Model>`.
F# `list<'Model>` and `seq<'Model>` overloads copy the input into an array
(O(n) time and space) before patching; arrays and `ResizeArray` can use the
`IReadOnlyList` overload without that copy.

| Parameter | Description |
| :--- | :--- |
| `models` | The new list of model snapshots. |
| `modelKey` | A function to extract a unique key from a model. |
| `vmKey` | A function to extract a unique key from a viewmodel. |
| `create` | A function to create a new viewmodel from a model. |
| `update` | An action to update an existing viewmodel with a new model snapshot. |

**Behavior:**
- **Matches** existing viewmodels to models using the provided keys.
- **Updates** matched viewmodels in place using the `update` action.
- **Inserts** new viewmodels using the `create` function.
- **Removes** viewmodels that no longer have a corresponding model.
- **Moves** viewmodels to match the order of the models.

## `KeyedSnapshotCollection<'T, 'Key>`

A helper class around an `ObservableCollection` with an `Update` method. Both
Projection and ElmView use it for lists of snapshots.

### Members

| Member | Description |
| :--- | :--- |
| `Items` | Returns the underlying `ObservableCollection<'T>`. |
| `Update(next: IReadOnlyList<'T>)` | Patches the collection to match the new list of snapshots. |

## Identity preserved by patching

By patching the collection instead of replacing it:
1. **Container Reuse**: Avalonia can reuse the existing `ListBoxItem` or `DataGridRow` containers.
2. **Selection Stability**: Selected items remain selected if they are still present in the list.
3. **Smooth Transitions**: List animations and layout transitions work correctly.

## Implementations

- [ObservableCollectionExtensions.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Glue.Core/ObservableCollectionExtensions.fs)
- [KeyedCollectionPatching.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Glue.Core/KeyedCollectionPatching.fs)
- [KeyedSnapshotCollection source](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Glue.Core/SnapshotSubstrate.fs)
