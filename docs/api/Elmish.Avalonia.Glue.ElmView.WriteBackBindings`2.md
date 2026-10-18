# Elmish.Avalonia.Glue.ElmView.WriteBackBindings`2

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
