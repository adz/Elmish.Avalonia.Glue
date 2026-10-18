# Threading and dispatch guarantees

Background callbacks publish messages through the host rather than editing bound objects. Start with [commands and subscriptions](background-commands.html) to choose a producer.

## Keep messages at the concurrency boundary

The background source reports a fact. Elmish decides what that fact means for
the model. The normal host update path then publishes the next snapshot.

```text
background callback
→ dispatch ReadingReceived
→ Elmish update
→ new immutable model
→ new view snapshot
→ Avalonia UI-thread post
→ host.Update
→ PropertyChanged
→ AXAML refresh
```

This path gives every state change the same ordering and debugging story,
regardless of which thread produced the message.

## Publish only through the Avalonia host

Dispatch through the Avalonia host so bound state is published on the UI thread. See the [Avalonia queue guarantees](../api/Elmish.Avalonia.Glue.ElmishHost.html#dispatch-queue-guarantees) and [core queue guarantees](../api/Elmish.Glue.Core.ElmishHost.html#core-queue-guarantees) for ordering, drain and failure semantics.

The Avalonia host publishes the next model on the UI thread before raising
notifications or patching a bound collection.

Do not call `Dispatcher.UIThread.Post` inside every background callback. The
host boundary already owns UI-thread publication.

