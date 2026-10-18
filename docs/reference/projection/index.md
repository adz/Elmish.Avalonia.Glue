# Elmish.Avalonia.Glue.Projection

Projection supports explicit CLR-facing viewmodels over immutable Elmish
state.

## Choose the smallest projection surface

Use `SnapshotHost<T>` when AXAML can read most values directly through
`Current`. Add named properties or commands only where they clarify the view
contract.

Use `KeyedSnapshotCollection<T,TKey>` when a bound collection must remain
stable while its immutable contents change.

Use Core's projection interfaces and `SyncWith` overloads for mutable row
viewmodels that update in place and dispatch child messages.
