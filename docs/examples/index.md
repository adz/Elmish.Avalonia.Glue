---
title: Runnable examples
---

# Runnable examples

Each example page is generated from code in `tools/DocsExamples`. The example
project is built before the documentation site, so its linked source is the
authoritative, compilable version—not a copy that can drift.

## Choose an example

- [Use a snapshot host](projection-snapshot-host.html) to see an immutable value
  replace a stable host snapshot.
- [Route an ElmView edit](elmview-write-back.html) to see a generated-shaped
  setter dispatch one message.
- [Patch a keyed collection](keyed-collection-sync.html) to see retained rows
  keep identity while order and values change.

Run all examples and regenerate their observed output with:

```bash
dotnet run --project tools/DocsExamples/DocsExamples.fsproj
```

Use an example as a narrow proof of one mechanism. Use the sample suites when
you need complete AXAML, design-time data, and application startup.
