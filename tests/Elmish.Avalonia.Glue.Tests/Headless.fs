namespace Elmish.Avalonia.Glue.Tests

open System
open System.Diagnostics
open Avalonia
open Avalonia.Controls
open Avalonia.Headless
open Avalonia.Headless.XUnit
open Avalonia.Threading
open Elmish
open Elmish.Avalonia.Glue
open Xunit

// AvaloniaFact creates the application on the dispatcher thread.
type TestAppBuilder =
    static member BuildAvaloniaApp() =
        AppBuilder.Configure<Application>().UseHeadless(AvaloniaHeadlessPlatformOptions(UseHeadlessDrawing = false)).UseSkia()

[<assembly: AvaloniaTestApplication(typeof<TestAppBuilder>)>]
do ()

[<CollectionDefinition("Avalonia headless", DisableParallelization = true)>]
type HeadlessCollection() = class end

module Headless =
    let start program onUpdate = ElmishHost.startAndBind program onUpdate

    let waitUntil (predicate: unit -> bool) (timeout: TimeSpan) =
        let clock = Stopwatch.StartNew()
        while not (predicate()) && clock.Elapsed < timeout do
            Dispatcher.UIThread.RunJobs()
            System.Threading.Thread.Sleep(1)
        if not (predicate()) then
            failwithf "Timed out after %O waiting for headless Avalonia state" timeout

    let captureFrame (window: Window) = window.CaptureRenderedFrame()
