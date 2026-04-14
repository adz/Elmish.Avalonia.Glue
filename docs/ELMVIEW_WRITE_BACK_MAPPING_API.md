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

## Task 14 Decision

Write-back mappings are fully explicit in ElmView V2.

ElmView V2 will not infer write-back routes from property names, message-case
names, `SetX` naming, or any other host-side convention. If a property should
dispatch on write, the host must register that property path explicitly. If the
path is not registered, the property stays non-dispatching.

This decision keeps the interactive contract predictable:

- message routing stays reviewable in one host-local mapping block
- nested binding paths only dispatch when they were declared
- runtime and design-time behavior stay aligned without hidden fallback rules
- generated setters can fail closed for unmapped properties instead of guessing

Helper APIs may still reduce repetition later, but they must expand to the same
explicit property-path registrations rather than introducing implicit routing.
