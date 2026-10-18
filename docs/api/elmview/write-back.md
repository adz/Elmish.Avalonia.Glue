# Write-back bindings

`WriteBackBindings<'View, 'Msg>` is a registry for editable properties.

It does not replace Avalonia bindings. AXAML declares the binding. The
registry says what message to dispatch when Avalonia writes to a generated
property through a `TwoWay` binding.

**If the snapshot is immutable, where does a `TextBox` write go?**

It goes to a generated setter. The setter asks this registry for the matching
Elmish message. The Elmish `update` function produces the next snapshot.

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

## Register editable paths

Configure these bindings in the host constructor or in a static configuration
block:

```csharp
private static readonly Action<WriteBackBindings<AppView, Msg>> ConfigureBindings =
    bindings =>
    {
        bindings.For(x => x.UserInput.Name).Dispatch(Msg.NewSetName);
        bindings.For(x => x.UserInput.Newsletter).Dispatch(Msg.NewSetNewsletter);
        bindings.For(x => x.UserInput.Experience).Dispatch(Msg.NewSetExperience);
    };
```

Those routes correspond to AXAML bindings:

```xml
<TextBox Text="{Binding UserInput.Name, Mode=TwoWay}" />
<CheckBox IsChecked="{Binding UserInput.Newsletter, Mode=TwoWay}" />
<Slider Value="{Binding UserInput.Experience, Mode=TwoWay}" />
```

The AXAML path and the write-back selector name the same property. The
AXAML line controls Avalonia binding behavior. The registry line controls
Elmish message routing.

## Dispatch sequence

1. `For` reads the expression tree and stores a path such as
   `"UserInput.Name"`.
2. A generated setter calls `host.TryDispatchWriteBack("UserInput.Name",
   value)`.
3. The host finds the registered route.
4. The route maps the new value to a message.
5. The host dispatches that message into Elmish.

The handwritten generated-shaped setter does not mutate the snapshot. `Host` in this sketch means a stored reference to the root host; see the [node example](nodes.html).

```csharp
public string Name
{
    get => Snapshot.Name;
    set => Host.TryDispatchWriteBack("UserInput.Name", value);
}
```

The Elmish update function applies the state change.

## Editable and display-only values

Use write-back for generated properties that Avalonia writes to:

- `TextBox.Text`
- `CheckBox.IsChecked`
- `ComboBox.SelectedItem`
- `ComboBox.SelectedIndex`
- `Slider.Value`
- other scalar editable control properties

Display-only values do not need write-back:

```xml
<TextBlock Text="{Binding UserInput.ValidationText}" />
<ItemsControl ItemsSource="{Binding RandomDice.History}" />
```

Commands and events can stay as small explicit host methods:

```xml
<Button Click="OnRefreshHttpClick" />
```

```csharp
public void RefreshHttp() => Dispatch(Msg.RefreshHttp);
```

## Centralized routing contract

- One place lists the editable UI paths.
- A C# `For` selector's member accesses are type-checked against the view type at compile time; only simple member-access expression trees are accepted at runtime. `ForPath` strings are not type-checked. Duplicate paths throw `InvalidOperationException` at registration; `TryDispatchWriteBack` returns `false` for an unmatched path and otherwise invokes the mapper and dispatcher (mapper/type errors propagate).
- AXAML stays standard.
- F# view records do not need binding attributes or metadata.
- Common form controls avoid handwritten event-forwarding code.

ElmView keeps the F# record plain, AXAML standard, and the bridge code in the
host.

## Implementation

- [ElmViewHosts.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Avalonia.Glue.ElmView/ElmViewHosts.fs)
