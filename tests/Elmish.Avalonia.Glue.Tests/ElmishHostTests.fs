namespace Elmish.Avalonia.Glue.Tests

open System
open System.ComponentModel
open System.Threading
open Elmish
open Elmish.Glue.Core
open Xunit

module ElmishHostTests =

    type private Msg =
        | NoOp

    type private CounterNode() =
        inherit ProjectionNode<int, int>()
        let mutable count = 0
        member this.Count = count
        override this.Update(value) = this.Change(&count, value, nameof this.Count) |> ignore

    [<Fact>]
    let ``projection node notifies only on changed values`` () =
        let node = CounterNode()
        let names = ResizeArray<string>()
        (node :> INotifyPropertyChanged).PropertyChanged.Add(fun args -> names.Add(args.PropertyName))
        node.Update(1)
        node.Update(1)
        node.Update(2)
        Assert.Equal(2, node.Count)
        Assert.Equal<string>([| "Count"; "Count" |], names)
        let mutable dispatched = 0
        (node :> IDispatchTarget<int>).SetDispatch(Action<int>(fun value -> dispatched <- value))
        node.Dispatch.Invoke(42)
        Assert.Equal(42, dispatched)

    [<Fact>]
    let ``concurrent dispatch processes each message once without overlapping updates`` () =
        let mutable active = 0
        let mutable overlap = 0
        let seen = Array.zeroCreate<int> 400000
        let last = [| -1; 199999 |]
        let mutable outOfOrder = 0
        let program =
            Program.mkProgram
                (fun () -> 0, Cmd.none)
                (fun index model ->
                    if Interlocked.Increment(&active) <> 1 then
                        Interlocked.Increment(&overlap) |> ignore
                    let producer = index / 200000
                    if index <> last[producer] + 1 then
                        Interlocked.Increment(&outOfOrder) |> ignore
                    last[producer] <- index
                    Interlocked.Increment(&seen[index]) |> ignore
                    Interlocked.Decrement(&active) |> ignore
                    model + 1, Cmd.none)
                (fun _ _ -> ())
        use host = ElmishHost.start program (Action<int>(ignore))
        let send start =
            Thread(fun () ->
                for index in start .. start + 199999 do
                    host.Dispatch.Invoke(index))
        let first = send 0
        let second = send 200000
        first.Start()
        second.Start()
        first.Join()
        second.Join()
        Assert.Equal(0, overlap)
        Assert.Equal(0, outOfOrder)
        Assert.All(seen, Action<int>(fun count -> Assert.Equal(1, count)))

    [<Fact>]
    let ``failed update releases drain for later messages`` () =
        let seen = ResizeArray<int>()
        let errors = ResizeArray<exn>()
        let program =
            Program.mkProgram (fun () -> 0, Cmd.none)
                (fun msg model ->
                    if msg = 1 then failwith "failed A"
                    seen.Add(msg)
                    model, Cmd.none) (fun _ _ -> ())
            |> Program.withErrorHandler (fun (_, error) -> errors.Add(error))
        use host = ElmishHost.start program (Action<int>(ignore))
        host.Dispatch.Invoke(1)
        host.Dispatch.Invoke(2)
        host.Dispatch.Invoke(3)
        Assert.Single(errors) |> ignore
        Assert.Equal<int>([| 2; 3 |], seen)

    [<Fact>]
    let ``first producer returns despite continuing arrivals`` () =
        use entered = new ManualResetEventSlim(false)
        use release = new ManualResetEventSlim(false)
        use returned = new ManualResetEventSlim(false)
        let mutable seen = 0
        let program =
            Program.mkProgram (fun () -> 0, Cmd.none)
                (fun msg model ->
                    if msg = 0 then
                        entered.Set()
                        release.Wait()
                    Interlocked.Increment(&seen) |> ignore
                    model, Cmd.none) (fun _ _ -> ())
        use host = ElmishHost.start program (Action<int>(ignore))
        let first = Thread(fun () -> host.Dispatch.Invoke(0); returned.Set())
        first.Start()
        try
            Assert.True(entered.Wait(TimeSpan.FromSeconds(5.0)))
            for n in 1 .. 10000 do host.Dispatch.Invoke(n)
            release.Set()
            Assert.True(returned.Wait(TimeSpan.FromSeconds(5.0)), "First producer was held by later arrivals")
        finally
            release.Set()
            first.Join()
        SpinWait.SpinUntil((fun () -> Volatile.Read(&seen) = 10001), TimeSpan.FromSeconds(10.0)) |> ignore
        Assert.Equal(10001, seen)

    [<Fact>]
    let ``core startAndBind delivers on dispatching thread`` () =
        let mutable send = Action<int>(ignore)
        let mutable deliveredThread = -1
        let program =
            Program.mkProgram (fun () -> 0, Cmd.none)
                (fun msg _ -> msg, Cmd.none) (fun _ _ -> ())
        use host =
            ElmishHost.startAndBind program
                (Action<int>(fun _ -> deliveredThread <- Environment.CurrentManagedThreadId))
                (Action<Action<int>>(fun dispatch -> send <- dispatch))
        let worker = Thread(fun () ->
            send.Invoke(42)
            Assert.Equal(Environment.CurrentManagedThreadId, deliveredThread))
        worker.Start()
        worker.Join()

    [<Fact>]
    let ``disposing host terminates Elmish subscriptions`` () =
        let disposed = new ManualResetEventSlim(false)

        let program =
            Program.mkProgram
                (fun () -> 0, Cmd.none)
                (fun _ model -> model, Cmd.none)
                (fun _ _ -> ())
            |> Program.withSubscription (fun _ ->
                [ [ "subscription" ],
                  fun _ ->
                      { new IDisposable with
                          member _.Dispose() = disposed.Set() } ])

        let host =
            ElmishHost.startWithPost
                (Action<Action>(fun action -> action.Invoke()))
                program
                (Action<int>(ignore))

        (host :> IDisposable).Dispose()

        Assert.True(disposed.Wait(TimeSpan.FromSeconds(2.0)), "Expected Elmish subscription to be disposed when the host is disposed.")
