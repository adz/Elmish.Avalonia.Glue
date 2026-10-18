# Elmish.Avalonia.Glue

- Avalonia has `.axaml`, bindings, design-time preview, and great DevTools.
- Elmish has immutable state, explicit messages, and one update loop.

Elmish.Avalonia.Glue keeps normal AXAML and designer tooling while moving UI
state and UI-shaped data into Elmish-style F#.

It supports two approaches:

- `Projection` uses CLR XAML-facing viewmodels populated from Elmish state
- `ElmView` binds immutable F# view records through a thin API with less C#

The project does not replace AXAML with an F# UI DSL or change how Elmish works in either case.


## Start with the guide

- [Docs site](https://adz.github.io/Elmish.Avalonia.Glue/): start here if you want the intent before the APIs.

## Packages

- [Architecture](https://adz.github.io/Elmish.Avalonia.Glue/guides/architecture.html) explains the shared substrate.
- [Projection](https://adz.github.io/Elmish.Avalonia.Glue/guides/projection.html) covers explicit CLR viewmodels.
- [ElmView](https://adz.github.io/Elmish.Avalonia.Glue/guides/elmview.html) covers immutable F# view records.

## Samples

- [Projection samples](https://github.com/adz/Elmish.Avalonia.Glue/tree/main/sample/Samples.Projection): explicit CLR viewmodel path.
- [ElmView samples](https://github.com/adz/Elmish.Avalonia.Glue/tree/main/sample/Samples.ElmView): F# view-record path.

Clone the repository and follow the [sample applications guide](https://adz.github.io/Elmish.Avalonia.Glue/guides/sample-applications.html) for setup and run commands; the links above are source directories, not install instructions.
