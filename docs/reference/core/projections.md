# Projection contracts

Core projection contracts connect named viewmodels to Elmish state and dispatch.

## API contracts

- <a href="/api/Elmish.Glue.Core.IProjection`1.html">`Elmish.Glue.Core.IProjection`1`</a>
- [`Elmish.Glue.Core.ProjectionExtensions`](/api/Elmish.Glue.Core.ProjectionExtensions.html)

## Appropriate uses

Use explicit projections when:

- Complex UI logic: model data needs significant transformation before binding.
- Interoperability: a mixed C#/F# codebase needs a C# viewmodel surface.
- Fine-grained control: specific UI components need explicit lifecycle or identity handling.


## Implementation

- [Projections.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Glue.Core/Projections.fs)
