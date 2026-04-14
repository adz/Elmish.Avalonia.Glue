namespace Elmish.Avalonia.Glue.Tests

open System
open System.Collections.Generic
open System.ComponentModel
open System.Linq.Expressions
open Elmish.Avalonia.Glue.ElmView
open Xunit

module ElmViewGeneratedHostTests =

    type private ChildView =
        {
            Name: string
            IsEnabled: bool
        }

    type private RootView =
        {
            Title: string
            Child: ChildView
        }

    type private Msg =
        | SetName of string

    let private childNameSelector () =
        let root = Expression.Parameter(typeof<RootView>, "x")
        let child = Expression.Property(root, nameof Unchecked.defaultof<RootView>.Child)
        let name = Expression.Property(child, nameof Unchecked.defaultof<ChildView>.Name)
        Expression.Lambda<Func<RootView, string>>(name, root)

    let private configureBindings =
        Action<WriteBackBindings<RootView, Msg>>(fun bindings ->
            bindings.For(childNameSelector()).Dispatch(Func<string, Msg>(SetName)) |> ignore)

    type private SampleHost(initialView: RootView) as this =
        inherit RuntimeGeneratedViewHost<RootView, Msg>(initialView, configureBindings)

        let child = SampleChildNode(this)

        override _.GeneratedPropertyNames =
            [ "Title"; "Child" ]

        member this.Title = this.View.Title
        member _.Child = child

    and private SampleChildNode(host: SampleHost) =
        inherit GeneratedViewNode<RootView, ChildView, Msg>(
            (fun () -> host.View),
            host.Dispatch,
            host.RegisterChildNode,
            (fun root -> root.Child),
            [ "Name"; "IsEnabled" ])

        member this.Name
            with get () = this.Snapshot.Name
            and set value = host.TryDispatchWriteBack("Child.Name", value) |> ignore

        member this.IsEnabled = this.Snapshot.IsEnabled

    let private createView title name isEnabled =
        {
            Title = title
            Child =
                {
                    Name = name
                    IsEnabled = isEnabled
                }
        }

    [<Fact>]
    let ``generated host getters read the latest immutable snapshot`` () =
        let host = SampleHost(createView "Before" "Ada" true)

        Assert.Equal("Before", host.Title)
        Assert.Equal("Ada", host.Child.Name)
        Assert.True(host.Child.IsEnabled)

        host.Update(createView "After" "Linus" false)

        Assert.Equal("After", host.Title)
        Assert.Equal("Linus", host.Child.Name)
        Assert.False(host.Child.IsEnabled)

    [<Fact>]
    let ``generated writable setters dispatch messages instead of mutating snapshots`` () =
        let host = SampleHost(createView "Before" "Ada" true)
        let messages = List<Msg>()

        host.SetDispatch(Action<Msg>(messages.Add))
        host.Child.Name <- "Grace"

        Assert.Equal<Msg list>([ SetName "Grace" ], Seq.toList messages)
        Assert.Equal("Ada", host.Child.Name)

    [<Fact>]
    let ``only explicitly registered write-back paths dispatch`` () =
        let host = SampleHost(createView "Before" "Ada" true)
        let messages = List<Msg>()

        host.SetDispatch(Action<Msg>(messages.Add))

        let dispatchedMapped = host.TryDispatchWriteBack("Child.Name", "Grace")
        let dispatchedUnmapped = host.TryDispatchWriteBack("Child.IsEnabled", true)

        Assert.True(dispatchedMapped)
        Assert.False(dispatchedUnmapped)
        Assert.Equal<Msg list>([ SetName "Grace" ], Seq.toList messages)

    [<Fact>]
    let ``write-back bindings expose the configured nested property path`` () =
        let host = SampleHost(createView "Before" "Ada" true)

        Assert.Equal<string list>([ "Child.Name" ], Seq.toList host.WriteBackBindings.Paths)

    [<Fact>]
    let ``snapshot updates notify root and nested generated properties`` () =
        let host = SampleHost(createView "Before" "Ada" true)
        let changed = ResizeArray<string>()

        (host :> INotifyPropertyChanged).PropertyChanged.Add(fun args -> changed.Add(args.PropertyName))
        (host.Child :> INotifyPropertyChanged).PropertyChanged.Add(fun args -> changed.Add($"Child.{args.PropertyName}"))

        host.Update(createView "After" "Grace" false)

        Assert.Equal<string list>(
            [ "View"; "Title"; "Child"; "Child.Name"; "Child.IsEnabled" ],
            Seq.toList changed)
