# Elmish.Avalonia.Glue

- Avalonia has `.axaml`, bindings, design-time preview, and great DevTools.
- Elmish has immutable state, explicit messages, and one update loop.

**Can an Avalonia app keep normal AXAML and designer tooling while moving UI
state and UI-shaped data into Elmish-style F#?**

*Elmish.Avalonia.Glue* does exactly this in two different approaches:

- `Projection` where you create normal CLR XAML-facing viewmodels, and 'project' into it
- `ElmView` with less C# - immutable F# view records bind via a thin api

The project does not replace AXAML with an F# UI DSL or change how Elmish works in either case.

It's just the glue between them.

## Read next

- [Docs site](https://adz.github.io/Elmish.Avalonia.Glue/docs/intro): start here if you want the intent before the APIs.

## Packages

- [Elmish.Avalonia.Glue](https://adz.github.io/Elmish.Avalonia.Glue/docs/guides/understand/shared-substrate) - the shared substrate glue between immutable snapthos and bindable objects.
- [Elmish.Avalonia.Glue.Projection](https://adz.github.io/Elmish.Avalonia.Glue/docs/guides/understand/projection-family)
- [Elmish.Avalonia.Glue.ElmView](https://adz.github.io/Elmish.Avalonia.Glue/docs/guides/understand/elmview-family)

## Samples

- [Projection samples](sample/Samples.Projection): explicit CLR viewmodel path.
- [ElmView samples](sample/Samples.ElmView): F# view-record path.
