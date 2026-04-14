namespace Elmish.Avalonia.Glue.Tests

open System
open System.Collections.Generic
open System.ComponentModel
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

    type private SampleHost(initialView: RootView) as this =
        inherit RuntimeGeneratedViewHost<RootView, Msg>(initialView)

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
            and set value = this.Dispatch(SetName value)

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
    let ``snapshot updates notify root and nested generated properties`` () =
        let host = SampleHost(createView "Before" "Ada" true)
        let changed = ResizeArray<string>()

        (host :> INotifyPropertyChanged).PropertyChanged.Add(fun args -> changed.Add(args.PropertyName))
        (host.Child :> INotifyPropertyChanged).PropertyChanged.Add(fun args -> changed.Add($"Child.{args.PropertyName}"))

        host.Update(createView "After" "Grace" false)

        Assert.Equal<string list>(
            [ "View"; "Title"; "Child"; "Child.Name"; "Child.IsEnabled" ],
            Seq.toList changed)
