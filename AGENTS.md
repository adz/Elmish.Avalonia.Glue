# Repository Context

## Product Direction

This repository is exploring two UI authoring styles on top of Elmish:

- `Projection`: explicit .NET/Avalonia-facing viewmodels, including both classic projection trees and snapshot-host variants
- `ElmView`: an F#-first path where immutable F# view records are the UI schema and handwritten projection boilerplate is minimized or eliminated

The current implementation focus remains Avalonia-first.

## Hard Constraints

- Keep Avalonia normal: standard `.axaml`, standard bindings, standard design-time preview, standard DevTools / DevTools MCP workflows.
- Do not introduce a custom UI DSL or nonstandard binding model.
- Preserve a strong design-time story: sample view data should support rendered preview without booting the full Elmish runtime.
- Push as much authored UI shaping logic as practical into F#.
- Keep C# glue shallow, mechanical, and highly patterned.

## Architectural Direction

- The generic architectural center of gravity should move toward a framework-neutral core, tentatively `Elmish.Glue.Core`.
- Only extract code to the generic core when it is truly framework-neutral and based on standard .NET concepts like immutable snapshots, property change notification, and keyed collection patching.
- Keep framework-specific binding/runtime/design-time integration in higher-level packages.
- Even while generalizing the core, stay Avalonia-focused in implementation order, docs, and samples.

## Execution Rules

- `PLAN.md` is the active architecture plan.
- `TASKS.md` is the execution checklist.
- After each completed task in `TASKS.md`, create a separate git commit before starting the next task.
- Prefer incremental changes that keep current tests and sample builds green.
- Do not treat speculative architecture as complete until it is proven in samples.

## Sample Direction

The repo should eventually contain parallel sample suites:

- `Samples.Projection`
- `Samples.ElmView`

Each suite should implement the same Elm-inspired example matrix so authored experience can be compared directly.

## Design Bias

- Favor immutable F# view data.
- Favor identity-aware updates.
- Use mutable keyed adapters only where a framework materially benefits from stable mutable identity.
- Optimize for strong conventions that are easy for humans and LLMs to extend.
