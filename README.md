# Elmish.Avalonia.Glue

- Avalonia has `.axaml`, bindings, design-time preview, and great DevTools.
- Elmish has immutable state, explicit messages, and one update loop.

Elmish.Avalonia.Glue keeps normal AXAML and designer tooling while moving UI
state and UI-shaped data into Elmish-style F#.

It supports two approaches:

- `Projection` uses CLR XAML-facing viewmodels populated from Elmish state
- `ElmView` binds immutable F# view records through a thin API with less C#

The project does not replace AXAML with an F# UI DSL or change how Elmish works in either case.


## Install and run

Use the .NET 10 SDK (bundled F#), Avalonia 12.0.0 and Elmish 5.0.2. Package IDs: `Elmish.Glue.Core`, `Elmish.Avalonia.Glue`, `Elmish.Avalonia.Glue.Projection`, `Elmish.Avalonia.Glue.ElmView`, and `Elmish.Avalonia.Glue.Testing`. Install the Avalonia bridge and the style package for your screen (Core is a transitive reference); use `Avalonia.Desktop` for a desktop UI. See [installation and compatibility](https://adz.github.io/Elmish.Avalonia.Glue/guides/install.html).

```bash
dotnet run --project sample/Samples.ElmView/ExampleMatrixSample/src/ExampleMatrixSample.UI/ExampleMatrixSample.UI.csproj
```

## Tasks

- [Startup](https://adz.github.io/Elmish.Avalonia.Glue/guides/connect-runtime.html) · [Write-back](https://adz.github.io/Elmish.Avalonia.Glue/guides/route-editable-elmview-properties.html) · [Nested nodes](https://adz.github.io/Elmish.Avalonia.Glue/guides/nested-elmview-paths.html) · [Lists](https://adz.github.io/Elmish.Avalonia.Glue/guides/keyed-collections.html)
- [Design preview](https://adz.github.io/Elmish.Avalonia.Glue/guides/design-time-preview.html) · [Background work](https://adz.github.io/Elmish.Avalonia.Glue/guides/background-commands.html) · [Headless tests](https://adz.github.io/Elmish.Avalonia.Glue/guides/testing-headlessly.html) · [All-F# UI](https://adz.github.io/Elmish.Avalonia.Glue/guides/all-fsharp-ui.html)
- [Choose an adapter](https://adz.github.io/Elmish.Avalonia.Glue/index.html#choose-the-adapter) · [Glossary](https://adz.github.io/Elmish.Avalonia.Glue/guides/glossary.html) · [Troubleshooting](https://adz.github.io/Elmish.Avalonia.Glue/guides/troubleshooting.html)

## Start with the guide

- [Docs site](https://adz.github.io/Elmish.Avalonia.Glue/): start here if you want the intent before the APIs.

## Packages

- [Architecture](https://adz.github.io/Elmish.Avalonia.Glue/guides/architecture.html) explains the shared substrate.
- [Projection](https://adz.github.io/Elmish.Avalonia.Glue/guides/projection.html) covers explicit CLR viewmodels.
- [ElmView](https://adz.github.io/Elmish.Avalonia.Glue/guides/elmview.html) covers immutable F# view records.
- [Package maturity](https://adz.github.io/Elmish.Avalonia.Glue/guides/package-maturity.html) lists each package's maturity level and its evidence.

## Samples

- [Projection samples](https://github.com/adz/Elmish.Avalonia.Glue/tree/main/sample/Samples.Projection): explicit CLR viewmodel path.
- [ElmView samples](https://github.com/adz/Elmish.Avalonia.Glue/tree/main/sample/Samples.ElmView): F# view-record path.

Clone the repository and follow the [sample applications guide](https://adz.github.io/Elmish.Avalonia.Glue/guides/sample-applications.html) for setup and run commands; the links above are source directories, not install instructions.
