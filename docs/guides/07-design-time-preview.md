# Design-time preview

A preview is useful only when it exercises the same binding paths as runtime.
Treat design data as part of the feature, not a screenshot fixture.

## Create realistic immutable data

Define a design snapshot in F# with useful lengths, empty states, validation
states, and representative collection rows.

```fsharp isolated
type ProfileView =
    { Name: string
      Email: string
      Validation: string }

let designProfile =
    { Name = "Ada Lovelace"
      Email = "ada@example.test"
      Validation = "Ready to save" }
```

FsLiveDocs compiles this example. The sample value needs no Elmish runtime.

## Construct the runtime-shaped host

Give the design host the snapshot in its constructor. For ElmView, use the
same root and node properties as the runtime host. For Projection, populate
the same projection properties runtime updates.

The preview must not use different binding paths, special controls, or a
second viewmodel hierarchy.

## Preview states that expose layout problems

Include long text, validation messages, loading states, empty lists, and a few
rows. A single happy-path record rarely tests the layout you care about.

## Keep dispatch inactive

The designer should render without starting the full program. Commands may be
inert. Writable setters should not fail simply because no runtime dispatcher
has been attached.

The two sample suites link their design snapshots and hosts from
[sample applications](sample-applications.html).
