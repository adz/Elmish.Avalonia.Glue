---
sidebar_position: 2
---

# Snapshot host

The `SnapshotHost<'T>` is a lightweight, bindable shell for a single immutable snapshot. It's the primary building block for the **Snapshot-Host Projection** style.

## `SnapshotHost<'T>`

### Members

| Member | Description |
| :--- | :--- |
| `Current` | Returns the current immutable snapshot. Binds in XAML to `Current`. |
| `Update(nextSnapshot)` | Inherited from `BindableSnapshotHost`. Updates the snapshot and raises `PropertyChanged` for `Current`. |

## How to use it

To create a Snapshot-Host Projection, you inherit from `SnapshotHost<'T>` and add any commands or derived properties you need:

```csharp
public class UserProfileHost : SnapshotHost<UserProfile>
{
    private Action<Msg> _dispatch = _ => {};

    public UserProfileHost(UserProfile initial) : base(initial) {}

    public void SetDispatch(Action<Msg> dispatch) => _dispatch = dispatch;

    // Derived property
    public string FullName => $"{Current.FirstName} {Current.LastName}";

    // Command
    public void Save() => _dispatch(Msg.SaveProfile);
}
```

In your AXAML, you can then bind directly to the snapshot or the host's properties:

```xml
<StackPanel>
    <TextBlock Text="{Binding FullName}" />
    <TextBlock Text="{Binding Current.Email}" />
    <Button Content="Save" Command="{Binding Save}" />
</StackPanel>
```

## Why use SnapshotHost?

- **Simplicity**: Much easier to write and maintain than a deep tree of manual projection classes.
- **Stability**: Provides a stable `DataContext` that Avalonia loves, while the data itself remains pure and immutable.
- **Named Contract**: Unlike raw ElmView, you have a named class where you can explicitly define the properties and commands exposed to the UI.

## Source

- [SnapshotHosts.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Avalonia.Glue.Projection/SnapshotHosts.fs)
