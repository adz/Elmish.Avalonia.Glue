namespace Elmish.Glue.Core

open System
open System.Collections.Generic
open System.Collections.ObjectModel

[<RequireQualifiedAccess>]
module internal KeyedCollectionPatching =
    let private shiftIndexesFrom (indexesByKey: Dictionary<'Key, int>) startIndex delta =
        let keys = ResizeArray<'Key>()

        for KeyValue(key, index) in indexesByKey do
            if index >= startIndex then
                keys.Add(key)

        for key in keys do
            indexesByKey[key] <- indexesByKey[key] + delta

    let private updateIndexesForMove (indexesByKey: Dictionary<'Key, int>) fromIndex toIndex movedKey =
        if fromIndex < toIndex then
            let keys = ResizeArray<'Key>()

            for KeyValue(key, index) in indexesByKey do
                if index > fromIndex && index <= toIndex then
                    keys.Add(key)

            for key in keys do
                indexesByKey[key] <- indexesByKey[key] - 1
        else
            let keys = ResizeArray<'Key>()

            for KeyValue(key, index) in indexesByKey do
                if index >= toIndex && index < fromIndex then
                    keys.Add(key)

            for key in keys do
                indexesByKey[key] <- indexesByKey[key] + 1

        indexesByKey[movedKey] <- toIndex

    let patch<'Current, 'Target, 'Key when 'Key: equality and 'Key: not null>
        (
            collection: ObservableCollection<'Current>,
            currentKey: 'Current -> 'Key,
            targets: IReadOnlyList<'Target>,
            targetKey: 'Target -> 'Key,
            onMissingTarget: 'Key -> unit,
            updateExisting: 'Current -> 'Target -> 'Current,
            create: 'Target -> 'Current,
            duplicateTargetMessage: 'Key -> string,
            duplicateCurrentMessage: 'Key -> string
        ) : unit =

        let targetKeys = HashSet<'Key>()
        for i in 0 .. targets.Count - 1 do
            let key = targetKey targets[i]
            if not (targetKeys.Add(key)) then
                invalidArg (nameof targets) (duplicateTargetMessage key)

        let indexesByKey = Dictionary<'Key, int>()

        for i in collection.Count - 1 .. -1 .. 0 do
            let item = collection[i]
            let key = currentKey item

            if indexesByKey.ContainsKey(key) then
                invalidOp (duplicateCurrentMessage key)

            indexesByKey[key] <- i

            if not (targetKeys.Contains(key)) then
                onMissingTarget key
                collection.RemoveAt(i)

        let ordered = ResizeArray<'Current>(collection)
        indexesByKey.Clear()

        for i in 0 .. ordered.Count - 1 do
            indexesByKey[currentKey ordered[i]] <- i

        for i in 0 .. targets.Count - 1 do
            let target = targets[i]
            let key = targetKey target

            match indexesByKey.TryGetValue(key) with
            | true, currentIndex ->
                let currentItem = ordered[currentIndex]
                let updatedItem = updateExisting currentItem target

                if not (obj.ReferenceEquals(currentItem, updatedItem)) then
                    collection[currentIndex] <- updatedItem
                    ordered[currentIndex] <- updatedItem

                if currentIndex <> i then
                    collection.Move(currentIndex, i)
                    ordered.RemoveAt(currentIndex)
                    ordered.Insert(i, updatedItem)
                    updateIndexesForMove indexesByKey currentIndex i key
            | false, _ ->
                let created = create target
                collection.Insert(i, created)
                ordered.Insert(i, created)
                shiftIndexesFrom indexesByKey i 1
                indexesByKey[key] <- i
