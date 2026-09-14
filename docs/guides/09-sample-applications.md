# Sample applications

The sample suites implement the same screens twice. Use them to compare
authored experience, not as competing product versions.

## Follow the example matrix

Both suites include:

- static layout and cards;
- form inputs and validation;
- random commands and history;
- HTTP loading, success, and failure;
- a clock subscription;
- Avalonia file picking;
- vector-like composition with Avalonia shapes.

This range matters. A counter proves dispatch, but it does not prove forms,
asynchronous work, design-time data, or identity-sensitive lists.

## Read the Projection suite

Start at
[`sample/Samples.Projection/ExampleMatrixSample`](https://github.com/adz/Elmish.Avalonia.Glue/tree/main/sample/Samples.Projection/ExampleMatrixSample).

Trace one screen in this order: F# model and update, projection or host, AXAML,
then application startup. Notice where mutable row identity is intentional.

## Read the ElmView suite

Start at
[`sample/Samples.ElmView/ExampleMatrixSample`](https://github.com/adz/Elmish.Avalonia.Glue/tree/main/sample/Samples.ElmView/ExampleMatrixSample).

Trace the same screen through its F# view record, `AppHost`, write-back routes,
AXAML, and design snapshot. The host should look repetitive because it is
mechanical infrastructure.

## Run the focused documentation examples

The smaller [runnable examples](../examples/index.html) isolate snapshot
hosting, write-back, and keyed patching. Their source project is built before
FsLiveDocs renders the observed output.
