# Collections

Collection utilities sync mutable `ObservableCollection`s with immutable lists while preserving item identity.

## `ObservableCollectionExtensions`

These extensions provide the primary way to sync collections in a Projection-style viewmodel.

### `SyncWith`

Synchronizes an `ObservableCollection<'ViewModel>` with an `IReadOnlyList<'Model>`.
F# `list<'Model>` and `seq<'Model>` overloads copy the input into an array
(O(n) time and space) before patching; arrays and `ResizeArray` can use the
`IReadOnlyList` overload without that input copy (cast to `IReadOnlyList` if overload resolution is ambiguous). The patch itself allocates key/index tables and may shift indexes repeatedly on moves/inserts; O(n) describes only the input conversion, not the total patch.

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

Patching keeps the `ObservableCollection` instance. `SyncWith` retains the same row viewmodel for matching keys and calls `update` on it. `KeyedSnapshotCollection` instead replaces a retained key's item when the incoming snapshot is a different reference; it does **not** preserve row object identity. Avalonia container reuse, selection, and animations depend on the control and its selection model; they are not guaranteed by a matching key. For selection across replaced snapshots, track the selected key and resolve it against the new items.

## Implementations

- [ObservableCollectionExtensions.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Glue.Core/ObservableCollectionExtensions.fs)
- [KeyedCollectionPatching.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Glue.Core/KeyedCollectionPatching.fs)
- [KeyedSnapshotCollection source](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Glue.Core/SnapshotSubstrate.fs)
