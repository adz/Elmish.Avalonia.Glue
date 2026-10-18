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

- keep older call sites readable
- move new docs toward the core package without breaking the existing package shape

## Implementation

- [Package.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Avalonia.Glue/Package.fs)
