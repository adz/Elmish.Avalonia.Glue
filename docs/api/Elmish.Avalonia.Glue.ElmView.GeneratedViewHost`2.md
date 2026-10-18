# Elmish.Avalonia.Glue.ElmView.GeneratedViewHost`2

## `GeneratedViewHost<'View, 'Msg>`

The abstract base class for all ElmView root hosts.

### Members

| Member | Description |
| :--- | :--- |
| `View` | Returns the current immutable view record snapshot. |
| `SetDispatch(dispatcher)` | Connects the host to the Elmish loop. |
| `Dispatch(message)` | Dispatches a message to the Elmish loop. |
| `WriteBackBindings` | Returns the `WriteBackBindings` collection for this host. |
| `TryDispatchWriteBack(path, value)` | Attempts to dispatch a message for a property change at the specified path. This is used by generated property setters. |
| `GeneratedPropertyNames` | Returns the property names exposed by the host. These properties are included in `PropertyChanged` notifications during refresh. |


## Snapshot refresh sequence

1. **Initialization**: The host is constructed with an initial snapshot and an optional configuration action for `WriteBackBindings`.
2. **Binding**: In AXAML, the host is set as the `DataContext`.
3. **Update**: When the Elmish program produces a new view snapshot, `host.Update(newSnapshot)` is called.
4. **Refresh**: Only when the new view is a different reference: it stores the new view, raises `PropertyChanged("View")`, then raises each name in `GeneratedPropertyNames` in enumeration order, then refreshes registered child subtrees (each node notifies its listed properties before its children). Notifications are unconditional per changed reference, not per changed field; same-reference updates do nothing. Exceptions from handlers interrupt the sequence.
