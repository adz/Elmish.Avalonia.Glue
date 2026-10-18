# Elmish.Glue.Core.IProjection`1

## Update and dispatch contracts

### `IProjection<'Model>`
Defines the contract for an object that can be updated with a new model snapshot.

| Member | Description |
| :--- | :--- |
| `Update(model)` | Updates the object with the latest model data. |

### `IDispatchTarget<'Msg>`
Defines the contract for an object that can dispatch messages back to the Elmish loop.

| Member | Description |
| :--- | :--- |
| `SetDispatch(dispatch)` | Provides the object with a dispatch function. |
