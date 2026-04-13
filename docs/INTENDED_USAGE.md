# Intended Usage

## Positioning

`Elmish.Avalonia.Glue` is meant for projects that want all application state to
live in Elmish while still using ordinary Avalonia ViewModels and AXAML.

It is intentionally narrow. The library is not trying to provide a new MVVM
base class, a reactive abstraction layer, or an alternate view DSL.

## Ownership Model

The F# Elmish program owns the real state.

The C# or F# ViewModel layer is a projection over that state:

- scalar properties mirror model values
- child ViewModels mirror nested model records
- observable collections mirror keyed model lists

The ViewModel layer may hold UI-only mechanics such as:

- stable `ObservableCollection<T>` instances
- imperative button handlers or command forwarding
- cached child ViewModel objects that correspond to stable model identities

It should not become a second source of truth for domain state.

## Starting The Host

The intended lifetime is:

1. Build the Elmish program in the core layer.
2. Construct the root ViewModel in the UI layer.
3. Start the host from the app, window, or view owner.
4. Hold onto the returned `ElmishHostConnection<'Msg>`.
5. Dispose that connection when the owner shuts down.

The host is responsible for:

- running the Elmish loop
- forwarding model updates onto Avalonia's UI thread
- exposing a stable dispatcher for the ViewModel layer
- terminating Elmish subscriptions when disposed

`ElmishHost.start` is the low-level API when the caller wants to wire the
dispatcher manually.

`ElmishHost.startAndBind` is the convenience API when the caller wants a single
call that both starts the loop and hands the dispatcher to a consumer.

## Dispatch Flow

The intended direction of flow is:

1. user interacts with the view
2. view or ViewModel invokes a dispatch-facing method
3. that method emits an Elmish message
4. Elmish updates the model
5. the host posts the new model to the UI thread
6. the ViewModel updates its projection

This keeps mutation localized to the projection layer and leaves behavior and
state transitions in Elmish.

## ViewModel Shape

The library assumes a plain ViewModel style. A typical root ViewModel will:

- expose child ViewModels as properties
- expose an `Update(model)` method
- expose a way to receive the dispatch action

A child ViewModel will typically:

- update scalar properties from a child model
- forward user actions as parent messages
- own its own stable `ObservableCollection<T>` instances where needed

The library does not require a specific base type or command abstraction.

## Collection Synchronization

`ObservableCollectionExtensions.SyncWith` is intended for model lists with
stable keys.

Use it when you need to:

- preserve existing ViewModel instances
- preserve selection, scroll position, and container reuse
- update, insert, remove, and reorder items without replacing the collection

Callers are expected to provide:

- the target `ObservableCollection<TViewModel>`
- the source model list
- a model key selector
- a ViewModel key selector
- a factory for new ViewModel instances
- an update function for existing ViewModel instances

## Key Requirements

`SyncWith` assumes keys are unique on both sides:

- every model item must have a unique key
- every existing ViewModel in the target collection must have a unique key

Duplicate keys are treated as invalid input and now fail fast.

That behavior is intentional. Silent recovery would hide projection bugs and
make the resulting UI state hard to reason about.

## When This Library Fits

This library is a good fit when:

- Elmish already owns application behavior
- the UI team wants to keep AXAML and compiled bindings
- the project wants minimal dependencies and explicit wiring
- ViewModels are used as projections rather than state machines

## When It Does Not Fit

This library is probably the wrong fit when:

- the UI layer is expected to own substantial independent state
- the project wants a reactive collection framework
- the project wants a higher-level Avalonia + Elmish integration with more
  built-in conventions
- the team does not want to manage host lifetime explicitly
