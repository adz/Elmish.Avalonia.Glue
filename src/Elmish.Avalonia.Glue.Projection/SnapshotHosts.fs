namespace Elmish.Avalonia.Glue.Projection

open System
open System.Collections.Generic
open System.Collections.ObjectModel
open System.ComponentModel
open System.Runtime.CompilerServices

[<AbstractClass>]
type ObservableHost() =
    let propertyChanged = Event<PropertyChangedEventHandler, PropertyChangedEventArgs>()

    [<CLIEvent>]
    member _.PropertyChanged = propertyChanged.Publish

    member private _.RaisePropertyChanged(propertyName: string) =
        propertyChanged.Trigger(null, PropertyChangedEventArgs(propertyName))

    member this.Notify(propertyName: string) =
        this.RaisePropertyChanged(propertyName)

    interface INotifyPropertyChanged with
        [<CLIEvent>]
        member _.PropertyChanged = propertyChanged.Publish

type SnapshotHost<'T when 'T : not struct>(initial: 'T) =
    inherit ObservableHost()

    let mutable current = initial

    member _.Current = current

    member this.Update(next: 'T) =
        if not (obj.ReferenceEquals(current, next)) then
            current <- next
            this.Notify(nameof this.Current)

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
