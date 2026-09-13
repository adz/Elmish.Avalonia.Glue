---
sidebar_position: 6
---

# Post-V2 comparison

ElmView V2 gives the repository two comparable authoring families. Both are
implemented against the same sample matrix.

## 1. Boilerplate and Code Volume

The main difference is the amount and location of glue code.

- **Projection**: Requires a deep tree of mutable viewmodel classes. For a typical form, you must manually define properties, handle `OnPropertyChanged` events to dispatch messages, and use an `_isUpdating` flag to suppress feedback loops during model updates.
- **ElmView**: Moves most glue into a mechanical host class. Manual property-change suppression and event-handler boilerplate disappear from the form path. Authored F# records serve as the primary UI schema.

| Feature | Projection | ElmView |
| :--- | :--- | :--- |
| **Form Property** | ~10 lines (Property + Event Handler) | 1 line (F# Record Field) |
| **List Sync** | Manual `SyncWith` calls | Automatic refresh propagation |
| **Message Routing** | Scattered through viewmodels | Centralized `WriteBackBindings` |

## 2. Reviewability

- **Projection**: A UI change often requires reviewing changes in three places: the F# model, the C# viewmodel, and the AXAML. The logic is spread across different paradigms (immutable vs. mutable).
- **ElmView**: Visual changes are primarily reviewed in the F# records and AXAML. The host facade is mechanical and usually changes only when the binding shape changes.

## 3. Design-Time Quality

Both families support design-time preview through different paths:

- **Projection**: Relies on a `PreviewHost` that populates the projection tree with design data. This works well but requires maintaining the design data in the projection layer.
- **ElmView**: Uses the same snapshots and host shape for design-time and runtime. `DesignGeneratedViewHost` renders the runtime binding shape with design data.

## 4. LLM Ergonomics

- **Projection**: Boilerplate and mutable transition code create more places for generated code to drift from the pattern.
- **ElmView**: The generated-host shape and F# records give generated code a smaller pattern to follow.

## Summary: When To Choose

| Choose **Projection** if... | Choose **ElmView** if... |
| :--- | :--- |
| You have a dedicated C# UI team that wants named viewmodels. | You want less handwritten binding glue. |
| You need fine-grained control over row identity and selection. | You want F# records to be your single source of truth for UI shape. |
| You are building a command-heavy desktop app. | You want an Elm-like authoring loop in Avalonia. |

## Source Evidence

- [Projection boilerplate (500+ lines)](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/sample/Samples.Projection/ExampleMatrixSample/src/ExampleMatrixSample.UI/ViewModels/AppProjection.cs)
- [ElmView mechanical host](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/sample/Samples.ElmView/ExampleMatrixSample/src/ExampleMatrixSample.UI/Views/AppHost.cs)
- [ElmView F# UI schema](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/sample/Samples.ElmView/ExampleMatrixSample/src/ExampleMatrixSample.Core/App.fs)
