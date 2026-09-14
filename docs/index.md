# Elmish.Avalonia.Glue

Avalonia gives you AXAML, compiled bindings, design-time preview, and DevTools.
Elmish gives you immutable state, explicit messages, and one update loop.

Elmish.Avalonia.Glue lets you keep both.

## The problem it solves

Elmish replaces an immutable model after each message. Avalonia expects a
stable `DataContext` that raises `PropertyChanged` and accepts `TwoWay` writes.

The library puts a small adapter between them:

```text
user edit → Avalonia binding → stable host → Elmish message
                                              ↓
AXAML binding ← PropertyChanged ← new immutable snapshot
```

The adapter is the glue. It does not replace AXAML, invent a UI DSL, or move
application behaviour out of F#.

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

You can use both styles in one application. The choice belongs to a screen,
not the whole codebase.

## Learn from a working path

1. [Build the smallest useful screen](guides/get-started.html).
2. [Learn how snapshots, hosts, and dispatch fit together](guides/architecture.html).
3. Build the screen with [Projection](guides/projection.html) or [ElmView](guides/elmview.html).
4. [Connect the host to the application lifetime](guides/connect-runtime.html).
5. [Surface background work through messages](guides/background-work.html).
6. [Add trustworthy design-time data](guides/design-time-preview.html).
7. [Preserve identity in changing lists](guides/keyed-collections.html).
8. [Explore the complete sample applications](guides/sample-applications.html).

When you know the concepts, use the generated [API reference](api.html).
