#!/usr/bin/env bash
set -euo pipefail

root_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$root_dir"

dotnet run --project tools/DocsExamples/DocsExamples.fsproj
dotnet livedocs test --interactive false --banner false
dotnet livedocs build --interactive false --banner false

if rg -U '<li data-sidebar-item="true"><a [^>]*>\s*</a></li>' output; then
  echo "FsLiveDocs generated a sidebar item without a label." >&2
  exit 1
fi
