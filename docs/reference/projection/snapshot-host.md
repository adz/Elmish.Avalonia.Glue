# Snapshot host

`SnapshotHost<'T>` is a bindable shell for one immutable snapshot. It underpins **Snapshot-Host Projection**.

## `SnapshotHost<'T>`

### Members

| Member | Description |
| :--- | :--- |
| `Current` | Returns the current immutable snapshot. Binds in XAML to `Current`. |
| `Update(nextSnapshot)` | Inherited from `BindableSnapshotHost`. Updates the snapshot and raises `PropertyChanged` for `Current`. |

## Define a shallow host

To create a Snapshot-Host Projection, you inherit from `SnapshotHost<'T>` and add any commands or derived properties you need:

```csharp
public class UserProfileHost : SnapshotHost<UserProfile>
{
    private Action<Msg> _dispatch = _ => {};

    public UserProfileHost(UserProfile initial) : base(initial) {}

    public void SetDispatch(Action<Msg> dispatch) => _dispatch = dispatch;

    // Derived property
    public string FullName => $"{Current.FirstName} {Current.LastName}";

    // Called by the view's click handler; for Command binding expose an ICommand instead.
    public void Save() => _dispatch(Msg.SaveProfile);
}
```

In AXAML, bind to read-only snapshot values. Wire button clicks in code-behind to `host.Save()`, or expose an `ICommand` property for `Command` binding:

```xml
<StackPanel>
    <TextBlock Text="{Binding Current.FirstName}" />
    <TextBlock Text="{Binding Current.Email}" />
    <Button Content="Save" Click="OnSaveClick" />
</StackPanel>
```

`Update` notifies `Current`, not `FullName`. A binding to `FullName` needs an additional `PropertyChanged(nameof(FullName))` when the snapshot changes (override `OnSnapshotUpdated`). `Current.Email` is read-only for immutable records; expose a separate writable dispatching host property for `TwoWay` editing.

## Appropriate uses

- **Simplicity**: Much easier to write and maintain than a deep tree of manual projection classes.
- **Stability**: Provides a stable `DataContext` that Avalonia loves, while the data itself remains pure and immutable.
- **Named Contract**: Unlike raw ElmView, you have a named class where you can explicitly define the properties and commands exposed to the UI.

## Implementation

- [SnapshotHosts.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Avalonia.Glue.Projection/SnapshotHosts.fs)
