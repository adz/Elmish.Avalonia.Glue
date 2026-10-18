# Elmish.Avalonia.Glue.ElmView.RuntimeGeneratedViewHost`2

## Runtime and design-time variants

ElmView provides host types for runtime and design-time while preserving one
bindable shape.

### `RuntimeGeneratedViewHost<'View, 'Msg>`

Used when the application is running. It is started and bound through
`ElmishHost.startAndBind`.

### `DesignGeneratedViewHost<'View, 'Msg>`

Used for Avalonia design-time preview. It is initialized with a sample
snapshot so the designer can render the same bindable shape used at runtime.
