namespace Elmish.Avalonia.Glue.Tests

open System
open System.ComponentModel
open Elmish.Glue.Core
open Xunit

module SnapshotSubstrateTests =

    type private RootSnapshot =
        {
            Title: string
            Child: ChildSnapshot
        }

    and private ChildSnapshot =
        {
            Name: string
            Enabled: bool
        }

    type private SampleHost(initialSnapshot: RootSnapshot) =
        inherit BindableSnapshotHost<RootSnapshot>(initialSnapshot, "Current")

        override _.OnSnapshotUpdated(_, nextSnapshot) =
            base.NotifyPropertyChanged(nameof Unchecked.defaultof<SampleHost>.Title)

        member this.Current = this.Snapshot
        member this.Title = this.Snapshot.Title

    type private SampleChildNode(host: SampleHost) =
        inherit BindableSnapshotNode<RootSnapshot, ChildSnapshot>(
            (fun () -> host.Current),
            ignore,
            (fun root -> root.Child),
            [ nameof Unchecked.defaultof<ChildSnapshot>.Name
              nameof Unchecked.defaultof<ChildSnapshot>.Enabled ])

        member this.Name = this.Snapshot.Name
        member this.Enabled = this.Snapshot.Enabled

    let private createSnapshot title childName enabled =
        {
            Title = title
            Child =
                {
                    Name = childName
                    Enabled = enabled
                }
        }

    [<Fact>]
    let ``bindable snapshot host raises its configured snapshot property and derived properties`` () =
        let host = SampleHost(createSnapshot "Before" "Ada" true)
        let changed = ResizeArray<string>()

        (host :> INotifyPropertyChanged).PropertyChanged.Add(fun args -> changed.Add(args.PropertyName))

        host.Update(createSnapshot "After" "Grace" false)

        Assert.Equal<string list>([ "Current"; "Title" ], Seq.toList changed)
        Assert.Equal("After", host.Title)

    [<Fact>]
    let ``bindable snapshot host skips notifications for the same snapshot reference`` () =
        let initialSnapshot = createSnapshot "Before" "Ada" true
        let host = SampleHost(initialSnapshot)
        let changed = ResizeArray<string>()

        (host :> INotifyPropertyChanged).PropertyChanged.Add(fun args -> changed.Add(args.PropertyName))

        host.Update(initialSnapshot)

        Assert.Empty(changed)

    [<Fact>]
    let ``bindable snapshot nodes stay stable and refresh their declared properties`` () =
        let host = SampleHost(createSnapshot "Before" "Ada" true)
        let child = SampleChildNode(host)
        let changed = ResizeArray<string>()

        (child :> INotifyPropertyChanged).PropertyChanged.Add(fun args -> changed.Add(args.PropertyName))

        host.Update(createSnapshot "After" "Grace" false)
        (child :> IBindableSnapshotNode).RefreshSubtree()

        Assert.Equal<string list>([ "Name"; "Enabled" ], Seq.toList changed)
        Assert.Equal("Grace", child.Name)
        Assert.False(child.Enabled)
