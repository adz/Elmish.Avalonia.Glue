# FSharpProjectionBase

Implement `Update` to apply each model snapshot. `SetDispatch` supplies the callback exposed by `Dispatch`; attach it before sending messages. Notifications and thread marshalling remain the responsibility of the derived projection and its host.
