# ProjectionNode

A bindable base for explicit projections. `Change` compares old and new values using F# equality, updates the field and raises `PropertyChanged` for the supplied name only when changed. `PropertyChanged` uses the calling thread; marshal UI-bound updates through the Avalonia host.
