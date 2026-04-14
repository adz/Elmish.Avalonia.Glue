# Post-V2 Architecture Comparison

This note closes task 27 by re-evaluating `ElmView` against `Projection`
after ElmView V2 landed.

The comparison is based on the two parallel `ExampleMatrixSample` suites,
because they implement the same feature matrix:

- static layout / HTML-equivalent
- user input / forms
- random / dice
- HTTP
- time / clock
- files
- SVG-equivalent composition

The numbers below are taken from the current repository state on
2026-04-15.

## Measurement Scope

The most useful apples-to-apples comparison points are:

- `sample/Samples.Projection/ExampleMatrixSample`
- `sample/Samples.ElmView/ExampleMatrixSample`
- `tests/Elmish.Avalonia.Glue.Tests/SnapshotSubstrateTests.fs`
- `tests/Elmish.Avalonia.Glue.Tests/ElmViewGeneratedHostTests.fs`

The code-count measurements use the current top-level sample files:

- `Projection`:
  `App.fs`, `AppProjection.cs`, `AppView.axaml`
- `ElmView`:
  `App.fs`, `AppHost.cs`, `AppView.axaml`

These counts are not a claim that every line is purely architectural. They do
show where each approach currently places authored complexity.

## Measured Comparison

| Measure | Projection | ElmView V2 | Reading |
| --- | ---: | ---: | --- |
| F# app/view file lines | 630 | 699 | ElmView keeps more UI-shaped detail in F# view records and view-building code. |
| C# bindable glue lines | 489 | 278 | ElmView removes 211 lines of handwritten CLR-facing glue in this sample, about 43% less than Projection. |
| AXAML lines | 387 | 501 | ElmView keeps more page structure visible in AXAML instead of moving it behind projection-only viewmodel seams. |
| Form editable wiring points | 6 `On...Changed` partial methods plus `_isUpdating` loop suppression | 6 centralized `bindings.For(...).Dispatch(...)` registrations | ElmView V2 makes editable-field routing more centralized and easier to audit. |
| Design-time root setup | root projection updated from `getDesignModel()` | generated host starts from `getDesignView()` and `Design.DataContext` uses `UserInputPreviewHost` | Both now satisfy the preview requirement; ElmView no longer needs a separate handwritten preview surface. |
| Runtime verification in tests | shared substrate behavior covered in `SnapshotSubstrateTests` | shared substrate plus 17 generated-host tests in `ElmViewGeneratedHostTests` | ElmView V2 now has direct automated coverage for writable bindings and nested property propagation. |

## Category Results

### Boilerplate

`Projection` still has the most explicit CLR-facing surface, but it pays for
that clarity with more handwritten glue. In the `ExampleMatrixSample`, the
projection path carries a 489-line `AppProjection.cs` file, while ElmView V2
uses a 278-line `AppHost.cs`.

The clearest before/after signal is the form page:

- `Projection` uses generated MVVM properties, six `On...Changed` partial
  methods, and `_isUpdating` suppression to avoid feedback loops.
- `ElmView` uses six write-back registrations near host construction and
  writable generated properties that forward through `TryDispatchWriteBack`.

Conclusion: ElmView V2 now wins on boilerplate reduction for standard
editable-field scenarios. Projection remains justified when the team wants
many explicit CLR-facing commands, viewmodel types, or mutable collection
surfaces.

### Reviewability

For a normal interactive UI change, the review surface is now different in a
useful way:

- `Projection` changes often span F# state/update logic, projection property
  definitions, projection update code, setter/command forwarding, and AXAML.
- `ElmView` changes usually span F# view records, F# update logic, AXAML, and
  one small host mapping section.

ElmView V2 is therefore better when the review goal is "show me the immutable
UI shape plus the visible markup". Projection is better when the review goal
is "show me the exact CLR-facing viewmodel contract and command surface".

Conclusion: ElmView V2 is now the more compact review surface for ordinary UI
changes. Projection still offers the most explicit review surface for command-
heavy or control-specific mechanics.

### Runtime Behavior

The shared runtime substrate is now a real point of convergence rather than a
theoretical one:

- `SnapshotSubstrateTests.fs` covers bindable snapshot updates, notification
  skipping for identical references, node refresh, and keyed collection
  identity-preserving reorder/replacement behavior.
- `ElmViewGeneratedHostTests.fs` adds 17 focused tests for generated host
  behavior, including text, checkbox, combo-box, slider, and multiline text
  write-back, `OneWay` vs `TwoWay`, nested node propagation, and "dispatch
  exactly once" behavior after snapshot catch-up.

Conclusion: ElmView V2 no longer trails Projection on runtime confidence for
interactive bindings. It now has stronger direct coverage for writable-binding
behavior than the Projection sample path. Projection still has the simpler
runtime mental model because everything is an explicit viewmodel property or
command.

### Design-Time Quality

The repository requirement was that Avalonia preview must stay normal and
trustworthy.

That bar is now met by both paths:

- `Projection` previews by updating the root projection with
  `Core.App.getDesignModel()`.
- `ElmView` previews through the same generated host shape used at runtime,
  with `AppHost` starting from `Core.App.getDesignView()` and
  `UserInputPreviewHost` used from `Design.DataContext`.

Conclusion: design-time quality is now effectively at parity for the current
sample matrix. ElmView V2 removed the earlier concern that preview might drift
from the runtime host shape.

### LLM Editing Ergonomics

ElmView V2 improves the repository's "LLMs should be able to extend this
reliably" goal in three ways:

- AXAML binding paths stay ordinary, for example `UserInput.Name`.
- editable-field routing is centralized in one `ConfigureBindings` block
  instead of scattered setter callbacks
- the important authored UI shape remains in plain F# records

Projection is still straightforward for an LLM when a task is essentially
"add another explicit property/command/viewmodel". It becomes noisier when the
same feature requires repeated update, forwarding, and loop-suppression
patterns across many CLR-facing types.

Conclusion: ElmView V2 is now the better default for LLM-assisted editing when
the feature mostly fits immutable snapshots plus standard Avalonia bindings.
Projection remains the better fit when the task needs very explicit,
named CLR seams or control-specific imperative behavior.

## Overall Decision

After ElmView V2, the comparison changes in a meaningful way:

- `Projection` is no longer the default winner on reviewability or practical
  editability for normal forms and standard interactive pages.
- `ElmView` is now the stronger default for features that want immutable F#
  view data, ordinary AXAML, centralized editable-field routing, and a small
  host layer.
- `Projection` remains important as the explicit-viewmodel path for teams or
  features that benefit from named CLR surfaces, command-heavy interaction,
  or stable mutable shells.

That means the repository should continue to keep both families, but the
post-V2 default bias should be:

- prefer `ElmView` for standard screen authoring
- prefer `Projection` as the escape hatch for explicitly viewmodel-shaped or
  mutable-control-heavy scenarios

This result is consistent with the repository plan: shared substrate in the
core, Avalonia-normal behavior in both families, and ElmView as the primary
closest-to-Elm research direction once V2 proved out.
