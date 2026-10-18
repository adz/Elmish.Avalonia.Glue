# Docs review: fresh-context reading notes

Scope: README.md and every Markdown page under docs/, read without opening implementation or sample source. Claims below are documentation assertions to verify later, not findings about implementation.

## Assumptions

- [fixed] `docs/index.md:4` — “Avalonia gives you AXAML, compiled bindings, design-time preview, and DevTools.” Assume these all work with the library's host shapes.
- [fixed] `docs/index.md:50-56` — “You can use both styles in one application. The choice belongs to a screen, not the whole codebase.” Verify mixed-screen setup and ownership.
- [fixed] `docs/guides/01-get-started.md:46-50` — “Keep one host instance … host.Update raises PropertyChanged for Name.” Verify exact notification path, including nested properties and equality checks.
- [fixed] `docs/guides/02-architecture.md:42-50` — “One user edit should dispatch one message, and one snapshot update should dispatch none.” Verify TwoWay reentrancy and initial binding behavior.
- [fixed] `docs/guides/03-projection.md:58-64` — “SnapshotHost<T> exposes one immutable value through Current. Bind directly to Current.Name.” Verify editable vs read-only binding behavior.
- [fixed] `docs/guides/04-elmview.md:68-75` — “the package does not yet emit it from the record automatically” and “The setter never mutates the snapshot.” Verify generator status and missing-route behavior.
- [fixed] `docs/guides/05-connect-runtime.md:20-26` — “delivers updates inline when already on the UI thread; from other threads it posts to Dispatcher.UIThread.” Verify ordering, teardown and posting failures.
- [fixed] `docs/guides/05-connect-runtime.md:29-35` — “After disposal, the connection stops accepting useful work.” Verify whether Dispatch throws, ignores, or queues.
- [fixed] `docs/guides/06-background-work.md:89-110` — “It returns an IDisposable that stops the producer” and “Use a stable subscription key.” Verify lifecycle and duplicate-key semantics.
- [fixed] `docs/guides/06-background-work.md:113-133` — “queues messages in FIFO order,” “An idle UI-thread dispatch runs inline,” “Each drain handles only the queue snapshot it found at entry,” and “Update exceptions propagate to the draining caller; the next dispatch can resume any queued messages.” Verify race, fairness, exception recovery and disposal cases.
- [fixed] `docs/guides/07-design-time-preview.md:39-43` — “Writable setters should not fail because no runtime dispatcher has been attached.” Verify all host variants, commands and unmatched routes.
- [fixed] `docs/guides/08-keyed-collections.md:18-19` — “Duplicate keys in an incoming SyncWith snapshot fail with an error naming the duplicate key.” Verify exception type, when failure occurs, and whether collection remains unchanged.
- [fixed] `docs/guides/08-keyed-collections.md:23-34` — “retained items keep their CLR identity.” Verify snapshot-only collection versus mutable row VM distinction and selection implications.
- [fixed] `docs/guides/10-all-fsharp-ui.md:8-14` — “Do not declare Avalonia styled or attached properties in F# … causing AVLN3000.” Verify versions and whether this is an absolute limitation.
- [fixed] `docs/api/core/snapshots.md:20-25` — “If the new snapshot is different (by reference) … raises PropertyChanged … and calls OnSnapshotUpdated.” Verify null/value-type behavior, call order, and same-reference mutation.
- [fixed] `docs/api/core/collections.md:9-13` — “list and seq overloads copy … O(n) time and space … arrays and ResizeArray can use IReadOnlyList … without that copy.” Verify overload selection and overall patch complexity.
- [fixed] `docs/api/core/collections.md:43-48` — “Avalonia can reuse … containers,” “Selected items remain selected,” “List animations … work correctly.” Verify these conditional framework claims; retained key alone may not retain selected snapshot object.
- [fixed] `docs/api/core/host-lifetime.md:26-31` — “dispatch function … remains stable even if the underlying program is restarted or modified.” Verify restart support and actual lifetime guarantee.
- [fixed] `docs/api/avalonia/dispatcher.md:7-18` — “start … Returns an IDisposable” and “start methods use … UIThread.Post … always executed on UI thread.” Verify concrete return type and inline exception to Post described elsewhere.
- [fixed] `docs/api/projection/keyed-collections.md:16-33` — “Update items in place if they are already present (by reference or value)” and “selected rows remain selected while their data changes.” Verify whether immutable snapshots are replaced and how item identity is defined.
- [fixed] `docs/api/elmview/hosts.md:36-41` — “raises PropertyChanged for its View property and all names in GeneratedPropertyNames, then calls RefreshSubtree() on all nested nodes.” Verify order, changed-only vs unconditional notifications.
- [fixed] `docs/api/elmview/write-back.md:105-110` — “selector is type-checked against the F# view record.” Verify accepted expression forms and runtime validation.

## Unclear or misleading

- [fixed] `docs/guides/01-get-started.md:3-4,24-25` — “You do not need you” / “assertions show visible” look like editing remnants; distract from first steps.
- [fixed] `docs/guides/01-get-started.md:28-55` — Says DataContext needs writable Name, then ElmView getter/setter is described only abstractly; no complete runnable host + AXAML + startup to follow.
- [fixed] `docs/guides/03-projection.md:20-38` — C# sample shows `Update` and PropertyChanged but not a writable property or dispatch, while later prose says writable projection properties dispatch. A beginner might imitate the private setter and expect TwoWay to work.
- [fixed] `docs/guides/03-projection.md:58-64` — `Current.Name` is presented after the editable-field tutorial; no warning that a record's immutable field cannot receive a TwoWay write directly.
- [fixed] `docs/guides/04-elmview.md:43-75` — `Profile` root property and generated-shaped host are not defined in the snippet; “generated” sounds automatic until the late disclaimer.
- [fixed] `docs/guides/05-connect-runtime.md:14-35` — No concrete startup snippet despite instructions to connect three callbacks; `startAndBind` overload resolution and model-to-view derivation left to samples.
- [fixed] `docs/guides/06-background-work.md:89-110` — Is the subscription snippet executable against the Elmish version in use? Key syntax, subscription restart/disposal and cancellation are not explained.
- [fixed] `docs/guides/06-background-work.md:113-133` — Dense scheduler internals interrupt a task guide; “later arrivals … handed off asynchronously (or picked up by another dispatcher)” leaves unclear exactly when that happens for a core consumer.
- [fixed] `docs/guides/07-testing-headlessly.md:1-27` — Example is pseudocode inside a text block with an empty test, not copy/pasteable; `HeadlessTests.fs` is not linked and testing page is absent from homepage path.
- [fixed] `docs/guides/10-all-fsharp-ui.md:17-38` — `RuntimeGeneratedViewHost` constructed without explicitly defining a Name facade; unclear how AXAML can bind Name and what “generated setter” means here.
- [fixed] `docs/api/avalonia/dispatcher.md:14-18` versus `docs/guides/05-connect-runtime.md:20-26` — “use UIThread.Post” reads as unconditional, but guide says inline on UI thread.
- [fixed] `docs/api/projection/snapshot-host.md:14-43` — `<Button Command="{Binding Save}" />` binds a method, not a shown ICommand property; `FullName` has no shown notification when only `Current` changes. This example may teach nonworking bindings.
- [fixed] `docs/api/projection/keyed-collections.md:26-33` versus `docs/guides/08-keyed-collections.md:36-42` — Snapshot collection allegedly keeps row identity but guide distinguishes it from row projections and says only collection identity remains stable. Which is promised?
- [fixed] `docs/api/elmview/nodes.md:31-72` — Example setter uses `Host` without declaring it; constructor calls `host.RegisterChildNode` without explaining visibility, and `FormView` vs `UserInput` naming is inconsistent.
- [fixed] `docs/api/elmview/write-back.md:16-25` versus `docs/guides/10-all-fsharp-ui.md:29-35` — Member list mentions only `For(selector)` but F# example uses `ForPath<string>`; no reference for string-path validation and failure handling.
- [fixed] `docs/examples/elmview-write-back.md:22-43` — `Host.Name` is flat but the route is `UserInput.Name`; this shows routing but not the nested AXAML path emphasized in the guide.
- [fixed] `README.md:27-30` — Relative sample links may not work outside a GitHub repository-root rendering; they are not installation/run instructions.

## Too long

- [done] `docs/guides/06-background-work.md` (154 lines): split into **Commands and subscriptions** (state transition, finite work, ongoing source, disposal/cancellation: lines 26-110, 137-145) and **Threading and dispatch guarantees** (message boundary, Avalonia/core queue behavior and failure semantics: lines 6-24, 113-135); leave a brief pointer to both styles (146-154).
- [done] `docs/api/elmview/write-back.md` (118 lines): split reference front matter for `WriteBackBindings`/`WriteBackBindingRegistration` (members and route failure semantics) from a **Route editable ElmView properties** recipe (registration, AXAML, dispatch walkthrough, display-only and commands).
- [done] `docs/api/elmview/nodes.md` (80 lines): keep node contract and notification guarantees in reference; move the extended C#/F#/AXAML sample to an ElmView nested-path recipe.

## Reference material in guides

- [done] `docs/guides/02-architecture.md`, **Package boundaries** (61-68): package responsibility table could be canonical on `docs/api/index.md` / package index front matter, with a link from the conceptual guide.
- [done] `docs/guides/05-connect-runtime.md`, **Start Elmish through the Avalonia bridge / Keep the connection alive** (14-35): inline-vs-post, queue/lifetime and disposal contract belongs with `Elmish.Avalonia.Glue.ElmishHost` and `ElmishHostConnection<'Msg>` (`docs/api/avalonia/dispatcher.md`, `docs/api/core/host-lifetime.md`); retain use-case guidance here.
- [done] `docs/guides/06-background-work.md`, **Publish only through the Avalonia host** (113-135): exhaustive FIFO, drain, threading and exception guarantees belong on `ElmishHost` reference (`docs/api/avalonia/dispatcher.md` and `docs/api/core/host-lifetime.md`).
- [done] `docs/guides/08-keyed-collections.md`, **Choose the collection API** (36-42): API selection belongs on `ObservableCollectionExtensions.SyncWith` and `KeyedSnapshotCollection<'T,'Key>` (`docs/api/core/collections.md`), linked from task guidance.
- [done] `docs/guides/10-all-fsharp-ui.md`, **Styled properties and attached properties** (6-15): Avalonia interop limitation is reference/compatibility material for the Avalonia package (`docs/api/avalonia/index.md`), with version scope.

## Missing

- [done] Installation prerequisites: NuGet IDs/versions, supported .NET/Avalonia/Elmish/F# versions, required project references, and a minimal run command (`README.md`, `docs/guides/01-get-started.md`).
- [done] One complete copyable minimal Projection and ElmView screen from model through host, AXAML and app lifetime, including a genuine TwoWay edit and notification (`docs/guides/13-minimal-projection-screen.md`, `docs/guides/14-minimal-elmview-screen.md`, `docs/examples/minimal-projection-screen.md`, `docs/examples/minimal-elmview-screen.md`).
- [done] Task index covering startup, write-back, nested node, list, design preview, background work, headless testing and all-F# UI; homepage path omits testing and all-F# (`docs/index.md:59-70`).
- [done] Decision table for Projection vs snapshot host vs ElmView vs core keyed collection/row projection, especially editable paths and identity (`docs/index.md:47-57`, `docs/api/index.md:6-17`).
- [done] Error-handling recipes: duplicate key before/after partial patch, missing/duplicate write-back route, binding errors, dispatcher not set/disposed, exceptions in update/subscription/post and stale async results (`docs/guides/15-error-handling.md`).
- [done] Migration notes for compatibility aliases and packages: exact namespace/reference changes and what is or is not source-compatible (`docs/api/avalonia/aliases.md:20-23`).
- [done] Glossary for model, view snapshot, host, node, projection, generated-shaped, route, subscription and keyed identity (`docs/guides/02-architecture.md`).
- [done] How to set up actual design-time DataContext in AXAML, and how to run preview without runtime (`docs/guides/07-design-time-preview.md:25-46`).
- [done] A working, linked headless test and package install/test commands (`docs/guides/07-testing-headlessly.md:1-27`).
- [done] `llms.txt` absent: no compact navigation map, API disambiguation or links to task-specific canonical pages.

## Suggestions for humans and LLMs

- Put a task-oriented entry map in README/home and `llms.txt`; label runnable snippets versus illustrative/pseudocode and link each to the matching fully working sample.
- Use consistent terminology (`AXAML` vs `.axaml`, “generated-shaped” vs “generated”, `Current` vs `Snapshot` vs `View`), and consistently qualify similarly named `ElmishHost` and keyed collection types by namespace.
- Make API enrichment pages entity-focused with stable entity IDs, canonical overload/exception/thread/lifetime notes and a single link back to each guide; avoid duplicated guarantees that drift.
- Add front matter for package, audience, prerequisites, API entities, task, and status (compiled/runnable/illustrative), plus a small example manifest for LLM retrieval.
- Cross-link conceptual claims directly to reference contracts and testable examples; explicitly distinguish collection identity, row identity, item value replacement, and Avalonia container/selection behavior.
- [done] Provide a compact troubleshooting matrix keyed by symptom (stale TextBox, missing PropertyChanged, preview crash, lost selection, cross-thread exception) and concrete verification steps.
