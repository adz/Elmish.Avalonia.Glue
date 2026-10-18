# Elmish.Glue.Core.KeyedSnapshotCollection`2

## `KeyedSnapshotCollection<'T, 'Key>`

A helper class around an `ObservableCollection` with an `Update` method. Both
Projection and ElmView use it for lists of snapshots.

### Members

| Member | Description |
| :--- | :--- |
| `Items` | Returns the underlying `ObservableCollection<'T>`. |
| `Update(next: IReadOnlyList<'T>)` | Patches the collection to match the new list of snapshots. |
