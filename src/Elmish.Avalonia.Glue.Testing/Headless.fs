namespace Elmish.Avalonia.Glue.Testing

open System
open System.Diagnostics
open Avalonia
open Avalonia.Controls
open Avalonia.Headless
open Avalonia.Threading
open Elmish.Avalonia.Glue

/// Helpers for running a real Avalonia drawing surface without a display.
type Headless =
    static member BuildAvaloniaApp() =
        AppBuilder.Configure<Application>().UseHeadless(AvaloniaHeadlessPlatformOptions(UseHeadlessDrawing = false)).UseSkia()

    static member start program onUpdate = ElmishHost.startAndBind program onUpdate

    static member waitUntil (predicate: unit -> bool) (timeout: TimeSpan) =
        let clock = Stopwatch.StartNew()
        while not (predicate()) && clock.Elapsed < timeout do
            Dispatcher.UIThread.RunJobs()
            System.Threading.Thread.Sleep(1)
        if not (predicate()) then
            failwithf "Timed out after %O waiting for headless Avalonia state" timeout

    static member captureFrame (window: Window) = window.CaptureRenderedFrame()
