# Elmish.Glue.Core.ObservableCollectionExtensions

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
