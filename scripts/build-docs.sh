#!/usr/bin/env bash
set -euo pipefail

root_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$root_dir"

dotnet run --project tools/DocsExamples/DocsExamples.fsproj
dotnet livedocs build --interactive false --banner false
