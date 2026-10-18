# Combined projection contract

Combines `IProjection<Model>.Update` with `IDispatchTarget<Msg>.SetDispatch`; a projection receives immutable model snapshots and forwards user actions to Elmish.
