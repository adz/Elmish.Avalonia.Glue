#!/usr/bin/env bash
# Fails unless dev-docs/releases/<version>.md exists (version defaults to NEXT_VERSION).
set -euo pipefail
root_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
version="${1:-$(head -n 1 "$root_dir/NEXT_VERSION" | tr -d '[:space:]')}"
notes="$root_dir/dev-docs/releases/$version.md"
if [[ ! -s "$notes" ]]; then
  echo "Missing release notes: dev-docs/releases/$version.md" >&2
  exit 1
fi
echo "Release notes found for $version"
