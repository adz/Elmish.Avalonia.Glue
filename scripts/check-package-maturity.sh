#!/usr/bin/env bash
# Verifies the package maturity table in docs/guides/16-package-maturity.md matches the
# <PackageMaturity> value in each packable .fsproj under src/.
set -euo pipefail

root_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$root_dir"

docs_page="docs/guides/16-package-maturity.md"
failures=0

for fsproj in src/*/*.fsproj; do
  maturity="$(sed -n 's/.*<PackageMaturity>\([^<]*\)<\/PackageMaturity>.*/\1/p' "$fsproj" | head -n1)"
  # Only packable projects set PackageMaturity; skip everything else.
  [[ -z "$maturity" ]] && continue

  pkgid="$(sed -n 's/.*<PackageId>\([^<]*\)<\/PackageId>.*/\1/p' "$fsproj" | head -n1)"
  if ! grep -Fq "\`$pkgid\` | $maturity" "$docs_page"; then
    echo "ERROR: docs package-maturity table lacks a row for '$pkgid | $maturity' (from $fsproj)" >&2
    failures=1
  fi
done

if (( failures )); then
  echo "Package maturity docs are out of sync with the .fsproj values." >&2
  exit 1
fi

echo "Package maturity docs and .fsproj values are in sync."
