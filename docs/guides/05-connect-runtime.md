# Connect the runtime

Connect one host when the Avalonia application starts, and dispose that
connection when the application ends.

## Assign the host once

Construct the view and its host before loading bindings. Keep the same host as
`DataContext` for the lifetime of the view.

Replacing the `DataContext` on each update defeats the stable-host model and
can discard control state.

## Start Elmish through the Avalonia bridge

`Elmish.Avalonia.Glue.ElmishHost.startAndBind` connects three pieces:

1. the Elmish program;
2. the host's snapshot update function;
3. the host's dispatch setter.

The Avalonia wrapper posts updates to `Dispatcher.UIThread`. The core host has
no Avalonia dependency; it accepts a generic post function instead.

## Keep the connection alive

The returned `ElmishHostConnection<TMsg>` owns the running loop. Store it on
the application or window. Dispose it during shutdown.

Its `Dispatch` member is a stable way to send a message into the running
program. After disposal, the connection stops accepting useful work.

## Keep platform effects at the edge

File pickers, clipboard access, and window services belong at the Avalonia
edge. Translate their result into a message, then let Elmish update state.

Do not hide platform services inside a view record. A view record is data, not
a service locator.

Use the generated [API reference](../api.html) for exact overloads and the
[sample applications](sample-applications.html) for startup code.

Continue with [background work](background-work.html) to connect commands,
subscriptions, and worker-thread callbacks without touching the host directly.
