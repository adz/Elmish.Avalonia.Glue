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

For a concrete C# application startup using a bound host, see [the dispatcher bridge](../api/Elmish.Avalonia.Glue.ElmishHost.html); derive the view snapshot from the model inside the update callback (for example `model => host.Update(View.fromModel(model))`). Bind the host once and use the Avalonia wrapper for interactive views; see [interactive delivery](../api/Elmish.Avalonia.Glue.ElmishHost.html#interactive-delivery) for threading and failure semantics.

## Keep the connection alive

The returned `ElmishHostConnection<TMsg>` owns the running loop. Store it on
the application or window. Dispose it during shutdown.

Its `Dispatch` member is a stable way to send a message into the running
program. See the <a href="../api/Elmish.Glue.Core.ElmishHostConnection`1.html#disposal-contract">connection disposal contract</a>.

## Keep platform effects at the edge

File pickers, clipboard access, and window services belong at the Avalonia
edge. Translate their result into a message, then let Elmish update state.

Do not hide platform services inside a view record. A view record is data, not
a service locator.

Use the generated [API reference](../api.html) for exact overloads and the
[sample applications](sample-applications.html) for startup code.

Continue with [background work](background-commands.html) to connect commands,
subscriptions, and worker-thread callbacks without touching the host directly.
