# Package maturity

Every published package carries a maturity level: **Experimental**,
**Incubating**, or **Stable**. The level is stamped into the package metadata
at pack time: a `maturity-<level>` tag is appended to `PackageTags`, and the
package `Description` is prefixed with `[<Level>]`.

## Levels

| Level | Meaning |
| --- | --- |
| **Experimental** | No real application uses the package. It may change or be removed in any release. |
| **Incubating** | At least one real application uses it (directly, or through a package it references), and it is pre-1.0 or used by fewer than two real applications. The API may still change in minor releases. |
| **Stable** | Version 1.0 or later **and** used by at least two real applications. Semantic versioning and deprecation cycles apply. |

## Promotion

A package moves:

- **Experimental → Incubating** when the first real application starts using it.
- **Incubating → Stable** when it reaches version 1.0 or later **and** at least
  two real applications use it.

Promotion is a manual, documented decision. Update the package's
`<PackageMaturity>` in its `.fsproj` and the table below in the same change.
[`scripts/check-package-maturity.sh`](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/scripts/check-package-maturity.sh)
fails the docs build when the two drift out of sync.

## What counts as a real application

A *real application* is an application in active use **outside this
repository**. The two currently known real applications are **gitkay** and
**Logs Digger**.

Examples, samples, tests and reference apps do **not** count.

## Package maturity table

| Package | Maturity | Evidence |
| --- | --- | --- |
| `Elmish.Avalonia.Glue` | Incubating | gitkay, Logs Digger |
| `Elmish.Glue.Core` | Incubating | gitkay |
| `Elmish.Avalonia.Glue.Projection` | Incubating | Logs Digger |
| `Elmish.Avalonia.Glue.ElmView` | Experimental | — |
| `Elmish.Avalonia.Glue.Testing` | Experimental | — |
