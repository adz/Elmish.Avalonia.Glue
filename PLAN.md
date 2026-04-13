# Plan

## Current Goal

Turn the repository from a working prototype into a small but documented
library with explicit contracts.

## Near-Term Work

1. Lock down the public API surface.
2. Document intended usage for host lifetime, ViewModel shape, and list syncing.
3. Keep the sample aligned with the recommended usage pattern.
4. Preserve the "minimal glue" positioning instead of drifting into a framework.

## API Notes

- `ElmishHost.start` is the low-level entry point.
- `ElmishHost.startAndBind` is the convenience entry point for C# consumers.
- `ElmishHostConnection<'Msg>` owns the running loop and must be disposed by
  the app or view that started it.
- `ObservableCollectionExtensions.SyncWith` is intended for keyed projections
  where preserving existing ViewModel instances matters.

## Contracts To Keep

- Elmish remains the source of truth for application state.
- ViewModels stay as projections over Elmish models, not independent state
  containers.
- Host updates are marshalled onto Avalonia's UI thread.
- `SyncWith` fails fast when keys are not unique.

## Follow-Up Work

1. Decide whether to keep mutable dispatch setters in the sample ViewModels or
   replace them with a more explicit command adapter pattern.
2. Add package metadata expected for public release:
   repository URL, SourceLink, symbols, real author details.
3. Consider whether the sample belongs in the main solution for routine CI.
4. Add a few more behavior tests around host disposal ordering and dispatch
   after disposal.
