# Elmish.Avalonia.Glue

Avalonia supports ordinary AXAML, compiled bindings (when configured in the app), design-time preview (with a design DataContext), and DevTools (when enabled). The library's stable CLR hosts and nodes expose standard properties to those tools; it does not enable or configure them automatically.
Elmish gives you immutable state, explicit messages, and one update loop.

Elmish.Avalonia.Glue connects them.

## Binding immutable state

Elmish replaces an immutable model after each message. Avalonia expects a
stable `DataContext` that raises `PropertyChanged` and accepts `TwoWay` writes.

The library puts a small adapter between them:

```text
user edit → Avalonia binding → stable host → Elmish message
                                              ↓
AXAML binding ← PropertyChanged ← new immutable snapshot
```

The adapter leaves AXAML and application behaviour in F# intact.

## A concrete example

Imagine a profile form. The application state stays immutable:

```fsharp isolated
type Model = { Name: string; Newsletter: bool }
type Msg = NameChanged of string | NewsletterChanged of bool

let update msg model =
    match msg with
    | NameChanged name -> { model with Name = name }
    | NewsletterChanged value -> { model with Newsletter = value }
```

The view remains ordinary AXAML:

```xml
<TextBox Text="{Binding Profile.Name, Mode=TwoWay}" />
<CheckBox IsChecked="{Binding Profile.Newsletter, Mode=TwoWay}" />
```

The host exposes `Profile.Name` and `Profile.Newsletter`. Reads come from the
latest snapshot. Writes dispatch `NameChanged` or `NewsletterChanged`.

## Choose an authoring style

Use **Projection** when a named CLR viewmodel is useful application code. It
is a good fit for command-heavy screens and controls that need mutable row
identity.

Use **ElmView** when an immutable F# view record should define the screen
shape. The CLR host then stays shallow and mechanical.

You can use both styles in one application: each screen owns its own stable host/DataContext and dispatch connection to an Elmish program. Share a program by routing its model and messages to the respective screens, or start separate programs and dispose each connection with its owning view. There is no automatic mixed-screen composition API.

## Choose the adapter

| Need | Choose | Editable path and identity |
| :--- | :--- | :--- |
| Explicit named CLR viewmodels | [Projection](guides/projection.html) | Write setters that dispatch; use row projections for stable row object identity. |
| Mostly read-only immutable values | <a href="api/Elmish.Avalonia.Glue.Projection.SnapshotHost`1.html">Snapshot host</a> | Bind `Current` for reads; add an explicit writable facade for edits. |
| F# records define screen shape | [ElmView](guides/elmview.html) | Register writable routes; keep nested node identity stable. |
| Framework-neutral keyed lists | [Core collections](reference/core/collections.html) | `SyncWith` retains row projections; keyed snapshots retain the collection, not row objects. |

## Tasks

- [Install and run](guides/install.html); [connect startup and lifetime](guides/connect-runtime.html).
- [Copy a minimal Projection screen](guides/minimal-projection-screen.html); [copy a minimal ElmView screen](guides/minimal-elmview-screen.html).
- [Route a TwoWay edit](guides/route-editable-elmview-properties.html); [bind nested nodes](guides/nested-elmview-paths.html).
- [Patch lists](guides/keyed-collections.html); [render design preview](guides/design-time-preview.html).
- [Publish background work](guides/background-commands.html); [test headlessly](guides/testing-headlessly.html).
- [Build an all-F# UI](guides/all-fsharp-ui.html); [handle errors](guides/error-handling.html); [troubleshoot symptoms](guides/troubleshooting.html); [glossary](guides/glossary.html).

## Follow a working path

1. [Build the smallest useful screen](guides/get-started.html).
2. [Learn how snapshots, hosts, and dispatch fit together](guides/architecture.html).
3. Build the screen with [Projection](guides/projection.html) or [ElmView](guides/elmview.html); copy the full [Projection](guides/minimal-projection-screen.html) or [ElmView](guides/minimal-elmview-screen.html) screen.
4. [Connect the host to the application lifetime](guides/connect-runtime.html); [handle errors](guides/error-handling.html).
5. [Surface background work through messages](guides/background-commands.html).
6. [Add trustworthy design-time data](guides/design-time-preview.html).
7. [Preserve identity in changing lists](guides/keyed-collections.html).
8. [Explore the complete sample applications](guides/sample-applications.html).

When you know the concepts, use the generated [API reference](api.html).
