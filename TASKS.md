# Tasks

1. [x] Commit the current working changes so the refactor baseline is preserved before the architecture split.
2. [x] Create the shared substrate project and move the reusable projection/snapshot primitives into it without breaking existing samples.
3. [x] Create the `Projection` package surface and structure it to support both classic projections and snapshot-host projections.
4. [x] Create the `ElmView` package surface and establish the F#-first schema/design-time path for AXAML-bound views.
5. [x] Extract the truly framework-neutral substrate toward an `Elmish.Glue.Core` package boundary and naming, while keeping implementation and samples Avalonia-focused.
6. [x] Add or reorganize sample solutions so there is a `Samples.Projection` suite and a `Samples.ElmView` suite.
7. [x] Implement the agreed initial example matrix in `Samples.Projection`: HTML/static layout, user input, random/dice, HTTP, time/clock, files, and basic SVG-equivalent.
8. [x] Implement the same initial example matrix in `Samples.ElmView`.
9. [x] Update docs to describe the shared substrate, the two architecture families, the design-time workflow, and when to choose each path.
10. [x] Run builds/tests across the solution and fix any regressions introduced by the split.

## Task Commit Policy

After each task is completed, create a separate git commit before starting the next task.

## Next Phase: ElmView V2

### Explicit goals

- Keep ElmView authored view models as plain immutable F# records.
- Keep AXAML as close as possible to standard Avalonia AXAML so LLM editing stays strong.
- Use normal binding modes as the primary signal: `OneWay` for snapshot display, `TwoWay` for editable fields.
- Do not require F# attributes or binding annotations on view records.
- Do not introduce a custom UI DSL or nonstandard control library as the main authoring path.
- Move interaction boilerplate out of code-behind and into generated or runtime-provided glue.
- Keep message routing explicit, but centralize it in one host-side mapping surface instead of scattering it through AXAML.
- Preserve Avalonia design-time preview and compiled-binding friendliness.
- Keep the runtime model immutable while presenting a bindable writable facade to Avalonia.
- Ensure interaction-heavy controls do not regress into feedback loops or redundant dispatch.

### ElmView V2 execution tasks

11. [x] Write a short design note that defines the ElmView V2 contract: pure F# view records, normal AXAML, generated bindable node graph, standard `TwoWay` bindings, and centralized write-back mapping.
12. [x] Define the generated host shape for ElmView: root host, nested bindable child nodes, property getters that read immutable snapshots, and property setters that dispatch messages.
13. [x] Define the write-back mapping API in one place near host construction with a target shape like `bindings.For(x => x.UserInput.Name).Dispatch(Msg.NewSetName)`.
14. [x] Decide whether write-back mappings are fully explicit or convention-first with explicit overrides, preferring explicit mappings unless conventions remain predictable.
15. [x] Prototype generated nested node support so AXAML can bind normally to paths like `UserInput.Name` rather than imperative code-behind event handlers.
16. [x] Make `Mode=TwoWay` the editable-field contract for ElmView while keeping `OneWay` display-only and non-dispatching.
17. [x] Remove manual form event-forwarding from the ElmView example matrix once the generated write-back host path is in place.
18. [x] Add ElmView interaction tests for writable generated properties covering text, checkbox, combo-box selection, slider or range, and multiline text.
19. [x] Add tests that prove snapshot updates do not re-dispatch and user edits dispatch exactly once.
20. [x] Add tests for nested bindable node propagation and `PropertyChanged` correctness across root and child nodes.
21. [x] Unify snapshot-host concepts across `Projection` and `ElmView` where possible so the core owns one coherent bindable snapshot substrate.
22. [ ] Rework keyed collection patching to avoid repeated `IndexOf`-driven quadratic behavior before scaling ElmView further into list-heavy scenarios.
23. [ ] Convert the ElmView example matrix form page to the new generated host and write-back path while keeping AXAML looking ordinary.
24. [ ] Validate that the converted sample still supports design-time preview with the same generated host shape.
25. [ ] After the form path is proven, extend the same pattern to other interactive sample areas.
26. [ ] Update docs to explain the new ElmView story clearly: pure F# models, normal AXAML, generated writable facade, centralized message mapping, and no F# binding metadata.
27. [ ] Re-evaluate the architecture comparison after ElmView V2 lands by measuring boilerplate, reviewability, runtime behavior, design-time quality, and LLM editing ergonomics against `Projection`.
