# Elm-First Avalonia: Dual Architecture Exploration

## Summary

Restructure the solution around two explicit UI authoring approaches that share a common substrate:

- `Projection`: a C#-friendly Avalonia viewmodel path for teams that want explicit CLR-facing viewmodels. This family will support both:
  - classic mutable projections
  - snapshot-host projections with identity-aware updates
- `ElmView`: an F#-first path where F# view records are the UI schema and Avalonia binds through a generated/runtime-provided bindable surface, with no handwritten projection boilerplate

The goal is not to pick a winner upfront. The goal is to compare authored experience, reviewability, tooling compatibility, runtime behavior, and LLM ergonomics across both approaches using the same sample set.

The generic architectural center of gravity should move toward a framework-neutral core package, tentatively `Elmish.Glue.Core`, while the active implementation and sample focus remains Avalonia-first.

## Core Constraints

### Primary product goal

Keep Avalonia completely normal while moving as much authored UI state and shaping logic as possible into Elm-style F#.
Generic runtime ideas should be extracted only when they are truly framework-neutral and do not compromise the Avalonia design-time and tooling story.

### Tooling constraints

- AXAML remains a first-class, ordinary Avalonia authoring format.
- Standard Avalonia design-time preview workflows must continue to work.
- DevTools and DevTools MCP must remain usable for rendered-tree inspection and pixel-precise iteration.
- The architecture must not require a custom view DSL, custom binding syntax, or unusual designer setup.
- Design-time rendering must work from sample view data without requiring a live Elmish runtime.

### Authoring constraints

- F# is the preferred place for model, update, and UI-shaped view data.
- Boilerplate C# projection code must not be the default path for Elm-like authoring.
- Any C# layer that remains must be shallow, mechanical, and highly patterned.
- Conventions may be strong and opinionated, but they must also be rigid, consistent, and easy for humans and LLMs to follow.
- The important authored logic for a visual change should ideally live in:
  - F# view types
  - F# sample design snapshots
  - AXAML
  - small command or host declarations only when required

### Runtime constraints

- The runtime should exploit immutable view data and structural sharing where practical.
- Identity checks should be used to skip unchanged subtrees and reduce update work.
- Mutable keyed adapters should exist only where Avalonia materially benefits from stable mutable identity.
- Large or interaction-heavy collections are allowed to use explicit mutable patching.
- The runtime model must remain inspectable and debuggable; avoid opaque smart behavior that cannot be understood from conventions plus readable generated/runtime code.

### Review and LLM constraints

- The architecture must reduce the number of files touched for a typical UI change.
- Generated or runtime-provided glue must be consistent enough that LLMs can reliably extend it.
- Humans must be able to reason about the system from a small number of concepts.
- Strong convention is acceptable; hidden special cases are not.

## Solution Structure

### Shared substrate

Create a common low-level library used by both top-level approaches. This shared layer should contain:

- Elmish host/lifetime management
- identity-aware snapshot update primitives
- keyed collection patching primitives
- design-time snapshot conventions
- any common bindable-node or host infrastructure that is genuinely reusable

This layer is implementation infrastructure, not the primary authoring surface.

This shared layer should be designed so it can become a framework-neutral `Elmish.Glue.Core` foundation over time:

- standard .NET collection and notification patterns belong here when they are not Avalonia-specific
- framework-specific binding, design-time, and host integration should remain in the higher layers
- Avalonia stays the reference implementation and immediate delivery target while the generic seams are clarified

### `Projection` family

A separate library/package for C#-facing viewmodel authoring.

It should support two sub-styles:

- classic projection tree:
  explicit `ObservableObject` projections similar to the current model
- snapshot-host projection:
  explicit C# hosts over immutable F# view snapshots, using identity-aware updates and shallow mutable shells instead of deep projection trees

This family is for teams that still want named CLR viewmodels and explicit C# ownership.

### `ElmView` family

A separate library/package for the F#-first, no-handwritten-projection path.

Characteristics:

- F# view records are the UI schema
- AXAML binds to a normal CLR data context backed by that schema
- handwritten projection classes are not part of the normal feature workflow
- the bindable surface must support both runtime and design-time usage in the same shape

This family is the closest-to-Elm path and the primary experimental avenue.

#### ElmView V2 direction

The next ElmView iteration should preserve the same core goal while making
interaction-heavy views feel substantially more like normal Avalonia authoring.

The intended direction is:

- F# view records remain plain immutable authored UI schema
- AXAML should stay as close as possible to standard Avalonia AXAML so human
  and LLM editing remains strong
- normal binding modes should carry the main interaction meaning:
  - `OneWay` means snapshot display
  - `TwoWay` means editable field backed by ElmView write-back glue
- the system should not require F# attributes or model annotations to drive
  binding behavior
- the system should not require a custom UI DSL or nonstandard control set
- imperative control-event forwarding should move out of handwritten
  code-behind and into generated or runtime-provided glue

That implies ElmView should evolve toward a generated bindable facade over the
immutable F# snapshot tree:

- generated root and nested bindable node types expose normal CLR properties
- property getters read from the current immutable snapshot
- writable properties dispatch Elmish messages instead of mutating the
  snapshot directly
- snapshot updates raise `PropertyChanged` across the generated node graph
- write-back routing is configured centrally near host construction rather than
  repeated in AXAML or encoded in the F# record definitions

The practical authoring target is:

- plain immutable F# view records
- ordinary AXAML bindings that look normal to Avalonia users
- centralized write-back mapping for editable fields
- no handwritten per-control loop-suppression code for common form controls

This is intentionally closer to Elm's authored feel while still respecting the
constraints of Avalonia and desktop control behavior.

## Design-Time Story

Design-time support is a hard requirement, not a follow-up task.

Both architecture families must support:

- realistic feature-level sample view data
- a normal Avalonia design-time data context
- previewable AXAML without booting the full application

The common pattern should be:

- F# defines UI-shaped `View` types
- F# provides sample design snapshots
- a small design host wraps those snapshots into the bindable shape used by AXAML
- AXAML preview binds to that shape exactly as runtime does, or as close as possible

The design-time bindable surface must be close enough to runtime that preview is trustworthy for visual editing and DevTools inspection.

## Example Program Matrix

Create two separate sample suites, each implementing the same feature set:

- `Samples.Projection`
- `Samples.ElmView`

Each sample suite should implement the same core Elm example set so the authored experience is directly comparable.

### Initial example set

Target the core non-WebGL Elm example family first, plus two explicit additions:

- basic HTML / static layout equivalent
- user input / forms
- random / dice equivalent
- HTTP
- time / clock
- files
- a basic SVG-equivalent sample

Do not include:

- WebGL
- Playground

The goal is breadth across interaction patterns, not full parity with browser-only capabilities.

## Architectural Defaults

### Default bias for new work

- prefer immutable F# view records
- prefer AXAML as the authored view format
- prefer design snapshots as part of the normal workflow
- prefer identity-aware updates
- prefer no handwritten projection trees unless justified
- prefer generated bindable facades over handwritten imperative bridge code
  when ElmView needs editable control support

### Escape hatches

Allow explicit mutable keyed adapters when required for:

- large lists
- frequently updating lists
- selection-sensitive controls
- virtualization-sensitive controls
- inline-editing or row-heavy interaction

For ElmView specifically, framework-facing writable facades or generated
write-back glue are acceptable when they are:

- mechanical
- centralized
- predictable
- invisible to the authored F# schema

They should be treated as implementation substrate rather than authored UI
state.

### Projection family comparison intent

Do not collapse the `Projection` family to a single style immediately. Keep both:

- classic projection
- snapshot-host projection

This preserves a useful comparison axis:

- explicit viewmodel ergonomics
- amount of boilerplate
- runtime behavior
- reviewability
- LLM friendliness

## Immediate Next Planning Work

The next detailed plans should specify:

1. shared substrate API
2. `Projection` package shape
3. `ElmView` package shape
4. sample execution plan

## Assumptions and Defaults

- Use a shared core plus two top-level packages/libraries.
- Keep moving the core toward framework-neutral naming and responsibilities where that does not dilute the Avalonia-first product direction.
- Keep both classic and snapshot-host styles inside `Projection`.
- Treat `ElmView` as the main closest-to-Elm research direction.
- Keep AXAML and Avalonia tooling compatibility as a non-negotiable constraint.
- Start with the core non-WebGL Elm examples, plus clock and a basic SVG-equivalent example.
- Defer WebGL and Playground.
