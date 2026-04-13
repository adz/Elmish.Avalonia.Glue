namespace Elmish.Avalonia.Glue.Tests

open System
open System.Collections.ObjectModel
open Elmish.Glue.Core
open Xunit

type private TestVm(id : int, value : string) =
    member val Id = id
    member val Value = value with get, set

type private TestModel =
    {
        Id : int
        Value : string
    }

module ObservableCollectionExtensionsTests =

    [<Fact>]
    let ``SyncWith rejects duplicate model keys`` () =
        let collection = ObservableCollection<TestVm>()
        let models =
            [|
                { Id = 1; Value = "a" }
                { Id = 1; Value = "b" }
            |]

        let ex =
            Assert.Throws<ArgumentException>(fun () ->
                ObservableCollectionExtensions.SyncWith(
                    collection,
                    models,
                    Func<TestModel, int>(fun model -> model.Id),
                    Func<TestVm, int>(fun vm -> vm.Id),
                    Func<TestModel, TestVm>(fun model -> TestVm(model.Id, model.Value)),
                    Action<TestVm, TestModel>(fun vm model -> vm.Value <- model.Value)))

        Assert.Contains("Duplicate key '1'", ex.Message)

    [<Fact>]
    let ``SyncWith rejects duplicate view-model keys`` () =
        let collection =
            ObservableCollection<TestVm>(
                [|
                    TestVm(1, "a")
                    TestVm(1, "b")
                |])

        let models = [| { Id = 1; Value = "updated" } |]

        let ex =
            Assert.Throws<InvalidOperationException>(fun () ->
                ObservableCollectionExtensions.SyncWith(
                    collection,
                    models,
                    Func<TestModel, int>(fun model -> model.Id),
                    Func<TestVm, int>(fun vm -> vm.Id),
                    Func<TestModel, TestVm>(fun model -> TestVm(model.Id, model.Value)),
                    Action<TestVm, TestModel>(fun vm model -> vm.Value <- model.Value)))

        Assert.Contains("Duplicate key '1'", ex.Message)

    [<Fact>]
    let ``SyncWith reuses existing instances when keys are stable`` () =
        let existing = TestVm(2, "old")
        let collection =
            ObservableCollection<TestVm>(
                [|
                    TestVm(1, "drop")
                    existing
                |])

        let models =
            [|
                { Id = 2; Value = "updated" }
                { Id = 3; Value = "new" }
            |]

        ObservableCollectionExtensions.SyncWith(
            collection,
            models,
            Func<TestModel, int>(fun model -> model.Id),
            Func<TestVm, int>(fun vm -> vm.Id),
            Func<TestModel, TestVm>(fun model -> TestVm(model.Id, model.Value)),
            Action<TestVm, TestModel>(fun vm model -> vm.Value <- model.Value))

        Assert.Equal(2, collection.Count)
        Assert.Same(existing, collection[0])
        Assert.Equal("updated", collection[0].Value)
        Assert.Equal(3, collection[1].Id)
