# Connect the runtime

Connect one host when the Avalonia application starts, and dispose that
connection when the application ends.

## Assign the host once

Construct the view and its host before loading bindings. Keep the same host as
`DataContext` for the lifetime of the view.

Replacing the `DataContext` on each update defeats the stable-host model and
can discard control state.

## Start Elmish through the Avalonia bridge

`Elmish.Avalonia.Glue.ElmishHost.startAndBind` connects three pieces (this is an F# module function; from F# pass an Elmish `Program<unit, Model, Msg, unit>` plus `Action<Model>` and `Action<Action<Msg>>` callbacks):

1. the Elmish program;
2. the host's snapshot update function;
3. the host's dispatch setter.

For a concrete C# application startup using a bound host, see [the dispatcher bridge](../api/avalonia/dispatcher.html); derive the view snapshot from the model inside the update callback (for example `model => host.Update(View.fromModel(model))`). The Avalonia wrapper delivers updates inline when already on the UI thread;
from other threads it posts to `Dispatcher.UIThread`. The core host has no
Avalonia dependency; it accepts a generic post function instead. Inline
delivery matters for `TwoWay` controls such as `TextBox`: queuing an older
model after a newer keystroke can write stale text back into the control.
Bind the host once and use the Avalonia wrapper for interactive views. Queued worker messages precede a later UI dispatch; the bridge does not cancel pending posts or catch exceptions from `Post` or `onUpdate`.

## Keep the connection alive

The returned `ElmishHostConnection<TMsg>` owns the running loop. Store it on
the application or window. Dispose it during shutdown.

Its `Dispatch` member is a stable way to send a message into the running
program. After disposal, `Dispatch` silently ignores new user messages; already queued messages may still drain before the queued termination message. Disposal can post asynchronously from a worker thread; keep the UI dispatcher running until shutdown completes. See [host lifetime](../api/core/host-lifetime.html).

## Keep platform effects at the edge

File pickers, clipboard access, and window services belong at the Avalonia
edge. Translate their result into a message, then let Elmish update state.

Do not hide platform services inside a view record. A view record is data, not
a service locator.

Use the generated [API reference](../api.html) for exact overloads and the
[sample applications](sample-applications.html) for startup code.

Continue with [background work](background-commands.html) to connect commands,
subscriptions, and worker-thread callbacks without touching the host directly.
