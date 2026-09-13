#!/usr/bin/env bash
# Packs every published library into artifacts/package. Pass -v <version> to stamp a release version.
set -euo pipefail

root_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$root_dir"

output_dir="artifacts/package"
mkdir -p "$output_dir"
find "$output_dir" -maxdepth 1 -type f \( -name '*.nupkg' -o -name '*.snupkg' \) -delete

VERSION=""
while getopts "v:" opt; do
  case $opt in
    v) VERSION="$OPTARG" ;;
    *) echo "Usage: $0 [-v <version>]"; exit 1 ;;
  esac
done

version_args=()
if [[ -n "$VERSION" ]]; then
  version_args+=("-p:Version=$VERSION")
fi

projects=(
  "src/Elmish.Glue.Core/Elmish.Glue.Core.fsproj"
  "src/Elmish.Avalonia.Glue/Elmish.Avalonia.Glue.fsproj"
  "src/Elmish.Avalonia.Glue.Projection/Elmish.Avalonia.Glue.Projection.fsproj"
  "src/Elmish.Avalonia.Glue.ElmView/Elmish.Avalonia.Glue.ElmView.fsproj"
)

# One project at a time: packing projects that share Elmish.Glue.Core in parallel races on its outputs.
for project in "${projects[@]}"; do
  echo "--- Packing $(basename "$project") ---"
  dotnet pack "$project" --configuration Release --output "$output_dir" -m:1 "${version_args[@]}"
done

ls -1 "$output_dir"
