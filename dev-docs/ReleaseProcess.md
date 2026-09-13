# Release process

All four packages (`Elmish.Glue.Core`, `Elmish.Avalonia.Glue`, `Elmish.Avalonia.Glue.Projection`,
`Elmish.Avalonia.Glue.ElmView`) share one version, taken from the `vX.Y.Z` tag. `VersionPrefix` in
`Directory.Build.props` is only the local default.

## Releasing

1. Set `NEXT_VERSION` and write `dev-docs/releases/<version>.md`. CI fails on `main` until the notes exist.
2. Check locally:

   ```bash
   dotnet build Elmish.Avalonia.Glue.Build.slnf -c Release -m:1
   dotnet test Elmish.Avalonia.Glue.Build.slnf -c Release --no-build
   bash scripts/pack.sh
   ```

3. Commit and push `main`, then tag: `git tag v0.1.0 && git push origin v0.1.0`.

The release workflow builds, tests and packs at the tag, creates the GitHub release with the packages attached,
then publishes to NuGet.org from the protected `nuget` environment. It is safe to re-run: use **Run workflow**
with the version to resume a failed release; existing releases and already-published packages are skipped.

## One-time setup: NuGet trusted publishing

Trusted publishing swaps a stored API key for a short-lived key minted from GitHub's OIDC token.

1. **nuget.org → your username → Trusted Publishing → Create policy**
   - Repository owner: `adz`
   - Repository: `Elmish.Avalonia.Glue`
   - Workflow file: `release.yml` (file name only, not the path)
   - Environment: `nuget`

   A policy for a package ID that doesn't exist yet stays "temporarily active" for 7 days, and becomes
   permanent after the first successful publish. If it lapses before then, edit and re-save it to restart the
   window.
2. **GitHub → repo Settings → Environments → New environment `nuget`.** Optionally add yourself as a required
   reviewer so every publish needs one click, and restrict deployment to tags matching `v*`.
3. **GitHub → repo Settings → Secrets and variables → Actions → Variables → `NUGET_USER`** = your nuget.org
   username (the profile name, not your email).
4. Optionally reserve an ID prefix (nuget.org → Account → prefix reservation request), e.g.
   `Elmish.Avalonia.Glue`. Not needed to publish.

The workflow side is `permissions: id-token: write` plus `NuGet/login@v1` in the `publish-nuget` job.
