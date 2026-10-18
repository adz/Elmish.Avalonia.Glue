# Elmish.Avalonia.Glue.Projection.SnapshotHost`1

## `SnapshotHost<'T>`

### Members

| Member | Description |
| :--- | :--- |
| `Current` | Returns the current immutable snapshot. Binds in XAML to `Current`. |
| `Update(nextSnapshot)` | Inherited from `BindableSnapshotHost`. Updates the snapshot and raises `PropertyChanged` for `Current`. |
