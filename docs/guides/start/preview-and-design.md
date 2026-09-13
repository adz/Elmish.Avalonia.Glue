---
sidebar_position: 2
---

# Preview and design data

The docs and samples keep design-time preview in the Avalonia workflow.

Design data is part of the authoring loop.

AXAML preview binds to the same shape runtime uses, with realistic F# data
supplied up front.

## Core Shape

- Projection samples populate a projection root with a design model.
- ElmView samples start the generated host with a design snapshot.
- AXAML binds to the same data-context path in both cases.
- Runtime later attaches dispatch and pushes live snapshots into that shape.

## What You Can Do

- open the sample windows in preview without booting the full runtime
- inspect the same page structure in DevTools at design time and runtime
- keep the preview path close enough to runtime that markup edits remain trustworthy

The F# part is where the sample snapshot is authored.

## Read next

- [Projection preview host](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/sample/Samples.Projection/ExampleMatrixSample/src/ExampleMatrixSample.UI/Views/AppView.axaml.cs)
- [ElmView preview host](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/sample/Samples.ElmView/ExampleMatrixSample/src/ExampleMatrixSample.UI/Views/AppView.axaml.cs)
- [ElmView generated host](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/sample/Samples.ElmView/ExampleMatrixSample/src/ExampleMatrixSample.UI/Views/AppHost.cs)
