# Tasks

1. [x] Commit the current working changes so the refactor baseline is preserved before the architecture split.
2. [x] Create the shared substrate project and move the reusable projection/snapshot primitives into it without breaking existing samples.
3. [x] Create the `Projection` package surface and structure it to support both classic projections and snapshot-host projections.
4. [x] Create the `ElmView` package surface and establish the F#-first schema/design-time path for AXAML-bound views.
5. [x] Extract the truly framework-neutral substrate toward an `Elmish.Glue.Core` package boundary and naming, while keeping implementation and samples Avalonia-focused.
6. [x] Add or reorganize sample solutions so there is a `Samples.Projection` suite and a `Samples.ElmView` suite.
7. [x] Implement the agreed initial example matrix in `Samples.Projection`: HTML/static layout, user input, random/dice, HTTP, time/clock, files, and basic SVG-equivalent.
8. [ ] Implement the same initial example matrix in `Samples.ElmView`.
9. [ ] Update docs to describe the shared substrate, the two architecture families, the design-time workflow, and when to choose each path.
10. [ ] Run builds/tests across the solution and fix any regressions introduced by the split.

## Task Commit Policy

After each task is completed, create a separate git commit before starting the next task.
