#!/usr/bin/env bash
set -euo pipefail

root_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$root_dir"

# FsLiveDocs extracts API docs from built assemblies, so every documented project
# must be compiled before the first livedocs command runs.
dotnet build Elmish.Avalonia.Glue.Build.slnf --nologo -v minimal

dotnet run --project tools/DocsExamples/DocsExamples.fsproj
dotnet livedocs test --interactive false --banner false
dotnet livedocs build --interactive false --banner false

bash scripts/check-package-maturity.sh

if rg -U '<li data-sidebar-item="true"><a [^>]*>\s*</a></li>' output; then
  echo "FsLiveDocs generated a sidebar item without a label." >&2
  exit 1
fi
