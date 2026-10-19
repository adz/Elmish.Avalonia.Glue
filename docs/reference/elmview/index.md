# Elmish.Avalonia.Glue.ElmView

> Maturity: **Experimental** · [package maturity](/guides/package-maturity.html)

ElmView exposes immutable F# view records through a stable bindable host.
AXAML remains normal and editable properties dispatch messages.

## Assemble an ElmView host

1. Derive a root F# view record from the application model.
2. Inherit from `RuntimeGeneratedViewHost<TView,TMsg>`; see [generated hosts](hosts.html).
3. Expose nested records through `GeneratedViewNode` instances; see [nested nodes](nodes.html).
4. List the properties each host or node must notify.
5. Register each writable path with `WriteBackBindings`; see [write-back bindings](write-back.html).
6. Use the matching design host shape with sample data.

The current package supplies the base types and routing mechanics. The sample
host is handwritten in a mechanical, generator-friendly shape; no source
generator is shipped yet.
