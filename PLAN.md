# Elm-First Avalonia: Dual Architecture Exploration

## Summary

Restructure the solution around two explicit UI authoring approaches that share a common substrate:

- `Projection`: a C#-friendly Avalonia viewmodel path for teams that want explicit CLR-facing viewmodels. This family will support both:
  - classic mutable projections
  - snapshot-host projections with identity-aware updates
- `ElmView`: an F#-first path where F# view records are the UI schema and Avalonia binds through a generated/runtime-provided bindable surface, with no handwritten projection boilerplate

The goal is not to pick a winner upfront. The goal is to compare authored experience, reviewability, tooling compatibility, runtime behavior, and LLM ergonomics across both approaches using the same sample set.

## Core Constraints

### Primary product goal

Keep Avalonia completely normal while moving as much authored UI state and shaping logic as possible into Elm-style F#.

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

### Escape hatches

Allow explicit mutable keyed adapters when required for:

- large lists
- frequently updating lists
- selection-sensitive controls
- virtualization-sensitive controls
- inline-editing or row-heavy interaction

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
- Keep both classic and snapshot-host styles inside `Projection`.
- Treat `ElmView` as the main closest-to-Elm research direction.
- Keep AXAML and Avalonia tooling compatibility as a non-negotiable constraint.
- Start with the core non-WebGL Elm examples, plus clock and a basic SVG-equivalent example.
- Defer WebGL and Playground.
