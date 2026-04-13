# Elmish.Avalonia.Glue

Minimal glue between an [Elmish](https://elmish.github.io/elmish/) program
and an Avalonia ViewModel. No Rx. No DynamicData. No new abstractions.

The repo now keeps the framework-neutral host/projection substrate in
`Elmish.Glue.Core`, while `Elmish.Avalonia.Glue` remains the Avalonia-facing
package that posts model updates onto Avalonia's UI thread and preserves the
existing surface for current samples.

## What it is

Two things:

1. `ElmishHost.start` / `ElmishHost.startAndBind` -- runs your Elmish loop,
   calls your ViewModel's `Update` function on the UI thread after every
   model change, and gives you a disposable host for lifetime management
2. `ObservableCollectionExtensions.SyncWith` -- patches an
   `ObservableCollection<T>` from an F# list without replacing it,
   preserving scroll position and selection

## What it is not

A framework. There is no required base class and no required command system.
For plain host wiring, the library only assumes your ViewModel has an
`Update` method and a way to receive dispatch.

If you want the projection helpers for child forwarding and collection sync,
implement `IProjection<'Model, 'Msg>` or the smaller `IProjection<'Model>` /
`IDispatchTarget<'Msg>` contracts.

Your F# model owns all state. Your ViewModel is a pure projection of that
state. Avalonia compiled bindings verify the projection at build time.

## Install

    dotnet add package Elmish.Avalonia.Glue

If you only need the framework-neutral host/projection primitives without the
Avalonia dispatcher integration, use `Elmish.Glue.Core`.

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

For lists, use `SyncWith` to keep `ObservableCollection<T>` instances stable:

    Log.SyncWith(
        model.Log,
        e => e.Id,
        vm => vm.Id,
        e => new LogEntryViewModel(e));

For child projections that inherit something else, such as
`CommunityToolkit.Mvvm.ComponentModel.ObservableObject`, implement
`IProjection<TModel, TMsg>` directly:

    public partial class CounterViewModel : ObservableObject, IProjection<CounterPage.Model, CounterPage.Msg>
    {
        private Action<CounterPage.Msg> _dispatch = _ => { };

        public void Update(CounterPage.Model model)
        {
            Count = model.Count;
            Log.SyncWith(model.Log, e => e.Id, vm => vm.Id, e => new LogEntryViewModel(e));
        }

        public void SetDispatch(Action<CounterPage.Msg> dispatch) => _dispatch = dispatch;
    }

`SyncWith` requires keys to be unique in both the source models and the target
collection. Duplicate keys fail fast instead of producing a mismatched
collection.

See the `sample/` directory for working examples:

- `Samples.Projection` is the current populated suite.
- `Samples.Projection/GlueSample` keeps the shape deliberately minimal.
- `Samples.Projection/OpsCenterSample` shows the same approach once the app
  grows into multiple pages, shared components, feature folders, and grouped
  projections.
- `Samples.Projection/GlueSample.CSharp` and
  `Samples.Projection/OpsCenterSample.CSharp` show the same projection pattern
  in code-only Avalonia views, without `.axaml` files.
- `Samples.ElmView` is reserved for the parallel ElmView sample suite.

## Why

The existing options (ReactiveElmish.Avalonia, Avalonia.FuncUI) either
require Rx/DynamicData or abandon AXAML entirely. This library does neither.
It is the 90 lines you would write yourself anyway, with a sample showing
exactly how to structure the surrounding code.

## License

MIT
