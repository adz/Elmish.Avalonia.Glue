# Elmish.Glue.Core.ProjectionExtensions

## Composition helpers

The `ProjectionExtensions` module provides helpers for wiring up projection trees.

### `ForwardTo`
Composes a child dispatcher into a parent dispatcher using a message mapping function.

### `SyncWith`
Synchronizes a collection of viewmodels with a list of models.

| Overload | Description |
| :--- | :--- |
| `SyncWith(collection, models, modelKey, vmKey, create)` | Simple synchronization where viewmodels only need an `Update` call. |
| `SyncWith(collection, models, modelKey, vmKey, create, parentDispatch, map)` | Synchronization that also wires up dispatching for new viewmodels. |
