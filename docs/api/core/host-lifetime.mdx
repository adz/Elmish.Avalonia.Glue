---
sidebar_position: 2
---

# Host lifetime

The `ElmishHost` module in the core package provides the fundamental mechanics for running an Elmish program and connecting it to an external update loop.

## `ElmishHost`

### Static Methods

| Method | Description |
| :--- | :--- |
| `start(program, onUpdate)` | Starts an Elmish program and calls `onUpdate` synchronously whenever the model changes. |
| `startWithPost(program, post, onUpdate)` | Starts an Elmish program and uses the provided `post` function to marshal `onUpdate` calls. This is used by the Avalonia package to marshal to the UI thread. |
| `startAndBind(program, post, onUpdate, setDispatch)` | Starts the program and also calls `setDispatch` with the stable dispatch function. |

## `ElmishHostConnection<'Msg>`

This class represents a running instance of an Elmish program. It implements `IDisposable` and provides a stable dispatch surface.

### Members

| Member | Description |
| :--- | :--- |
| `Dispatch(message)` | Dispatches a message to the running Elmish program. |
| `Dispose()` | Stops the Elmish program and cleans up any subscriptions. |

## Why is it in Core?

By keeping the host mechanics in the core package, we can:
1. **Testability**: Run Elmish programs in headless unit tests without needing a full UI framework.
2. **Framework Neutrality**: The core host doesn't know about Avalonia. It only knows about a generic `post` function, allowing it to be adapted to other frameworks (like WPF or even console apps) in the future.
3. **Stable Dispatch**: The host ensures that the `dispatch` function provided to the UI remains stable even if the underlying program is restarted or modified.

## Source

- [ElmishHost.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Glue.Core/ElmishHost.fs)
