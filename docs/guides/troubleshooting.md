# Troubleshooting

| Symptom | Check first | Next step |
| :--- | :--- | :--- |
| TextBox shows stale text | Does `Name` notify after a new snapshot, and is its TwoWay setter dispatching? | Keep the `DataContext` stable; list `Name` in `GeneratedPropertyNames` or notify in a handwritten projection. See [write-back](route-editable-elmview-properties.html). |
| Missing `PropertyChanged` | Did `Update` receive the same snapshot reference? Is the binding path a nested node? | Supply a new immutable snapshot and refresh stable nodes; see [node contract](../api/Elmish.Avalonia.Glue.ElmView.GeneratedViewNode`3.html). |
| Preview crashes | Does a setter or command assume runtime dispatch is connected? | Provide a design snapshot and safe no-op dispatch; do not start Elmish in preview. See [design preview](design-time-preview.html). |
| Lost selection | Did a matching key receive a new snapshot object? | Track selected key and resolve it after patching, or use a stable row projection; see [collections](../reference/core/collections.html). |
| Cross-thread exception | Was a UI-bound host updated by a background callback directly? | Dispatch a message via the Avalonia bridge and let it publish on the UI thread. See [threading](threading-dispatch.html). |
| Duplicate key or write-back path | Check incoming key uniqueness and registered paths before publishing. | Duplicate routes fail at registration; inspect the exception and ensure each path is registered once. See [collections](../api/Elmish.Glue.Core.ObservableCollectionExtensions.html) and [write-back](route-editable-elmview-properties.html). |
| Dispatch after shutdown | Is the connection disposed or the UI dispatcher already stopped? | Stop producers, dispose the connection with the view and keep the UI loop alive through teardown. See [lifetime](connect-runtime.html). |
| Async result overwrites newer state | Did work finish after a later request or disposal? | Include a request identity in messages and ignore results for obsolete requests in `update`. See [background work](background-commands.html). |

Each row above has a longer, verified recipe in [error handling](error-handling.html).
