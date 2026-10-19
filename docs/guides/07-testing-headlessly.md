# Testing headlessly

> Maturity: **Experimental** · [package maturity](package-maturity.html)

Reference `Elmish.Avalonia.Glue.Testing` and `Avalonia.Headless.XUnit` from your test project. Run the compiled repository test with `dotnet test tests/Elmish.Avalonia.Glue.Tests/Elmish.Avalonia.Glue.Tests.fsproj`; [HeadlessTests.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/tests/Elmish.Avalonia.Glue.Tests/HeadlessTests.fs) contains the working bound-window test. The sketch below is illustrative, not a replacement for that test. It supplies a real headless Skia drawing surface without depending on a test framework. Configure your framework to create the app on the Avalonia dispatcher thread:

The following is a sketch, **not** a runnable test (the assertion and window creation are omitted). For a compiled end-to-end example see [HeadlessTests.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/tests/Elmish.Avalonia.Glue.Tests/HeadlessTests.fs). Add `Avalonia.Headless.XUnit` to your test project; the testing helper does not reference it.

```text
open Avalonia.Headless
open Avalonia.Headless.XUnit
open Elmish.Avalonia.Glue.Testing

// Avalonia.Headless.XUnit belongs in the test project, not the helper package.
type TestAppBuilder =
    static member BuildAvaloniaApp() = Headless.BuildAvaloniaApp()

[<assembly: AvaloniaTestApplication(typeof<TestAppBuilder>)>]
do ()

[<AvaloniaFact>]
let ``window renders`` () =
    // Start your Elmish program using Elmish.Avalonia.Glue.ElmishHost.startAndBind program onUpdate setDispatch.
    // Show a bound Window, then pump pending UI jobs:
    // Headless.waitUntil (fun () -> view.Text = "ready") (TimeSpan.FromSeconds(5.0))
    // use frame = Headless.captureFrame window
    // Assert on rendered pixel data as well as PixelSize.
    ()
```

Close the window and dispose the host at the end of the test. Run the repository example with `dotnet test tests/Elmish.Avalonia.Glue.Tests/Elmish.Avalonia.Glue.Tests.fsproj`. See the linked test for a complete bound-window example.
