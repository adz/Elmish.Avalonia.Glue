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
        override this.Update(value) = this.Change(&count, value, "Count") |> ignore

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
        let program =
            Program.mkProgram
                (fun () -> 0, Cmd.none)
                (fun index model ->
                    if Interlocked.Increment(&active) <> 1 then
                        Interlocked.Increment(&overlap) |> ignore
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
        Assert.All(seen, Action<int>(fun count -> Assert.Equal(1, count)))

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
