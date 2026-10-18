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

The subscription callback may run on a worker thread. Dispatch through the
Avalonia host: it queues messages in FIFO order and drains them on `Dispatcher.UIThread`, running
inline if already there and posting otherwise. This serializes the Elmish
message queue as well as the bound viewmodel updates. Dispatching directly
into an unprotected Elmish queue from multiple threads is not safe.

An idle UI-thread dispatch runs inline; if worker messages are waiting, it joins
that queue rather than overtaking them. A dispatch during a drain never re-enters
Elmish. The framework-neutral core host instead drains on the dispatching thread
that finds the queue idle: it guarantees serialization and FIFO order, not thread
affinity. Each drain handles only the queue snapshot it found at entry; later arrivals are scheduled on the thread pool (which calls the supplied `post`), unless another dispatcher takes the idle queue first, so
one producer does not have to drain an indefinitely growing queue. Update exceptions propagate to the draining caller and stop that batch; the next dispatch can resume remaining queued messages. A failure from `post` is not recovered here and can leave the queue waiting. Disposal queues a stop message, so messages already queued may run first. Core `startAndBind` delivers models on the draining thread.

The Avalonia host publishes the next model on the UI thread before raising
notifications or patching a bound collection.

Do not call `Dispatcher.UIThread.Post` inside every background callback. The
host boundary already owns UI-thread publication.

