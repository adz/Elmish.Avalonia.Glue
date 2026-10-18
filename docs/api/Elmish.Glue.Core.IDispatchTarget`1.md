# Dispatch target

`SetDispatch(Action<Msg>)` supplies the callback used for user edits and commands. Connect before invoking it; dispatch through the host on the UI thread for Avalonia-bound state.
