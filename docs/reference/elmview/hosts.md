# Generated hosts

The `GeneratedViewHost` classes provide the bindable root for ElmView. They
bridge immutable F# view records and the Avalonia `DataContext`.

## Bindable root

`BindableViewHost<'View>` exposes `View` and the `OnViewUpdated` hook, but it
has no dispatcher or write-back registry. `GeneratedViewHost` derives from it
and adds dispatch, generated-shaped property notifications, child-node
registration, and write-back routes. `RuntimeViewHost` and `DesignViewHost`
are the non-generated runtime and preview variants.

## API contracts

- <a href="/api/Elmish.Avalonia.Glue.ElmView.GeneratedViewHost`2.html">`Elmish.Avalonia.Glue.ElmView.GeneratedViewHost`2`</a>
- <a href="/api/Elmish.Avalonia.Glue.ElmView.RuntimeGeneratedViewHost`2.html">`Elmish.Avalonia.Glue.ElmView.RuntimeGeneratedViewHost`2`</a>
- <a href="/api/Elmish.Avalonia.Glue.ElmView.DesignGeneratedViewHost`2.html">`Elmish.Avalonia.Glue.ElmView.DesignGeneratedViewHost`2`</a>

## Implementation

- [ElmViewHosts.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Avalonia.Glue.ElmView/ElmViewHosts.fs)
