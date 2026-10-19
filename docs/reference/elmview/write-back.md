# Write-back bindings

`WriteBackBindings<'View, 'Msg>` is a registry for editable properties.

It does not replace Avalonia bindings. AXAML declares the binding. The
registry says what message to dispatch when Avalonia writes to a generated-shaped
property through a `TwoWay` binding.

For registration, AXAML and dispatch in context, see [route editable ElmView properties](../../guides/route-editable-elmview-properties.html).

## API contracts

- <a href="/api/Elmish.Avalonia.Glue.ElmView.WriteBackBindings`2.html">`Elmish.Avalonia.Glue.ElmView.WriteBackBindings`2`</a>
- <a href="/api/Elmish.Avalonia.Glue.ElmView.WriteBackBindingRegistration`3.html">`Elmish.Avalonia.Glue.ElmView.WriteBackBindingRegistration`3`</a>

## Implementation

- [ElmViewHosts.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Avalonia.Glue.ElmView/ElmViewHosts.fs)
