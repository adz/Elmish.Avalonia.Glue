# Elmish.Glue.Core.BindableNode

## `BindableNode`

Base class for bindable objects with an `INotifyPropertyChanged` implementation.

### Members

| Member | Description |
| :--- | :--- |
| `PropertyChanged` | The standard `INotifyPropertyChanged` event. |
| `NotifyPropertyChanged(propertyName: string)` | Triggers the `PropertyChanged` event for the specified property. |
