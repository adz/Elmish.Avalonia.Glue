using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elmish.Glue.Core;
using ExampleMatrixSample.Core;
using Microsoft.FSharp.Core;

namespace ExampleMatrixSample.UI.ViewModels;

using AppCore = ExampleMatrixSample.Core.App;

static class FSharpOptionHelpers
{
    public static T? ValueOrNull<T>(FSharpOption<T>? option) where T : class => option?.Value;
}

public partial class AppProjection : ObservableObject
{
    public AppProjection()
    {
        NavigationItems = new ObservableCollection<NavigationItemProjection>();

        foreach (var info in AppCore.pageInfos)
        {
            NavigationItems.Add(new NavigationItemProjection(info.Example, info.NavTitle, info.NavCaption));
        }
    }

    public ObservableCollection<NavigationItemProjection> NavigationItems { get; }
    public StaticLayoutPageProjection StaticLayout { get; } = new();
    public FormPageProjection UserInput { get; } = new();
    public DicePageProjection RandomDice { get; } = new();
    public HttpPageProjection Http { get; } = new();
    public ClockPageProjection Clock { get; } = new();
    public FilesPageProjection Files { get; } = new();
    public SvgPageProjection Svg { get; } = new();

    [ObservableProperty] public partial object CurrentPage { get; set; } = null!;
    [ObservableProperty] public partial string CurrentTitle { get; set; } = "";
    [ObservableProperty] public partial string CurrentSubtitle { get; set; } = "";
    [ObservableProperty] public partial string CurrentKey { get; set; } = "";

    public void AttachFilePicker(Func<Task<FilesPage.SelectedFile?>> picker) => Files.AttachFilePicker(picker);

    public void Update(AppCore.Model model)
    {
        StaticLayout.Update(model.StaticLayout);
        UserInput.Update(model.UserInput);
        RandomDice.Update(model.RandomDice);
        Http.Update(model.Http);
        Clock.Update(model.Clock);
        Files.Update(model.Files);
        Svg.Update(model.Svg);

        foreach (var item in NavigationItems)
        {
            item.SetSelected(Equals(item.Example, model.CurrentExample));
        }

        var info = AppCore.pageInfo(model.CurrentExample);
        CurrentTitle = info.Title;
        CurrentSubtitle = info.Subtitle;
        CurrentKey = info.Key;

        switch (model.CurrentExample.Tag)
        {
            case 0:
                CurrentPage = StaticLayout;
                break;
            case 1:
                CurrentPage = UserInput;
                break;
            case 2:
                CurrentPage = RandomDice;
                break;
            case 3:
                CurrentPage = Http;
                break;
            case 4:
                CurrentPage = Clock;
                break;
            case 5:
                CurrentPage = Files;
                break;
            default:
                CurrentPage = Svg;
                break;
        }
    }

    public void SetDispatch(Action<AppCore.Msg> dispatch)
    {
        foreach (var item in NavigationItems)
        {
            item.SetNavigate(example => dispatch(AppCore.Msg.NewNavigate(example)));
        }

        UserInput.SetDispatch(msg => dispatch(AppCore.Msg.NewFormMsg(msg)));
        RandomDice.SetDispatch(msg => dispatch(AppCore.Msg.NewDiceMsg(msg)));
        Http.SetDispatch(msg => dispatch(AppCore.Msg.NewHttpMsg(msg)));
        Clock.SetDispatch(msg => dispatch(AppCore.Msg.NewClockMsg(msg)));
        Files.SetDispatch(msg => dispatch(AppCore.Msg.NewFilesMsg(msg)));
        Svg.SetDispatch(msg => dispatch(AppCore.Msg.NewSvgMsg(msg)));
    }
}

public partial class NavigationItemProjection : ObservableObject
{
    private Action<AppCore.Example> _navigate = _ => { };

    public NavigationItemProjection(AppCore.Example example, string title, string caption)
    {
        Example = example;
        Title = title;
        Caption = caption;
        Background = "#00000000";
        Foreground = "#D7E4DC";
    }

    public AppCore.Example Example { get; }
    public string Title { get; }
    public string Caption { get; }

    [ObservableProperty] public partial string Background { get; set; }
    [ObservableProperty] public partial string Foreground { get; set; }

    public void SetNavigate(Action<AppCore.Example> navigate) => _navigate = navigate;

    public void SetSelected(bool isSelected)
    {
        Background = isSelected ? "#275545" : "#00000000";
        Foreground = isSelected ? "#FFF8EF" : "#D7E4DC";
    }

    [RelayCommand]
    private void Navigate() => _navigate(Example);
}

public partial class StaticLayoutPageProjection : ObservableObject
{
    public ObservableCollection<StaticMetricProjection> Metrics { get; } = new();
    public ObservableCollection<StaticCardProjection> Cards { get; } = new();

    [ObservableProperty] public partial string Heading { get; set; } = "";
    [ObservableProperty] public partial string Summary { get; set; } = "";

    public void Update(StaticLayoutPage.Model model)
    {
        Heading = model.Heading;
        Summary = model.Summary;

        Metrics.SyncWith(
            model.Metrics,
            metric => metric.Id,
            vm => vm.Id,
            metric => new StaticMetricProjection(metric),
            (vm, metric) => vm.Update(metric));

        Cards.SyncWith(
            model.Cards,
            card => card.Id,
            vm => vm.Id,
            card => new StaticCardProjection(card),
            (vm, card) => vm.Update(card));
    }
}

public partial class StaticMetricProjection : ObservableObject
{
    public StaticMetricProjection(StaticLayoutPage.Metric model)
    {
        Id = model.Id;
        Update(model);
    }

    public Guid Id { get; }

    [ObservableProperty] public partial string Label { get; set; } = "";
    [ObservableProperty] public partial string Value { get; set; } = "";
    [ObservableProperty] public partial string Accent { get; set; } = "";

    public void Update(StaticLayoutPage.Metric model)
    {
        Label = model.Label;
        Value = model.Value;
        Accent = model.Accent;
    }
}

public partial class StaticCardProjection : ObservableObject
{
    public StaticCardProjection(StaticLayoutPage.Card model)
    {
        Id = model.Id;
        Update(model);
    }

    public Guid Id { get; }

    [ObservableProperty] public partial string Title { get; set; } = "";
    [ObservableProperty] public partial string Body { get; set; } = "";
    [ObservableProperty] public partial string Footer { get; set; } = "";

    public void Update(StaticLayoutPage.Card model)
    {
        Title = model.Title;
        Body = model.Body;
        Footer = model.Footer;
    }
}

public partial class FormPageProjection : ObservableObject
{
    private Action<FormPage.Msg> _dispatch = _ => { };
    private bool _isUpdating;

    public FormPageProjection()
    {
        Languages = new ObservableCollection<string>(FormPage.languages);
    }

    public ObservableCollection<string> Languages { get; }

    [ObservableProperty] public partial string Name { get; set; } = "";
    [ObservableProperty] public partial string Email { get; set; } = "";
    [ObservableProperty] public partial bool Newsletter { get; set; }
    [ObservableProperty] public partial string FavoriteLanguage { get; set; } = "";
    [ObservableProperty] public partial int Experience { get; set; }
    [ObservableProperty] public partial string Notes { get; set; } = "";
    [ObservableProperty] public partial string Validation { get; set; } = "";
    [ObservableProperty] public partial string SubmittedSummary { get; set; } = "";
    [ObservableProperty] public partial bool HasSummary { get; set; }

    public void SetDispatch(Action<FormPage.Msg> dispatch) => _dispatch = dispatch;

    public void Update(FormPage.Model model)
    {
        _isUpdating = true;
        Name = model.Name;
        Email = model.Email;
        Newsletter = model.Newsletter;
        FavoriteLanguage = model.FavoriteLanguage;
        Experience = model.Experience;
        Notes = model.Notes;
        Validation = FSharpOptionHelpers.ValueOrNull(model.Validation) ?? "";
        SubmittedSummary = FSharpOptionHelpers.ValueOrNull(model.SubmittedSummary) ?? "";
        HasSummary = FSharpOptionHelpers.ValueOrNull(model.SubmittedSummary) is not null;
        _isUpdating = false;
    }

    partial void OnNameChanged(string value)
    {
        if (!_isUpdating) _dispatch(FormPage.Msg.NewSetName(value));
    }

    partial void OnEmailChanged(string value)
    {
        if (!_isUpdating) _dispatch(FormPage.Msg.NewSetEmail(value));
    }

    partial void OnNewsletterChanged(bool value)
    {
        if (!_isUpdating) _dispatch(FormPage.Msg.NewSetNewsletter(value));
    }

    partial void OnFavoriteLanguageChanged(string value)
    {
        if (!_isUpdating && !string.IsNullOrWhiteSpace(value))
        {
            _dispatch(FormPage.Msg.NewSetFavoriteLanguage(value));
        }
    }

    partial void OnExperienceChanged(int value)
    {
        if (!_isUpdating) _dispatch(FormPage.Msg.NewSetExperience(value));
    }

    partial void OnNotesChanged(string value)
    {
        if (!_isUpdating) _dispatch(FormPage.Msg.NewSetNotes(value));
    }

    [RelayCommand]
    private void Submit() => _dispatch(FormPage.Msg.Submit);
}

public partial class DicePageProjection : ObservableObject
{
    private Action<DicePage.Msg> _dispatch = _ => { };

    public ObservableCollection<DiceRollProjection> History { get; } = new();

    [ObservableProperty] public partial string Status { get; set; } = "";
    [ObservableProperty] public partial string LastRoll { get; set; } = "No rolls yet";
    [ObservableProperty] public partial string TotalRolls { get; set; } = "0";
    [ObservableProperty] public partial bool IsRolling { get; set; }

    public void SetDispatch(Action<DicePage.Msg> dispatch) => _dispatch = dispatch;

    public void Update(DicePage.Model model)
    {
        Status = model.Status;
        LastRoll = FSharpOptionHelpers.ValueOrNull(model.LastRoll)?.Caption ?? "No rolls yet";
        TotalRolls = model.TotalRolls.ToString();
        IsRolling = model.IsRolling;

        History.SyncWith(
            model.History,
            roll => roll.Id,
            vm => vm.Id,
            roll => new DiceRollProjection(roll),
            (vm, roll) => vm.Update(roll));
    }

    [RelayCommand]
    private void Roll() => _dispatch(DicePage.Msg.RollRequested);
}

public partial class DiceRollProjection : ObservableObject
{
    public DiceRollProjection(DicePage.Roll model)
    {
        Id = model.Id;
        Update(model);
    }

    public Guid Id { get; }

    [ObservableProperty] public partial string Caption { get; set; } = "";
    [ObservableProperty] public partial string Faces { get; set; } = "";

    public void Update(DicePage.Roll model)
    {
        Caption = model.Caption;
        Faces = $"[{model.Left}] [{model.Right}]";
    }
}

public partial class HttpPageProjection : ObservableObject
{
    private Action<HttpPage.Msg> _dispatch = _ => { };

    [ObservableProperty] public partial string Status { get; set; } = "";
    [ObservableProperty] public partial string PayloadTitle { get; set; } = "";
    [ObservableProperty] public partial string PayloadState { get; set; } = "";
    [ObservableProperty] public partial string LastUpdated { get; set; } = "";
    [ObservableProperty] public partial string Error { get; set; } = "";
    [ObservableProperty] public partial bool HasError { get; set; }
    [ObservableProperty] public partial bool IsLoading { get; set; }

    public void SetDispatch(Action<HttpPage.Msg> dispatch) => _dispatch = dispatch;

    public void Update(HttpPage.Model model)
    {
        Status = model.Status;
        PayloadTitle = model.PayloadTitle;
        PayloadState = model.PayloadState;
        LastUpdated = model.LastUpdated;
        Error = FSharpOptionHelpers.ValueOrNull(model.Error) ?? "";
        HasError = FSharpOptionHelpers.ValueOrNull(model.Error) is not null;
        IsLoading = model.IsLoading;
    }

    [RelayCommand]
    private void Refresh() => _dispatch(HttpPage.Msg.RefreshRequested);
}

public partial class ClockPageProjection : ObservableObject
{
    private Action<ClockPage.Msg> _dispatch = _ => { };

    [ObservableProperty] public partial string TimeText { get; set; } = "";
    [ObservableProperty] public partial string DateText { get; set; } = "";
    [ObservableProperty] public partial string ZoneText { get; set; } = "";
    [ObservableProperty] public partial string TickCount { get; set; } = "";
    [ObservableProperty] public partial string FormatLabel { get; set; } = "";

    public void SetDispatch(Action<ClockPage.Msg> dispatch) => _dispatch = dispatch;

    public void Update(ClockPage.Model model)
    {
        TimeText = model.TimeText;
        DateText = model.DateText;
        ZoneText = model.ZoneText;
        TickCount = model.TickCount.ToString();
        FormatLabel = model.Use24Hour ? "Switch to 12-hour" : "Switch to 24-hour";
    }

    [RelayCommand]
    private void ToggleFormat() => _dispatch(ClockPage.Msg.ToggleFormat);
}

public partial class FilesPageProjection : ObservableObject
{
    private Action<FilesPage.Msg> _dispatch = _ => { };
    private Func<Task<FilesPage.SelectedFile?>>? _picker;

    [ObservableProperty] public partial string Status { get; set; } = "";
    [ObservableProperty] public partial string FileName { get; set; } = "";
    [ObservableProperty] public partial string Kind { get; set; } = "";
    [ObservableProperty] public partial string SizeText { get; set; } = "";
    [ObservableProperty] public partial string Preview { get; set; } = "";
    [ObservableProperty] public partial string Location { get; set; } = "";
    [ObservableProperty] public partial bool HasSelection { get; set; }

    public void SetDispatch(Action<FilesPage.Msg> dispatch) => _dispatch = dispatch;

    public void AttachFilePicker(Func<Task<FilesPage.SelectedFile?>> picker) => _picker = picker;

    public void Update(FilesPage.Model model)
    {
        Status = model.Status;
        var selected = FSharpOptionHelpers.ValueOrNull(model.Selected);
        if (selected is null)
        {
            FileName = "";
            Kind = "";
            SizeText = "";
            Preview = "";
            Location = "";
            HasSelection = false;
        }
        else
        {
            FileName = selected.Name;
            Kind = selected.Kind;
            SizeText = selected.SizeText;
            Preview = selected.Preview;
            Location = selected.Location;
            HasSelection = true;
        }
    }

    [RelayCommand]
    private async Task OpenFileAsync()
    {
        if (_picker is null)
        {
            return;
        }

        var file = await _picker();
        if (file is not null)
        {
            _dispatch(FilesPage.Msg.NewFileOpened(file));
        }
    }

    [RelayCommand]
    private void ClearSelection() => _dispatch(FilesPage.Msg.ClearSelection);
}

public partial class SvgPageProjection : ObservableObject
{
    private Action<SvgPage.Msg> _dispatch = _ => { };

    [ObservableProperty] public partial string ThemeName { get; set; } = "";
    [ObservableProperty] public partial string Caption { get; set; } = "";
    [ObservableProperty] public partial string Background { get; set; } = "";
    [ObservableProperty] public partial string GridLine { get; set; } = "";
    [ObservableProperty] public partial string Sky { get; set; } = "";
    [ObservableProperty] public partial string Sun { get; set; } = "";
    [ObservableProperty] public partial string Hill { get; set; } = "";
    [ObservableProperty] public partial string Accent { get; set; } = "";
    [ObservableProperty] public partial string Rocket { get; set; } = "";
    [ObservableProperty] public partial string Trail { get; set; } = "";

    public void SetDispatch(Action<SvgPage.Msg> dispatch) => _dispatch = dispatch;

    public void Update(SvgPage.Model model)
    {
        ThemeName = model.Theme.Name;
        Caption = model.Theme.Caption;
        Background = model.Theme.Background;
        GridLine = model.Theme.GridLine;
        Sky = model.Theme.Sky;
        Sun = model.Theme.Sun;
        Hill = model.Theme.Hill;
        Accent = model.Theme.Accent;
        Rocket = model.Theme.Rocket;
        Trail = model.Theme.Trail;
    }

    [RelayCommand]
    private void NextTheme() => _dispatch(SvgPage.Msg.NextTheme);
}
