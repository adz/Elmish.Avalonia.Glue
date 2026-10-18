# Install and run

Use the .NET 10 SDK (including its bundled F# compiler). This repository targets `net10.0` and currently builds against Avalonia 12.0.0 and Elmish 5.0.2; other version combinations are not verified here. Install Avalonia.Desktop 12.0.0 for desktop apps. NuGet package IDs are `Elmish.Glue.Core` (framework-neutral hosts and collections), `Elmish.Avalonia.Glue` (UI-thread bridge), `Elmish.Avalonia.Glue.Projection` (projection hosts), `Elmish.Avalonia.Glue.ElmView` (view-record hosts), and `Elmish.Avalonia.Glue.Testing` (headless helpers). Choose the style package plus the Avalonia bridge; its project reference supplies Core transitively. An F# model project needs Elmish; an Avalonia UI project needs Avalonia.Desktop.

For a runnable repository checkout (including local library project references):

```bash
dotnet run --project sample/Samples.ElmView/ExampleMatrixSample/src/ExampleMatrixSample.UI/ExampleMatrixSample.UI.csproj
```

See [sample applications](sample-applications.html) for the Projection alternative and startup wiring. Published package versions should be selected together; the repo's local default `VersionPrefix` is 0.1.0, not a promise of published compatibility.
