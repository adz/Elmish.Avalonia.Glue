# Write-back bindings

`WriteBackBindings<'View, 'Msg>` is a registry for editable properties.

It does not replace Avalonia bindings. AXAML declares the binding. The
registry says what message to dispatch when Avalonia writes to a generated
property through a `TwoWay` binding.

For registration, AXAML and dispatch in context, see [route editable ElmView properties](../../guides/route-editable-elmview-properties.html).

## `WriteBackBindings<'View, 'Msg>`

### Members

| Member | Description |
| :--- | :--- |
| `For<'Value>(selector)` | Accepts an expression-tree chain of member accesses rooted at the view parameter (e.g., `x => x.UserInput.Name`), optionally with conversions; rejects method calls, indexers and other expressions with `ArgumentException`, or null with `ArgumentNullException`. Returns a registration object. |
| `ForPath<'Value>(path)` | Takes the exact CLR facade path as a string for F# callers; does not validate that members exist or that the value type matches. An empty/whitespace path fails when `Dispatch` registers it. |

### `WriteBackBindingRegistration`

| Member | Description |
| :--- | :--- |
| `Dispatch(map)` | Completes the registration by providing a function that takes the new property value and returns a message to dispatch. |

## Route failure semantics

A C# `For` selector accepts only simple member-access expression trees; `ForPath` strings are not type-checked. Duplicate paths throw `InvalidOperationException` at registration; `TryDispatchWriteBack` returns `false` for an unmatched path and otherwise invokes the mapper and dispatcher (mapper/type errors propagate). Avalonia may invoke a `TwoWay` setter at initialization; the registry does not deduplicate writes.

## Implementation

- [ElmViewHosts.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Avalonia.Glue.ElmView/ElmViewHosts.fs)
