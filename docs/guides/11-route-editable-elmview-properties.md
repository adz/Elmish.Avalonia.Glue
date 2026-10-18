# Route editable ElmView properties

Register each writable facade path so a standard AXAML TwoWay edit sends an Elmish message instead of mutating an immutable snapshot. The [write-back API](../reference/elmview/write-back.html) documents members and route failures.

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

The handwritten generated-shaped setter does not mutate the snapshot. `Host` in this sketch means a stored reference to the root host; see the [nested-path recipe](nested-elmview-paths.html).

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

