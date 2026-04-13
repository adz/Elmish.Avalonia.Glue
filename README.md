# Elmish.Avalonia.Glue

Minimal glue between an [Elmish](https://elmish.github.io/elmish/) program
and an Avalonia ViewModel. No Rx. No DynamicData. No new abstractions.

## What it is

Two things:

1. `ElmishHost.start` / `ElmishHost.startAndBind` -- runs your Elmish loop,
   calls your ViewModel's `Update` function on the UI thread after every
   model change, and gives you a disposable host for lifetime management
2. `ObservableCollectionExtensions.SyncWith` -- patches an
   `ObservableCollection<T>` from an F# list without replacing it,
   preserving scroll position and selection

## What it is not

A framework. There is no base class to inherit, no interface to implement,
no opinion on your ViewModel beyond "it has an Update method".

Your F# model owns all state. Your ViewModel is a pure projection of that
state. Avalonia compiled bindings verify the projection at build time.

## Install

    dotnet add package Elmish.Avalonia.Glue

## Usage

Wire your program in one call and keep the returned host alive for as long as
the view or application owns the Elmish loop:

    // App.axaml.cs
    _host = ElmishHost.startAndBind(
        program,
        model => appVm.Update(model),
        appVm.SetDispatch);

Your ViewModel receives every model update on the UI thread:

    public void Update(App.Model model)
    {
        CounterVm.Update(model.Counter);
        // ...
    }

For lists, use SyncWith to keep ObservableCollection instances stable:

    Log.SyncWith(
        models:   model.Log,
        modelKey: e => e.Id,
        vmKey:    vm => vm.Id,
        create:   e => new LogEntryViewModel(e),
        update:   (vm, e) => vm.Update(e));

`SyncWith` requires keys to be unique in both the source models and the target
collection. It now fails fast when duplicates are present instead of silently
producing a mismatched collection.

See the `sample/` directory for a complete working example.

## Why

The existing options (ReactiveElmish.Avalonia, Avalonia.FuncUI) either
require Rx/DynamicData or abandon AXAML entirely. This library does neither.
It is the 90 lines you would write yourself anyway, with a sample showing
exactly how to structure the surrounding code.

## License

MIT
