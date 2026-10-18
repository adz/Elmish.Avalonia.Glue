# Testing headlessly

Reference `Elmish.Avalonia.Glue.Testing` from your test project. It supplies a real headless Skia drawing surface without depending on a test framework. Configure your framework to create the app on the Avalonia dispatcher thread:

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
    // Start your Elmish program using Headless.start program onUpdate setDispatch.
    // Show a bound Window, then pump pending UI jobs:
    // Headless.waitUntil (fun () -> view.Text = "ready") (TimeSpan.FromSeconds(5.0))
    // use frame = Headless.captureFrame window
    // Assert on rendered pixel data as well as PixelSize.
    ()
```

Close the window and dispose the host at the end of the test. See `HeadlessTests.fs` for a complete bound-window example.
