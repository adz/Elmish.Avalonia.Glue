# ElmView V2 Contract

ElmView V2 keeps the authored UI model pure and F#-first while making editable
Avalonia controls look normal again. The contract is:

- F# view types remain plain immutable records that describe UI shape.
- AXAML remains ordinary Avalonia AXAML with ordinary binding paths.
- A generated bindable node graph sits between AXAML and the immutable snapshot.
- `Mode=OneWay` remains display-only and never dispatches.
- `Mode=TwoWay` becomes the standard contract for editable fields.
- Write-back routing is defined centrally near host construction instead of in
  code-behind, ad hoc event handlers, or F# record metadata.

## Authored Surface

ElmView-authored features continue to put the important UI shaping logic in:

- immutable F# view records
- F# design-time sample snapshots
- normal `.axaml`

They should not require:

- custom UI DSLs
- F# binding attributes or per-property annotations
- handwritten projection trees as the default path

## Bindable Runtime Surface

Avalonia does not bind directly to the immutable records. ElmView V2 instead
projects the current snapshot into a generated bindable node graph:

- the root host exposes a generated root node
- nested records appear as nested bindable child nodes
- node getters read from the current immutable snapshot
- writable properties on generated nodes provide the Avalonia-facing `TwoWay`
  seam

This keeps AXAML binding paths ordinary, such as `UserInput.Name`, while the
runtime stays identity-aware and snapshot-based underneath.

## Binding Contract

ElmView V2 uses standard Avalonia binding modes as the primary authoring signal:

- `OneWay` means snapshot display only
- `TwoWay` means editable field with generated write-back support

The intent is that common controls such as `TextBox`, `CheckBox`, `ComboBox`,
`Slider`, and multiline text inputs bind with normal Avalonia properties rather
than imperative event-forwarding code.

## Write-Back Contract

All write-back mapping is centralized in one host-side configuration surface.
That mapping is responsible for turning generated property sets into explicit
Elmish messages.

The contract is:

- AXAML declares normal bindings
- generated setters call into the centralized mapping layer
- the mapping layer dispatches explicit Elmish messages
- snapshot refresh then updates the generated node graph without re-dispatching

This keeps message routing explicit while removing repetitive control-specific
glue from views.

ElmView V2 chooses a fully explicit mapping model rather than a convention-first
one. Editable properties dispatch only when their property path has been
registered near host construction. There is no implicit fallback based on
property names or message naming conventions.

## Avalonia-First Constraints

ElmView V2 must preserve the repository's Avalonia-first rules:

- standard `.axaml`
- standard bindings and binding modes
- standard design-time preview
- standard DevTools inspection

Design-time preview should use the same generated host shape, or as close to it
as practical, backed by F# sample snapshots rather than a live Elmish runtime.
