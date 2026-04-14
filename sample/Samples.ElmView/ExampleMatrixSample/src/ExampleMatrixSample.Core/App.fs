namespace ExampleMatrixSample.ElmView.Core

open System
open System.Net.Http
open System.Text.Json
open System.Timers
open Elmish

type Example =
    | StaticLayout
    | UserInput
    | RandomDice
    | Http
    | Clock
    | Files
    | Svg

type NavigationItemView =
    {
        Example: Example
        Key: string
        Title: string
        Caption: string
        IsSelected: bool
        Background: string
        Foreground: string
    }

type StaticMetricView =
    {
        Id: Guid
        Label: string
        Value: string
        Accent: string
    }

type StaticCardView =
    {
        Id: Guid
        Title: string
        Body: string
        Footer: string
    }

type StaticLayoutView =
    {
        Heading: string
        Summary: string
        Metrics: StaticMetricView list
        Cards: StaticCardView list
    }

type FormView =
    {
        Name: string
        Email: string
        Newsletter: bool
        FavoriteLanguage: string
        Languages: string list
        Experience: int
        Notes: string
        ValidationText: string
        HasValidation: bool
        SummaryText: string
        HasSummary: bool
    }

type DiceRollView =
    {
        Id: Guid
        Caption: string
        Total: int
    }

type DiceView =
    {
        Status: string
        IsRolling: bool
        HasLastRoll: bool
        LastRollLeft: int
        LastRollRight: int
        LastRollTotal: int
        LastRollCaption: string
        TotalRolls: int
        History: DiceRollView list
    }

type HttpView =
    {
        Status: string
        PayloadTitle: string
        PayloadState: string
        LastUpdated: string
        ErrorText: string
        HasError: bool
        IsLoading: bool
    }

type ClockView =
    {
        TimeText: string
        DateText: string
        ZoneText: string
        TickCount: int
        Use24Hour: bool
        FormatLabel: string
    }

type SelectedFileView =
    {
        Name: string
        Kind: string
        SizeText: string
        Preview: string
        Location: string
    }

type FilesView =
    {
        Status: string
        HasSelection: bool
        SelectedName: string
        SelectedKind: string
        SelectedSizeText: string
        SelectedPreview: string
        SelectedLocation: string
    }

type SvgThemeView =
    {
        Name: string
        Caption: string
        Background: string
        GridLine: string
        Sky: string
        Sun: string
        Hill: string
        Accent: string
        Rocket: string
        Trail: string
    }

type SvgView =
    {
        ThemeIndex: int
        ThemeCount: int
        Theme: SvgThemeView
    }

type AppView =
    {
        CurrentExample: Example
        HeaderTitle: string
        HeaderSubtitle: string
        HeaderKey: string
        Navigation: NavigationItemView list
        ShowStaticLayout: bool
        ShowUserInput: bool
        ShowRandomDice: bool
        ShowHttp: bool
        ShowClock: bool
        ShowFiles: bool
        ShowSvg: bool
        StaticLayout: StaticLayoutView
        UserInput: FormView
        RandomDice: DiceView
        Http: HttpView
        Clock: ClockView
        Files: FilesView
        Svg: SvgView
    }

type Msg =
    | Navigate of Example
    | SetName of string
    | SetEmail of string
    | SetNewsletter of bool
    | SetFavoriteLanguage of string
    | SetExperience of int
    | SetNotes of string
    | SubmitForm
    | RollDice
    | DiceRolled of int * int
    | RefreshHttp
    | HttpLoaded of string * string * DateTimeOffset
    | HttpFailed of string
    | ClockTick of DateTimeOffset
    | ToggleClockFormat
    | FileOpened of SelectedFileView
    | ClearFileSelection
    | NextSvgTheme

type private ExampleInfo =
    {
        Example: Example
        Key: string
        Title: string
        Subtitle: string
        NavTitle: string
        NavCaption: string
    }

module private ExampleInfo =
    let all =
        [
            { Example = StaticLayout; Key = "HTML"; Title = "Static layout"; Subtitle = "Fixed composition, cards, and hierarchy using ordinary Avalonia layout."; NavTitle = "Static"; NavCaption = "HTML/layout" }
            { Example = UserInput; Key = "FORM"; Title = "User input"; Subtitle = "Editable fields, validation, toggles, and submit flow with immutable view snapshots."; NavTitle = "Input"; NavCaption = "Forms" }
            { Example = RandomDice; Key = "RAND"; Title = "Random dice"; Subtitle = "Randomized commands driven through Elmish messages and a stable history list."; NavTitle = "Dice"; NavCaption = "Random" }
            { Example = Http; Key = "HTTP"; Title = "HTTP"; Subtitle = "Async network request state rendered into the ElmView host."; NavTitle = "HTTP"; NavCaption = "Async fetch" }
            { Example = Clock; Key = "TIME"; Title = "Time / clock"; Subtitle = "A ticking subscription updates immutable view records every second."; NavTitle = "Clock"; NavCaption = "Subscription" }
            { Example = Files; Key = "FILE"; Title = "Files"; Subtitle = "Platform file picking bridged through a minimal Avalonia-specific seam."; NavTitle = "Files"; NavCaption = "Storage" }
            { Example = Svg; Key = "SVG"; Title = "SVG-equivalent"; Subtitle = "Basic vector-like composition using Avalonia shapes and theme data from F#."; NavTitle = "Vector"; NavCaption = "Shapes" }
        ]

    let byExample example =
        all |> List.find (fun info -> info.Example = example)

module private StaticLayoutPage =
    let design =
        {
            Heading = "Static layout equivalent"
            Summary = "A fixed dashboard-like composition using ordinary Avalonia layout primitives with immutable ElmView records."
            Metrics =
                [
                    { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"); Label = "Open incidents"; Value = "3"; Accent = "#8C4B2F" }
                    { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"); Label = "Recovery SLA"; Value = "97.2%"; Accent = "#2D5B4F" }
                    { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"); Label = "Pending approvals"; Value = "14"; Accent = "#355C7D" }
                ]
            Cards =
                [
                    {
                        Id = Guid.Parse("44444444-4444-4444-4444-444444444444")
                        Title = "ElmView schema"
                        Body = "The authored UI shape lives in immutable F# records while AXAML remains ordinary and previewable."
                        Footer = "No handwritten projection tree."
                    }
                    {
                        Id = Guid.Parse("55555555-5555-5555-5555-555555555555")
                        Title = "Reviewability"
                        Body = "A visual tweak is typically a record or AXAML edit rather than a deep rewrite of CLR-facing viewmodels."
                        Footer = "Shallow host, explicit data."
                    }
                    {
                        Id = Guid.Parse("66666666-6666-6666-6666-666666666666")
                        Title = "Design-time"
                        Body = "The same view shape feeds preview snapshots and the runtime Elmish host."
                        Footer = "Preview-friendly by default."
                    }
                ]
        }

module private FormPage =
    let languages = [ "F#"; "C#"; "Rust"; "TypeScript" ]

    let private validate (view: FormView) =
        if String.IsNullOrWhiteSpace view.Name then
            "Enter a name before submitting."
        elif String.IsNullOrWhiteSpace view.Email || not (view.Email.Contains("@")) then
            "Enter an email address that looks valid."
        elif String.IsNullOrWhiteSpace view.FavoriteLanguage then
            "Choose a favorite language."
        else
            ""

    let private summary (view: FormView) =
        let newsletter =
            if view.Newsletter then
                "Subscribed to follow-up notes."
            else
                "No follow-up subscription."

        $"{view.Name} ({view.Email}) prefers {view.FavoriteLanguage}, rates experience at {view.Experience}/10, and says: {view.Notes} {newsletter}"

    let design =
        {
            Name = "Morgan Lee"
            Email = "morgan@example.com"
            Newsletter = true
            FavoriteLanguage = "F#"
            Languages = languages
            Experience = 7
            Notes = "Interested in comparing projection ergonomics with immutable view data."
            ValidationText = ""
            HasValidation = false
            SummaryText = "Morgan Lee (morgan@example.com) prefers F#, rates experience at 7/10, and says: Interested in comparing projection ergonomics with immutable view data. Subscribed to follow-up notes."
            HasSummary = true
        }

    let init =
        {
            Name = ""
            Email = ""
            Newsletter = true
            FavoriteLanguage = "F#"
            Languages = languages
            Experience = 5
            Notes = ""
            ValidationText = ""
            HasValidation = false
            SummaryText = ""
            HasSummary = false
        }

    let submit (view: FormView) =
        let error = validate view

        if String.IsNullOrWhiteSpace error then
            { view with ValidationText = ""; HasValidation = false; SummaryText = summary view; HasSummary = true }
        else
            { view with ValidationText = error; HasValidation = true; SummaryText = ""; HasSummary = false }

module private DicePage =
    let private mkRoll left right : DiceRollView =
        let total = left + right

        {
            Id = Guid.NewGuid()
            Caption = $"Rolled {left} + {right} = {total}"
            Total = total
        }

    let private applyLastRoll left right (roll: DiceRollView) (view: DiceView) =
        {
            view with
                HasLastRoll = true
                LastRollLeft = left
                LastRollRight = right
                LastRollTotal = left + right
                LastRollCaption = roll.Caption
                TotalRolls = view.TotalRolls + 1
                History = roll :: view.History
                Status = $"Generated a new random roll totaling {roll.Total}."
                IsRolling = false
        }

    let design =
        let latest = mkRoll 5 6

        {
            Status = "The last throw landed on an 11."
            IsRolling = false
            HasLastRoll = true
            LastRollLeft = 5
            LastRollRight = 6
            LastRollTotal = 11
            LastRollCaption = latest.Caption
            TotalRolls = 3
            History = [ latest; mkRoll 2 4; mkRoll 3 3 ]
        }

    let init =
        {
            Status = "Press Roll dice to generate a new random result."
            IsRolling = false
            HasLastRoll = false
            LastRollLeft = 0
            LastRollRight = 0
            LastRollTotal = 0
            LastRollCaption = ""
            TotalRolls = 0
            History = []
        }

    let rollCommand () = Random.Shared.Next(1, 7), Random.Shared.Next(1, 7)

    let startRolling (view: DiceView) =
        { view with Status = "Rolling..."; IsRolling = true }

    let finishRolling left right (view: DiceView) =
        let roll = mkRoll left right
        applyLastRoll left right roll view

module private HttpPage =
    let private httpClient = new HttpClient()

    let design =
        {
            Status = "Fetched a sample JSON payload."
            PayloadTitle = "delectus aut autem"
            PayloadState = "still open"
            LastUpdated = "Preview snapshot"
            ErrorText = ""
            HasError = false
            IsLoading = false
        }

    let init =
        {
            Status = "Waiting for the first HTTP request."
            PayloadTitle = "No payload yet"
            PayloadState = "-"
            LastUpdated = "Never"
            ErrorText = ""
            HasError = false
            IsLoading = false
        }

    let fetchTodo () =
        async {
            use! response = httpClient.GetAsync("https://jsonplaceholder.typicode.com/todos/1") |> Async.AwaitTask
            response.EnsureSuccessStatusCode() |> ignore
            let! json = response.Content.ReadAsStringAsync() |> Async.AwaitTask
            use document = JsonDocument.Parse(json)
            let title = document.RootElement.GetProperty("title").GetString() |> Option.ofObj |> Option.defaultValue "(missing title)"
            let completed = document.RootElement.GetProperty("completed").GetBoolean()
            let state = if completed then "completed" else "still open"
            return title, state, DateTimeOffset.Now
        }

    let startLoading (view: HttpView) =
        { view with Status = "Fetching https://jsonplaceholder.typicode.com/todos/1 ..."; ErrorText = ""; HasError = false; IsLoading = true }

    let loaded title state (fetchedAt: DateTimeOffset) (view: HttpView) =
        {
            view with
                Status = "HTTP request succeeded."
                PayloadTitle = title
                PayloadState = state
                LastUpdated = fetchedAt.ToString("HH:mm:ss")
                ErrorText = ""
                HasError = false
                IsLoading = false
        }

    let failed error (view: HttpView) =
        { view with Status = "HTTP request failed."; ErrorText = error; HasError = true; IsLoading = false }

module private ClockPage =
    let private zoneText =
        $"{TimeZoneInfo.Local.DisplayName} ({TimeZoneInfo.Local.StandardName})"

    let render (now: DateTimeOffset) use24Hour tickCount : ClockView =
        let timeFormat = if use24Hour then "HH:mm:ss" else "hh:mm:ss tt"

        {
            TimeText = now.ToString(timeFormat)
            DateText = now.ToString("dddd, dd MMMM yyyy")
            ZoneText = zoneText
            TickCount = tickCount
            Use24Hour = use24Hour
            FormatLabel = if use24Hour then "Switch to 12-hour" else "Switch to 24-hour"
        }

    let design =
        render (DateTimeOffset(2026, 4, 14, 16, 35, 12, TimeSpan.FromHours(9.5))) true 42

    let init () = render DateTimeOffset.Now true 0

module private FilesPage =
    let private selectedFields (selected: SelectedFileView option) =
        match selected with
        | Some file ->
            true, file.Name, file.Kind, file.SizeText, file.Preview, file.Location
        | None -> false, "", "", "", "", ""

    let private render status (selected: SelectedFileView option) : FilesView =
        let hasSelection, name, kind, sizeText, preview, location = selectedFields selected

        {
            Status = status
            HasSelection = hasSelection
            SelectedName = name
            SelectedKind = kind
            SelectedSizeText = sizeText
            SelectedPreview = preview
            SelectedLocation = location
        }

    let design =
        render
            "Previewing a design-time file selection."
            (Some
                {
                    Name = "notes.txt"
                    Kind = ".txt"
                    SizeText = "312 bytes"
                    Preview = "ElmView samples can still route platform file access through a tiny Avalonia-specific seam while keeping authored state in F#."
                    Location = "/samples/notes.txt"
                })

    let init = render "Use Open file to inspect a local text-like file." None

    let opened (file: SelectedFileView) = render $"Loaded {file.Name}." (Some file)

    let cleared = render "Selection cleared." None

module private SvgPage =
    let private themes =
        [|
            {
                Name = "Dawn survey"
                Caption = "An Avalonia shape scene standing in for a simple SVG example."
                Background = "#FFF9F1"
                GridLine = "#E9D9C3"
                Sky = "#F6C28B"
                Sun = "#D96C3A"
                Hill = "#6B8F71"
                Accent = "#355C7D"
                Rocket = "#203245"
                Trail = "#C84C31"
            }
            {
                Name = "Night telemetry"
                Caption = "Same vector composition, different palette, no custom drawing DSL."
                Background = "#101722"
                GridLine = "#213247"
                Sky = "#1E2A3A"
                Sun = "#F2C14E"
                Hill = "#3A6B6F"
                Accent = "#8BD3DD"
                Rocket = "#F5F7FA"
                Trail = "#F08A5D"
            }
            {
                Name = "Studio pastel"
                Caption = "The sample keeps the geometry in AXAML and the theme data in F#."
                Background = "#F8F0FF"
                GridLine = "#E4D7F5"
                Sky = "#D6C2FF"
                Sun = "#A5668B"
                Hill = "#8FC0A9"
                Accent = "#5C5470"
                Rocket = "#3B3355"
                Trail = "#E07A5F"
            }
        |]

    let private render index : SvgView =
        {
            ThemeIndex = index
            ThemeCount = themes.Length
            Theme = themes[index]
        }

    let design = render 1
    let init = render 0
    let next (view: SvgView) = render ((view.ThemeIndex + 1) % themes.Length)

module App =
    let private withDerived (app: AppView) =
        let info = ExampleInfo.byExample app.CurrentExample

        let renderNav current (info: ExampleInfo) : NavigationItemView =
            let isSelected = current = info.Example

            {
                Example = info.Example
                Key = info.Key
                Title = info.NavTitle
                Caption = info.NavCaption
                IsSelected = isSelected
                Background = if isSelected then "#275545" else "#00000000"
                Foreground = if isSelected then "#FFF8EF" else "#D7E4DC"
            }

        {
            app with
                HeaderTitle = info.Title
                HeaderSubtitle = info.Subtitle
                HeaderKey = info.Key
                Navigation = ExampleInfo.all |> List.map (renderNav app.CurrentExample)
                ShowStaticLayout = app.CurrentExample = StaticLayout
                ShowUserInput = app.CurrentExample = UserInput
                ShowRandomDice = app.CurrentExample = RandomDice
                ShowHttp = app.CurrentExample = Http
                ShowClock = app.CurrentExample = Clock
                ShowFiles = app.CurrentExample = Files
                ShowSvg = app.CurrentExample = Svg
        }

    let getDesignView () =
        {
            CurrentExample = StaticLayout
            HeaderTitle = ""
            HeaderSubtitle = ""
            HeaderKey = ""
            Navigation = []
            ShowStaticLayout = false
            ShowUserInput = false
            ShowRandomDice = false
            ShowHttp = false
            ShowClock = false
            ShowFiles = false
            ShowSvg = false
            StaticLayout = StaticLayoutPage.design
            UserInput = FormPage.design
            RandomDice = DicePage.design
            Http = HttpPage.design
            Clock = ClockPage.design
            Files = FilesPage.design
            Svg = SvgPage.design
        }
        |> withDerived

    let init () =
        {
            CurrentExample = StaticLayout
            HeaderTitle = ""
            HeaderSubtitle = ""
            HeaderKey = ""
            Navigation = []
            ShowStaticLayout = false
            ShowUserInput = false
            ShowRandomDice = false
            ShowHttp = false
            ShowClock = false
            ShowFiles = false
            ShowSvg = false
            StaticLayout = StaticLayoutPage.design
            UserInput = FormPage.init
            RandomDice = DicePage.init
            Http = HttpPage.init
            Clock = ClockPage.init ()
            Files = FilesPage.init
            Svg = SvgPage.init
        }
        |> withDerived,
        Cmd.ofMsg RefreshHttp

    let update msg model =
        match msg with
        | Navigate example ->
            { model with CurrentExample = example } |> withDerived, Cmd.none
        | SetName value ->
            { model with UserInput = { model.UserInput with Name = value; ValidationText = ""; HasValidation = false } }
            |> withDerived,
            Cmd.none
        | SetEmail value ->
            { model with UserInput = { model.UserInput with Email = value; ValidationText = ""; HasValidation = false } }
            |> withDerived,
            Cmd.none
        | SetNewsletter value ->
            { model with UserInput = { model.UserInput with Newsletter = value } } |> withDerived, Cmd.none
        | SetFavoriteLanguage value ->
            { model with UserInput = { model.UserInput with FavoriteLanguage = value; ValidationText = ""; HasValidation = false } }
            |> withDerived,
            Cmd.none
        | SetExperience value ->
            { model with UserInput = { model.UserInput with Experience = value } } |> withDerived, Cmd.none
        | SetNotes value ->
            { model with UserInput = { model.UserInput with Notes = value } } |> withDerived, Cmd.none
        | SubmitForm ->
            { model with UserInput = FormPage.submit model.UserInput } |> withDerived, Cmd.none
        | RollDice ->
            { model with RandomDice = DicePage.startRolling model.RandomDice } |> withDerived,
            Cmd.OfFunc.perform DicePage.rollCommand () DiceRolled
        | DiceRolled (left, right) ->
            { model with RandomDice = DicePage.finishRolling left right model.RandomDice } |> withDerived, Cmd.none
        | RefreshHttp ->
            { model with Http = HttpPage.startLoading model.Http } |> withDerived,
            Cmd.OfAsync.either
                HttpPage.fetchTodo
                ()
                (fun (title, state, fetchedAt) -> HttpLoaded(title, state, fetchedAt))
                (fun ex -> HttpFailed ex.Message)
        | HttpLoaded (title, state, fetchedAt) ->
            { model with Http = HttpPage.loaded title state fetchedAt model.Http } |> withDerived, Cmd.none
        | HttpFailed error ->
            { model with Http = HttpPage.failed error model.Http } |> withDerived, Cmd.none
        | ClockTick now ->
            { model with Clock = ClockPage.render now model.Clock.Use24Hour (model.Clock.TickCount + 1) } |> withDerived, Cmd.none
        | ToggleClockFormat ->
            { model with Clock = ClockPage.render DateTimeOffset.Now (not model.Clock.Use24Hour) model.Clock.TickCount }
            |> withDerived,
            Cmd.none
        | FileOpened file ->
            { model with Files = FilesPage.opened file } |> withDerived, Cmd.none
        | ClearFileSelection ->
            { model with Files = FilesPage.cleared } |> withDerived, Cmd.none
        | NextSvgTheme ->
            { model with Svg = SvgPage.next model.Svg } |> withDerived, Cmd.none

    let private subscriptions _ =
        [
            [ "clock-tick" ],
            fun dispatch ->
                let timer = new Timer(1000.0)
                timer.AutoReset <- true
                timer.Elapsed.Add(fun _ -> dispatch (ClockTick DateTimeOffset.Now))
                timer.Start()

                { new IDisposable with
                    member _.Dispose() =
                        timer.Stop()
                        timer.Dispose() }
        ]

    let program =
        Program.mkProgram init update (fun _ _ -> ())
        |> Program.withSubscription subscriptions
