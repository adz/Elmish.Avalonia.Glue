namespace Elmish.Avalonia.Glue.Tests

open System
open System.Threading
open Elmish
open Elmish.Avalonia.Glue
open Xunit

module ElmishHostTests =

    type private Msg =
        | NoOp

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

        let host = ElmishHost.start program (Action<int>(ignore))

        (host :> IDisposable).Dispose()

        Assert.True(disposed.Wait(TimeSpan.FromSeconds(2.0)), "Expected Elmish subscription to be disposed when the host is disposed.")
