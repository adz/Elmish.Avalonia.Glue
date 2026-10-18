# Elmish.Glue.Core.ElmishHost

## `ElmishHost`

### Static Methods

| Method | Description |
| :--- | :--- |
| `start(program, onUpdate)` | Starts an Elmish program and calls `onUpdate` synchronously whenever the model changes. |
| `startWithPost(program, post, onUpdate)` | Starts an Elmish program and uses the provided `post` function to marshal `onUpdate` calls. This is used by the Avalonia package to marshal to the UI thread. |
| `startAndBind(program, post, onUpdate, setDispatch)` | Starts the program and also calls `setDispatch` with the stable dispatch function. |


## Core queue guarantees

The framework-neutral core host instead drains on the dispatching thread
that finds the queue idle: it guarantees serialization and FIFO order, not thread
affinity. Each drain handles only the queue snapshot it found at entry; later arrivals are scheduled on the thread pool (which calls the supplied `post`), unless another dispatcher takes the idle queue first, so
one producer does not have to drain an indefinitely growing queue. Update exceptions propagate to the draining caller and stop that batch; the next dispatch can resume remaining queued messages. A failure from `post` is not recovered here and can leave the queue waiting. Disposal queues a stop message, so messages already queued may run first. Core `startAndBind` delivers models on the draining thread.



## Framework-neutral posting

By keeping the host mechanics in the core package, we can:
1. **Testability**: Run Elmish programs in headless unit tests without needing a full UI framework.
2. **Framework Neutrality**: The core host doesn't know about Avalonia. It only knows about a generic `post` function, allowing it to be adapted to other frameworks (like WPF or even console apps) in the future.
3. **Stable Dispatch**: The connection's `Dispatch` action remains the same object during one run. This API does not restart or replace a program; create a new connection for a new run. After disposal, user dispatches are silently ignored.
