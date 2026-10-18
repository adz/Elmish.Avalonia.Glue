# Elmish.Glue.Core.BindableSnapshotHost`1

## `BindableSnapshotHost<'Snapshot>`

An abstract base class for top-level view hosts. It manages the current snapshot and handles update notifications.

### Members

| Member | Description |
| :--- | :--- |
| `Snapshot` | Returns the current immutable snapshot. |
| `Update(nextSnapshot: 'Snapshot)` | Updates the host with a new snapshot. For reference-type snapshots (including `null`), if `ReferenceEquals(current, next)` is false, it stores `next`, synchronously raises `PropertyChanged` for the constructor-supplied name, then calls `OnSnapshotUpdated(previous, next)`. Same-reference updates do nothing, even if that object was mutated. Exceptions from listeners prevent the hook from running; there is no rollback. |
| `OnSnapshotUpdated(previous, next)` | An overridable hook called after the snapshot has been updated and notifications have been raised. |
