# Dispatcher

A small dispatch target for a projection. `Connect` stores the callback; `Send` invokes it synchronously. Connect before sending: an unconnected callback cannot be invoked. It performs no UI-thread marshalling; use the Avalonia host for bound updates.
