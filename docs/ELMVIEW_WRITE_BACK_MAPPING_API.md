# ElmView Write-Back Mapping API

Task 13 defines the centralized mapping surface that ElmView V2 generation
should target for editable properties.

## Goal

Write-back routing should be declared once near host construction rather than
repeated in AXAML, code-behind event handlers, or F# view records.

The intended shape is:

```csharp
public sealed class AppHost : RuntimeGeneratedViewHost<AppView, Msg>
{
    public AppHost()
        : base(
            Core.App.getDesignView(),
            bindings => bindings
                .For(x => x.UserInput.Name).Dispatch(Msg.NewSetName)
                .For(x => x.UserInput.Email).Dispatch(Msg.NewSetEmail))
    {
    }
}
```

## Contract

- `WriteBackBindings<'View,'Msg>` owns the explicit property-path to message map.
- `bindings.For(...)` accepts a normal CLR property-path expression rooted at the
  immutable view snapshot type.
- `Dispatch(...)` registers the Elmish message factory for that property.
- Nested paths are recorded in ordinary dotted form, such as `UserInput.Name`.
- The generated host exposes the configured binding map so generated setters can
  route property writes through it later.

Task 13 only defines this mapping API and the host-side storage for it. Wiring
generated setters and `Mode=TwoWay` behavior into the sample remains in later
tasks.
