# Elmish.Avalonia.Glue

> Maturity: **Incubating** · [package maturity](/guides/package-maturity.html)

This package is the Avalonia runtime boundary. Its `Elmish.Avalonia.Glue.ElmishHost` overloads use
Avalonia's UI dispatcher before updating a host; the core host is a separate
`Elmish.Glue.Core.ElmishHost` module.

## Start and own the application loop

Use `start` when the UI only receives snapshots. Use `startAndBind` when the
host also needs the Elmish dispatcher for commands or writable properties.

Store the returned connection for as long as the view is active, then dispose
it during shutdown.

The package also retains compatibility aliases for the shared Core types.
New lower-level code can name `Elmish.Glue.Core` directly.

## F# styled and attached property compatibility

With the Avalonia 12.0.0 XAML compiler used by this repository, F# compiled values for styled/attached property registrations can appear as properties rather than the public static fields the compiler looks up, producing `AVLN3000` when referenced from AXAML. This is an observed interoperability constraint, not a blanket prohibition on all F# custom controls or future Avalonia versions. Use a C# declaration when AXAML needs the property registration field.

