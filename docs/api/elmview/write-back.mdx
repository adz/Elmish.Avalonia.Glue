---
sidebar_position: 3
---

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
| `For<'Value>(selector)` | Starts a registration for the property identified by the expression selector (e.g., `x => x.UserInput.Name`). Returns a registration object. |

### `WriteBackBindingRegistration`

| Member | Description |
| :--- | :--- |
| `Dispatch(map)` | Completes the registration by providing a function that takes the new property value and returns a message to dispatch. |

## Usage Example

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

## How it works

1. `For` reads the expression tree and stores a path such as
   `"UserInput.Name"`.
2. A generated setter calls `host.TryDispatchWriteBack("UserInput.Name",
   value)`.
3. The host finds the registered route.
4. The route maps the new value to a message.
5. The host dispatches that message into Elmish.

The generated setter does not mutate the snapshot.

```csharp
public string Name
{
    get => Snapshot.Name;
    set => Host.TryDispatchWriteBack("UserInput.Name", value);
}
```

The Elmish update function applies the state change.

## What Can Use Write-Back

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

## Why Use This Registry

- One place lists the editable UI paths.
- The selector is type-checked against the F# view record.
- AXAML stays standard.
- F# view records do not need binding attributes or metadata.
- Common form controls avoid handwritten event-forwarding code.

ElmView keeps the F# record plain, AXAML standard, and the bridge code in the
host.

## Source

- [ElmViewHosts.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Avalonia.Glue.ElmView/ElmViewHosts.fs)
