namespace Elmish.Glue.Core

open System
open System.Collections.Generic
open System.Collections.ObjectModel
open System.ComponentModel

[<AbstractClass>]
type BindableNode() =
    let propertyChanged = Event<PropertyChangedEventHandler, PropertyChangedEventArgs>()

    [<CLIEvent>]
    member _.PropertyChanged = propertyChanged.Publish

    member this.NotifyPropertyChanged(propertyName: string) =
        propertyChanged.Trigger(this, PropertyChangedEventArgs(propertyName))

    interface INotifyPropertyChanged with
        [<CLIEvent>]
        member _.PropertyChanged = propertyChanged.Publish

[<AbstractClass>]
type BindableSnapshotHost<'Snapshot when 'Snapshot : not struct>
    (
        initialSnapshot: 'Snapshot,
        snapshotPropertyName: string
    ) =
    inherit BindableNode()

    let mutable currentSnapshot = initialSnapshot

    do
        if String.IsNullOrWhiteSpace snapshotPropertyName then
            invalidArg
                (nameof snapshotPropertyName)
                "Bindable snapshot hosts require a non-empty snapshot property name."

    member _.Snapshot = currentSnapshot

    abstract member OnSnapshotUpdated: previousSnapshot: 'Snapshot * nextSnapshot: 'Snapshot -> unit
    default _.OnSnapshotUpdated(_, _) = ()

    member this.Update(nextSnapshot: 'Snapshot) =
        if not (obj.ReferenceEquals(currentSnapshot, nextSnapshot)) then
            let previousSnapshot = currentSnapshot
            currentSnapshot <- nextSnapshot
            this.NotifyPropertyChanged(snapshotPropertyName)
            this.OnSnapshotUpdated(previousSnapshot, nextSnapshot)

type IBindableSnapshotNode =
    abstract RefreshSubtree: unit -> unit

[<AbstractClass>]
type BindableSnapshotNode<'RootSnapshot, 'NodeSnapshot when 'RootSnapshot : not struct>
    (
        getRootSnapshot: unit -> 'RootSnapshot,
        registerWithParent: IBindableSnapshotNode -> unit,
        getNodeSnapshot: 'RootSnapshot -> 'NodeSnapshot,
        propertyNames: seq<string>
    ) as this =
    inherit BindableNode()

    let children = ResizeArray<IBindableSnapshotNode>()
    let propertyNames = propertyNames |> Seq.toArray

    do registerWithParent (this :> IBindableSnapshotNode)

    new
        (
            getRootSnapshot: Func<'RootSnapshot>,
            registerWithParent: Action<IBindableSnapshotNode>,
            getNodeSnapshot: Func<'RootSnapshot, 'NodeSnapshot>,
            propertyNames: System.Collections.IEnumerable
        ) =
        BindableSnapshotNode(
            getRootSnapshot.Invoke,
            registerWithParent.Invoke,
            getNodeSnapshot.Invoke,
            propertyNames |> Seq.cast<string>)

    member _.Snapshot = getNodeSnapshot (getRootSnapshot())

    member _.RegisterChildNode(child: IBindableSnapshotNode) =
        children.Add(child)

    member private this.RefreshSelf() =
        for propertyName in propertyNames do
            this.NotifyPropertyChanged(propertyName)

    member private this.RefreshSubtreeCore() =
        this.RefreshSelf()

        for child in children do
            child.RefreshSubtree()

    interface IBindableSnapshotNode with
        member this.RefreshSubtree() =
            this.RefreshSubtreeCore()

type KeyedSnapshotCollection<'T, 'Key when 'T : not struct and 'Key : equality and 'Key : not null>
    (
        keySelector: Func<'T, 'Key>
    ) =
    let items = ObservableCollection<'T>()
    let mutable lastSnapshot : IReadOnlyList<'T> option = None

    member _.Items = items

    member _.Update(next: IReadOnlyList<'T>) =
        match lastSnapshot with
        | Some previous when obj.ReferenceEquals(previous, next) -> ()
        | _ ->
            let targetKeys = HashSet<'Key>()
            for i in 0 .. next.Count - 1 do
                let key = keySelector.Invoke(next[i])
                if not (targetKeys.Add(key)) then
                    invalidArg (nameof next) $"KeyedSnapshotCollection requires unique keys. Duplicate key '{key}' was found."

            let existing = Dictionary<'Key, 'T>()
            for i in items.Count - 1 .. -1 .. 0 do
                let item = items[i]
                let key = keySelector.Invoke(item)
                existing[key] <- item
                if not (targetKeys.Contains(key)) then
                    items.RemoveAt(i)

            for i in 0 .. next.Count - 1 do
                let nextItem = next[i]
                let key = keySelector.Invoke(nextItem)

                match existing.TryGetValue(key) with
                | true, currentItem ->
                    let currentIndex = items.IndexOf(currentItem)
                    if not (obj.ReferenceEquals(currentItem, nextItem)) then
                        items[currentIndex] <- nextItem

                    let updatedItem =
                        if currentIndex < items.Count then items[currentIndex]
                        else nextItem

                    let updatedIndex = items.IndexOf(updatedItem)
                    if updatedIndex <> i then
                        items.Move(updatedIndex, i)
                | false, _ ->
                    items.Insert(i, nextItem)

            lastSnapshot <- Some next
