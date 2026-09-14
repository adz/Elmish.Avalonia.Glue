# Elmish.Avalonia.Glue

This package is the Avalonia runtime boundary. Its `ElmishHost` overloads use
Avalonia's UI dispatcher before updating a host.

## Start and own the application loop

Use `start` when the UI only receives snapshots. Use `startAndBind` when the
host also needs the Elmish dispatcher for commands or writable properties.

Store the returned connection for as long as the view is active, then dispose
it during shutdown.

The package also retains compatibility aliases for the shared Core types.
New lower-level code can name `Elmish.Glue.Core` directly.
