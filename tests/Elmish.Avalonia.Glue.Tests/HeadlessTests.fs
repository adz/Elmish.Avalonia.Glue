namespace Elmish.Avalonia.Glue.Tests

open System
open System.Collections.Generic
open System.Threading.Tasks
open Avalonia.Controls
open Avalonia.Headless.XUnit
open Avalonia.Threading
open Elmish
open Elmish.Avalonia.Glue
open Xunit

type FrameViewModel() =
    member val Count = "0" with get, set

[<Collection("Avalonia headless")>]
module HeadlessTests =
    let private program onMessage =
        Program.mkProgram (fun () -> [], Cmd.none)
            (fun msg model -> onMessage msg; msg :: model, Cmd.none)
            (fun _ _ -> ())

    [<AvaloniaFact>]
    let ``worker producers run on UI thread once and in producer order`` () =
        let seen = ResizeArray<int>()
        let accesses = ResizeArray<bool>()
        use host = Headless.start (program (fun msg ->
            seen.Add(msg)
            accesses.Add(Dispatcher.UIThread.CheckAccess()))) (Action<int list>(ignore)) (Action<Action<int>>(ignore))
        let producers = [| for p in 0 .. 1 -> Task.Run(fun () -> for n in 0 .. 49 do host.Dispatch.Invoke(p * 100 + n)) |]
        Task.WaitAll(producers)
        Headless.waitUntil (fun () -> seen.Count = 100) (TimeSpan.FromSeconds(5.0))
        Assert.All(accesses, Action<bool>(fun access -> Assert.True(access)))
        for p in 0 .. 1 do
            Assert.Equal<int>([| for n in 0 .. 49 -> p * 100 + n |], seen |> Seq.filter (fun n -> n / 100 = p) |> Seq.toArray)

    [<AvaloniaFact>]
    let ``UI dispatch cannot overtake queued worker messages`` () =
        let seen = ResizeArray<int>()
        use host = Headless.start (program seen.Add) (Action<int list>(ignore)) (Action<Action<int>>(ignore))
        Task.Run(fun () -> for n in 0 .. 9 do host.Dispatch.Invoke(n)).Wait()
        host.Dispatch.Invoke(10)
        Headless.waitUntil (fun () -> seen.Count = 11) (TimeSpan.FromSeconds(5.0))
        Assert.Equal<int>([| 0 .. 10 |], seen)

    [<AvaloniaFact>]
    let ``startAndBind delivers model inline on UI thread`` () =
        let mutable model = []
        let mutable dispatch = Action<int>(ignore)
        use host = Headless.start (program ignore) (Action<int list>(fun next -> model <- next)) (Action<Action<int>>(fun send -> dispatch <- send))
        dispatch.Invoke(42)
        Assert.Equal<int list>([42], model)

    [<AvaloniaFact>]
    let ``bound window captures a rendered frame`` () =
        let vm = FrameViewModel()
        use host = Headless.start (program ignore) (Action<int list>(fun next -> vm.Count <- string next.Length)) (Action<Action<int>>(ignore))
        host.Dispatch.Invoke(1)
        let window = new Window(Width = 160., Height = 80.)
        let text = TextBlock()
        text.Bind(TextBlock.TextProperty, Avalonia.Data.Binding("Count")) |> ignore
        window.DataContext <- vm
        window.Content <- text
        window.Show()
        Headless.waitUntil (fun () -> text.Text = "1") (TimeSpan.FromSeconds(5.0))
        use bitmap = Headless.captureFrame window
        Assert.NotNull(bitmap)
        Assert.True(bitmap.PixelSize.Width > 0 && bitmap.PixelSize.Height > 0)
        window.Close()
