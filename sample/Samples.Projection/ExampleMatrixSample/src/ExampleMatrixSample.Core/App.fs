namespace ExampleMatrixSample.Core

open System
open System.Net.Http
open System.Text.Json
open System.Timers
open Elmish

[<RequireQualifiedAccess>]
module StaticLayoutPage =

    type Metric =
        {
            Id: Guid
            Label: string
            Value: string
            Accent: string
        }

    type Card =
        {
            Id: Guid
            Title: string
            Body: string
            Footer: string
        }

    type Model =
        {
            Heading: string
            Summary: string
            Metrics: Metric list
            Cards: Card list
        }

    let designModel =
        {
            Heading = "Static layout equivalent"
            Summary = "A fixed dashboard-like composition using ordinary Avalonia layout primitives and projection-backed sample data."
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
                        Title = "Status board"
                        Body = "The sample keeps layout data in immutable F# records while AXAML stays entirely standard and previewable."
                        Footer = "No runtime interaction required."
                    }
                    {
                        Id = Guid.Parse("55555555-5555-5555-5555-555555555555")
                        Title = "Reviewability"
                        Body = "A visual tweak is mostly a data or AXAML change rather than a deeper rewrite of projection mechanics."
                        Footer = "Projection code remains shallow."
                    }
                    {
                        Id = Guid.Parse("66666666-6666-6666-6666-666666666666")
                        Title = "Design-time"
                        Body = "The page renders from design snapshots first, then the Elmish host replaces them when the app boots."
                        Footer = "Preview-friendly by default."
                    }
                ]
        }

    let init () = designModel, Cmd.none

[<RequireQualifiedAccess>]
module FormPage =

    type Model =
        {
            Name: string
            Email: string
            Newsletter: bool
            FavoriteLanguage: string
            Experience: int
            Notes: string
            Validation: string option
            SubmittedSummary: string option
        }

    type Msg =
        | SetName of string
        | SetEmail of string
        | SetNewsletter of bool
        | SetFavoriteLanguage of string
        | SetExperience of int
        | SetNotes of string
        | Submit

    let languages =
        [ "F#"; "C#"; "Rust"; "TypeScript" ]

    let private validate model =
        if String.IsNullOrWhiteSpace model.Name then
            Some "Enter a name before submitting."
        elif String.IsNullOrWhiteSpace model.Email || not (model.Email.Contains("@")) then
            Some "Enter an email address that looks valid."
        elif String.IsNullOrWhiteSpace model.FavoriteLanguage then
            Some "Choose a favorite language."
        else
            None

    let private summary model =
        let newsletter =
            if model.Newsletter then
                "Subscribed to follow-up notes."
            else
                "No follow-up subscription."

        $"{model.Name} ({model.Email}) prefers {model.FavoriteLanguage}, rates experience at {model.Experience}/10, and says: {model.Notes} {newsletter}"

    let designModel =
        {
            Name = "Morgan Lee"
            Email = "morgan@example.com"
            Newsletter = true
            FavoriteLanguage = "F#"
            Experience = 7
            Notes = "Interested in comparing projection ergonomics with immutable view data."
            Validation = None
            SubmittedSummary = Some "Morgan Lee (morgan@example.com) prefers F#, rates experience at 7/10, and says: Interested in comparing projection ergonomics with immutable view data. Subscribed to follow-up notes."
        }

    let init () =
        {
            Name = ""
            Email = ""
            Newsletter = true
            FavoriteLanguage = "F#"
            Experience = 5
            Notes = ""
            Validation = None
            SubmittedSummary = None
        },
        Cmd.none

    let update msg model =
        let next =
            match msg with
            | SetName value -> { model with Name = value; Validation = None }
            | SetEmail value -> { model with Email = value; Validation = None }
            | SetNewsletter value -> { model with Newsletter = value }
            | SetFavoriteLanguage value -> { model with FavoriteLanguage = value; Validation = None }
            | SetExperience value -> { model with Experience = value }
            | SetNotes value -> { model with Notes = value }
            | Submit ->
                match validate model with
                | Some error -> { model with Validation = Some error; SubmittedSummary = None }
                | None -> { model with Validation = None; SubmittedSummary = Some (summary model) }

        next, Cmd.none

[<RequireQualifiedAccess>]
module DicePage =

    type Roll =
        {
            Id: Guid
            Left: int
            Right: int
            Total: int
            Caption: string
        }

    type Model =
        {
            LastRoll: Roll option
            History: Roll list
            TotalRolls: int
            Status: string
            IsRolling: bool
        }

    type Msg =
        | RollRequested
        | Rolled of int * int

    let private rollDice () =
        Random.Shared.Next(1, 7), Random.Shared.Next(1, 7)

    let private mkRoll left right =
        let total = left + right

        {
            Id = Guid.NewGuid()
            Left = left
            Right = right
            Total = total
            Caption = $"Rolled {left} + {right} = {total}"
        }

    let designModel =
        let latest = mkRoll 5 6

        {
            LastRoll = Some latest
            History =
                [
                    latest
                    mkRoll 2 4
                    mkRoll 3 3
                ]
            TotalRolls = 3
            Status = "The last throw landed on an 11."
            IsRolling = false
        }

    let init () =
        {
            LastRoll = None
            History = []
            TotalRolls = 0
            Status = "Press Roll dice to generate a new random result."
            IsRolling = false
        },
        Cmd.none

    let update msg model =
        match msg with
        | RollRequested ->
            let cmd = Cmd.OfFunc.perform rollDice () Rolled
            { model with IsRolling = true; Status = "Rolling..." }, cmd
        | Rolled (left, right) ->
            let roll = mkRoll left right

            {
                model with
                    LastRoll = Some roll
                    History = roll :: model.History
                    TotalRolls = model.TotalRolls + 1
                    Status = $"Generated a new random roll totaling {roll.Total}."
                    IsRolling = false
            },
            Cmd.none

[<RequireQualifiedAccess>]
module HttpPage =

    type Model =
        {
            Status: string
            PayloadTitle: string
            PayloadState: string
            LastUpdated: string
            Error: string option
            IsLoading: bool
        }

    type Msg =
        | RefreshRequested
        | Loaded of string * string * DateTimeOffset
        | Failed of string

    let private httpClient = new HttpClient()

    let private fetchTodo () =
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

    let designModel =
        {
            Status = "Fetched a sample JSON payload."
            PayloadTitle = "delectus aut autem"
            PayloadState = "still open"
            LastUpdated = "Preview snapshot"
            Error = None
            IsLoading = false
        }

    let init () =
        {
            Status = "Waiting for the first HTTP request."
            PayloadTitle = "No payload yet"
            PayloadState = "-"
            LastUpdated = "Never"
            Error = None
            IsLoading = false
        },
        Cmd.ofMsg RefreshRequested

    let update msg model =
        match msg with
        | RefreshRequested ->
            let cmd =
                Cmd.OfAsync.either
                    fetchTodo
                    ()
                    (fun (title, state, fetchedAt) -> Loaded(title, state, fetchedAt))
                    (fun ex -> Failed ex.Message)

            { model with Status = "Fetching https://jsonplaceholder.typicode.com/todos/1 ..."; Error = None; IsLoading = true }, cmd
        | Loaded (title, state, fetchedAt) ->
            {
                model with
                    Status = "HTTP request succeeded."
                    PayloadTitle = title
                    PayloadState = state
                    LastUpdated = fetchedAt.ToString("HH:mm:ss")
                    Error = None
                    IsLoading = false
            },
            Cmd.none
        | Failed error ->
            { model with Status = "HTTP request failed."; Error = Some error; IsLoading = false }, Cmd.none

[<RequireQualifiedAccess>]
module ClockPage =

    type Model =
        {
            TimeText: string
            DateText: string
            ZoneText: string
            TickCount: int
            Use24Hour: bool
        }

    type Msg =
        | Tick of DateTimeOffset
        | ToggleFormat

    let private zoneText =
        $"{TimeZoneInfo.Local.DisplayName} ({TimeZoneInfo.Local.StandardName})"

    let private render (now: DateTimeOffset) use24Hour tickCount =
        let timeFormat = if use24Hour then "HH:mm:ss" else "hh:mm:ss tt"

        {
            TimeText = now.ToString(timeFormat)
            DateText = now.ToString("dddd, dd MMMM yyyy")
            ZoneText = zoneText
            TickCount = tickCount
            Use24Hour = use24Hour
        }

    let designModel =
        render (DateTimeOffset(2026, 4, 14, 16, 35, 12, TimeSpan.FromHours(9.5))) true 42

    let init now =
        render now true 0

    let update msg model =
        match msg with
        | Tick now -> render now model.Use24Hour (model.TickCount + 1), Cmd.none
        | ToggleFormat -> render DateTimeOffset.Now (not model.Use24Hour) model.TickCount, Cmd.none

[<RequireQualifiedAccess>]
module FilesPage =

    type SelectedFile =
        {
            Name: string
            Kind: string
            SizeText: string
            Preview: string
            Location: string
        }

    type Model =
        {
            Status: string
            Selected: SelectedFile option
        }

    type Msg =
        | FileOpened of SelectedFile
        | ClearSelection

    let designModel =
        {
            Status = "Previewing a design-time file selection."
            Selected =
                Some
                    {
                        Name = "notes.txt"
                        Kind = ".txt"
                        SizeText = "312 bytes"
                        Preview = "Projection samples can still route platform file access through tiny C# seams while keeping authored state in F#."
                        Location = "/samples/notes.txt"
                    }
        }

    let init () =
        { Status = "Use Open file to inspect a local text-like file." ; Selected = None }, Cmd.none

    let update msg model =
        match msg with
        | FileOpened file ->
            { model with Status = $"Loaded {file.Name}."; Selected = Some file }, Cmd.none
        | ClearSelection ->
            { model with Status = "Selection cleared."; Selected = None }, Cmd.none

[<RequireQualifiedAccess>]
module SvgPage =

    type Theme =
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

    type Model =
        {
            ThemeIndex: int
            Theme: Theme
        }

    type Msg =
        | NextTheme

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

    let designModel =
        {
            ThemeIndex = 1
            Theme = themes[1]
        }

    let init () =
        {
            ThemeIndex = 0
            Theme = themes[0]
        },
        Cmd.none

    let update msg model =
        match msg with
        | NextTheme ->
            let nextIndex = (model.ThemeIndex + 1) % themes.Length
            { ThemeIndex = nextIndex; Theme = themes[nextIndex] }, Cmd.none

[<RequireQualifiedAccess>]
module App =

    type Example =
        | StaticLayout
        | UserInput
        | RandomDice
        | Http
        | Clock
        | Files
        | Svg

    type ExampleInfo =
        {
            Example: Example
            Key: string
            Title: string
            Subtitle: string
            NavTitle: string
            NavCaption: string
        }

    type Model =
        {
            CurrentExample: Example
            StaticLayout: StaticLayoutPage.Model
            UserInput: FormPage.Model
            RandomDice: DicePage.Model
            Http: HttpPage.Model
            Clock: ClockPage.Model
            Files: FilesPage.Model
            Svg: SvgPage.Model
        }

    type Msg =
        | Navigate of Example
        | FormMsg of FormPage.Msg
        | DiceMsg of DicePage.Msg
        | HttpMsg of HttpPage.Msg
        | ClockMsg of ClockPage.Msg
        | FilesMsg of FilesPage.Msg
        | SvgMsg of SvgPage.Msg

    let pageInfos =
        [
            { Example = StaticLayout; Key = "HTML"; Title = "Static layout"; Subtitle = "Fixed composition, cards, and hierarchy using ordinary Avalonia layout."; NavTitle = "Static"; NavCaption = "HTML/layout" }
            { Example = UserInput; Key = "FORM"; Title = "User input"; Subtitle = "Two-way fields, validation, toggles, and a projection-backed submit flow."; NavTitle = "Input"; NavCaption = "Forms" }
            { Example = RandomDice; Key = "RAND"; Title = "Random dice"; Subtitle = "Randomized commands driven through Elmish messages and a stable history list."; NavTitle = "Dice"; NavCaption = "Random" }
            { Example = Http; Key = "HTTP"; Title = "HTTP"; Subtitle = "Async network request state projected into normal bindings."; NavTitle = "HTTP"; NavCaption = "Async fetch" }
            { Example = Clock; Key = "TIME"; Title = "Time / clock"; Subtitle = "A ticking subscription that updates view data every second."; NavTitle = "Clock"; NavCaption = "Subscription" }
            { Example = Files; Key = "FILE"; Title = "Files"; Subtitle = "Platform file picking bridged through a minimal Avalonia-specific seam."; NavTitle = "Files"; NavCaption = "Storage" }
            { Example = Svg; Key = "SVG"; Title = "SVG-equivalent"; Subtitle = "Basic vector-like composition using Avalonia shapes and theme data from F#."; NavTitle = "Vector"; NavCaption = "Shapes" }
        ]

    let pageInfo page =
        pageInfos |> List.find (fun info -> info.Example = page)

    let designModel =
        {
            CurrentExample = StaticLayout
            StaticLayout = StaticLayoutPage.designModel
            UserInput = FormPage.designModel
            RandomDice = DicePage.designModel
            Http = HttpPage.designModel
            Clock = ClockPage.designModel
            Files = FilesPage.designModel
            Svg = SvgPage.designModel
        }

    let getDesignModel () = designModel

    let init () =
        let staticLayout, _ = StaticLayoutPage.init ()
        let form, formCmd = FormPage.init ()
        let dice, diceCmd = DicePage.init ()
        let http, httpCmd = HttpPage.init ()
        let clock = ClockPage.init DateTimeOffset.Now
        let files, filesCmd = FilesPage.init ()
        let svg, svgCmd = SvgPage.init ()

        {
            CurrentExample = StaticLayout
            StaticLayout = staticLayout
            UserInput = form
            RandomDice = dice
            Http = http
            Clock = clock
            Files = files
            Svg = svg
        },
        Cmd.batch
            [
                Cmd.map FormMsg formCmd
                Cmd.map DiceMsg diceCmd
                Cmd.map HttpMsg httpCmd
                Cmd.map FilesMsg filesCmd
                Cmd.map SvgMsg svgCmd
            ]

    let update msg model =
        match msg with
        | Navigate example ->
            { model with CurrentExample = example }, Cmd.none
        | FormMsg subMsg ->
            let next, cmd = FormPage.update subMsg model.UserInput
            { model with UserInput = next }, Cmd.map FormMsg cmd
        | DiceMsg subMsg ->
            let next, cmd = DicePage.update subMsg model.RandomDice
            { model with RandomDice = next }, Cmd.map DiceMsg cmd
        | HttpMsg subMsg ->
            let next, cmd = HttpPage.update subMsg model.Http
            { model with Http = next }, Cmd.map HttpMsg cmd
        | ClockMsg subMsg ->
            let next, cmd = ClockPage.update subMsg model.Clock
            { model with Clock = next }, Cmd.map ClockMsg cmd
        | FilesMsg subMsg ->
            let next, cmd = FilesPage.update subMsg model.Files
            { model with Files = next }, Cmd.map FilesMsg cmd
        | SvgMsg subMsg ->
            let next, cmd = SvgPage.update subMsg model.Svg
            { model with Svg = next }, Cmd.map SvgMsg cmd

    let private subscriptions _ =
        [
            [ "clock-tick" ],
            fun dispatch ->
                let timer = new Timer(1000.0)
                timer.AutoReset <- true
                timer.Elapsed.Add(fun _ -> dispatch (ClockMsg(ClockPage.Msg.Tick DateTimeOffset.Now)))
                timer.Start()

                { new IDisposable with
                    member _.Dispose() =
                        timer.Stop()
                        timer.Dispose() }
        ]

    let program =
        Program.mkProgram init update (fun _ _ -> ())
        |> Program.withSubscription subscriptions
