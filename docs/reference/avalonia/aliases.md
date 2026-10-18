# Compatibility aliases

The Avalonia package preserves existing type names as aliases to the core surface.

## Reason for the aliases

This keeps the public API stable while the framework-neutral types live in `Elmish.Glue.Core`.

## Aliased types

- `ElmishHostConnection<'Msg>`
- `IProjection<'Model>`
- `IDispatchTarget<'Msg>`
- `IProjection<'Model,'Msg>`
- `ProjectionExtensions`
- `ObservableCollectionExtensions`
- `FSharpProjectionBase<'Model,'Msg>`
- `Dispatcher<'Msg>`

## Migration guidance

Existing `Elmish.Avalonia.Glue` names above forward to `Elmish.Glue.Core` counterparts; for new code reference `Elmish.Glue.Core`, change the namespace in `open`/`using` directives, and add the `Elmish.Glue.Core` project/package reference if it is not already transitive. `Elmish.Avalonia.Glue.ElmishHost` is **not** the core host: keep it for Avalonia UI-thread delivery; `Elmish.Glue.Core.ElmishHost` requires caller-provided posting when needed. Source compatibility of namespace changes, overload resolution and F# type inference is not guaranteed: compile each caller after migration. Do not remove the Avalonia bridge from an interactive app.

## Implementation

- [Package.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Avalonia.Glue/Package.fs)
