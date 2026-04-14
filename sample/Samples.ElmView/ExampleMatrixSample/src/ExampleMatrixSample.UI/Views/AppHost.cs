using System;
using System.Collections.Generic;
using Elmish.Avalonia.Glue.ElmView;
using Elmish.Glue.Core;
using ExampleMatrixSample.ElmView.Core;
using CoreAppView = ExampleMatrixSample.ElmView.Core.AppView;

namespace ExampleMatrixSample.ElmView.UI.Views;

public class AppHost : RuntimeGeneratedViewHost<CoreAppView, Msg>
{
    private static readonly Action<WriteBackBindings<CoreAppView, Msg>> ConfigureBindings =
        bindings =>
        {
            bindings.For(x => x.UserInput.Name).Dispatch(Msg.NewSetName);
            bindings.For(x => x.UserInput.Email).Dispatch(Msg.NewSetEmail);
            bindings.For(x => x.UserInput.Newsletter).Dispatch(Msg.NewSetNewsletter);
            bindings.For(x => x.UserInput.FavoriteLanguage).Dispatch(Msg.NewSetFavoriteLanguage);
            bindings.For(x => x.UserInput.Experience).Dispatch(Msg.NewSetExperience);
            bindings.For(x => x.UserInput.Notes).Dispatch(Msg.NewSetNotes);
            bindings.For(x => x.Clock.Use24Hour).Dispatch(Msg.NewSetClockUse24Hour);
            bindings.For(x => x.Svg.ThemeIndex).Dispatch(Msg.NewSetSvgThemeIndex);
        };

    private readonly UserInputNode _userInput;
    private readonly RandomDiceNode _randomDice;
    private readonly HttpNode _http;
    private readonly ClockNode _clock;
    private readonly FilesNode _files;
    private readonly SvgNode _svg;

    public AppHost() : this(Core.App.getDesignView())
    {
    }

    protected AppHost(CoreAppView initialView) : base(initialView, ConfigureBindings)
    {
        _userInput = new UserInputNode(this);
        _randomDice = new RandomDiceNode(this);
        _http = new HttpNode(this);
        _clock = new ClockNode(this);
        _files = new FilesNode(this);
        _svg = new SvgNode(this);
    }

    public override IEnumerable<string> GeneratedPropertyNames =>
        ["UserInput", "RandomDice", "Http", "Clock", "Files", "Svg"];

    public UserInputNode UserInput => _userInput;
    public RandomDiceNode RandomDice => _randomDice;
    public HttpNode Http => _http;
    public ClockNode Clock => _clock;
    public FilesNode Files => _files;
    public SvgNode Svg => _svg;

    public void Navigate(Example example) => Dispatch(Msg.NewNavigate(example));
    public void SubmitForm() => Dispatch(Msg.SubmitForm);
    public void RollDice() => Dispatch(Msg.RollDice);
    public void RefreshHttp() => Dispatch(Msg.RefreshHttp);
    public void FileOpened(SelectedFileView file) => Dispatch(Msg.NewFileOpened(file));
    public void ClearFileSelection() => Dispatch(Msg.ClearFileSelection);

    public sealed class UserInputNode : GeneratedViewNode<CoreAppView, FormView, Msg>
    {
        public UserInputNode(AppHost host)
            : base(
                new Func<CoreAppView>(() => host.View),
                new Action<Msg>(host.Dispatch),
                new Action<IBindableSnapshotNode>(host.RegisterChildNode),
                new Func<CoreAppView, FormView>(view => view.UserInput),
                new[] { nameof(Name), nameof(Email), nameof(Newsletter), nameof(FavoriteLanguage), nameof(Languages), nameof(Experience), nameof(Notes), nameof(ValidationText), nameof(HasValidation), nameof(SummaryText), nameof(HasSummary) })
        {
            Host = host;
        }

        private AppHost Host { get; }

        public string Name
        {
            get => Snapshot.Name;
            set => Host.TryDispatchWriteBack("UserInput.Name", value);
        }

        public string Email
        {
            get => Snapshot.Email;
            set => Host.TryDispatchWriteBack("UserInput.Email", value);
        }

        public bool Newsletter
        {
            get => Snapshot.Newsletter;
            set => Host.TryDispatchWriteBack("UserInput.Newsletter", value);
        }

        public string FavoriteLanguage
        {
            get => Snapshot.FavoriteLanguage;
            set => Host.TryDispatchWriteBack("UserInput.FavoriteLanguage", value);
        }

        public System.Collections.Generic.IReadOnlyList<string> Languages => Snapshot.Languages;

        public int Experience
        {
            get => Snapshot.Experience;
            set => Host.TryDispatchWriteBack("UserInput.Experience", value);
        }

        public string Notes
        {
            get => Snapshot.Notes;
            set => Host.TryDispatchWriteBack("UserInput.Notes", value);
        }

        public string ValidationText => Snapshot.ValidationText;

        public bool HasValidation => Snapshot.HasValidation;

        public string SummaryText => Snapshot.SummaryText;

        public bool HasSummary => Snapshot.HasSummary;
    }

    public sealed class RandomDiceNode : GeneratedViewNode<CoreAppView, DiceView, Msg>
    {
        public RandomDiceNode(AppHost host)
            : base(
                new Func<CoreAppView>(() => host.View),
                new Action<Msg>(host.Dispatch),
                new Action<IBindableSnapshotNode>(host.RegisterChildNode),
                new Func<CoreAppView, DiceView>(view => view.RandomDice),
                new[] { nameof(Status), nameof(IsRolling), nameof(HasLastRoll), nameof(LastRollLeft), nameof(LastRollRight), nameof(LastRollTotal), nameof(LastRollCaption), nameof(TotalRolls), nameof(History) })
        {
        }

        public string Status => Snapshot.Status;
        public bool IsRolling => Snapshot.IsRolling;
        public bool HasLastRoll => Snapshot.HasLastRoll;
        public int LastRollLeft => Snapshot.LastRollLeft;
        public int LastRollRight => Snapshot.LastRollRight;
        public int LastRollTotal => Snapshot.LastRollTotal;
        public string LastRollCaption => Snapshot.LastRollCaption;
        public int TotalRolls => Snapshot.TotalRolls;
        public IReadOnlyList<DiceRollView> History => Snapshot.History;
    }

    public sealed class HttpNode : GeneratedViewNode<CoreAppView, HttpView, Msg>
    {
        public HttpNode(AppHost host)
            : base(
                new Func<CoreAppView>(() => host.View),
                new Action<Msg>(host.Dispatch),
                new Action<IBindableSnapshotNode>(host.RegisterChildNode),
                new Func<CoreAppView, HttpView>(view => view.Http),
                new[] { nameof(Status), nameof(PayloadTitle), nameof(PayloadState), nameof(LastUpdated), nameof(ErrorText), nameof(HasError), nameof(IsLoading) })
        {
        }

        public string Status => Snapshot.Status;
        public string PayloadTitle => Snapshot.PayloadTitle;
        public string PayloadState => Snapshot.PayloadState;
        public string LastUpdated => Snapshot.LastUpdated;
        public string ErrorText => Snapshot.ErrorText;
        public bool HasError => Snapshot.HasError;
        public bool IsLoading => Snapshot.IsLoading;
    }

    public sealed class ClockNode : GeneratedViewNode<CoreAppView, ClockView, Msg>
    {
        public ClockNode(AppHost host)
            : base(
                new Func<CoreAppView>(() => host.View),
                new Action<Msg>(host.Dispatch),
                new Action<IBindableSnapshotNode>(host.RegisterChildNode),
                new Func<CoreAppView, ClockView>(view => view.Clock),
                new[] { nameof(TimeText), nameof(DateText), nameof(ZoneText), nameof(TickCount), nameof(Use24Hour), nameof(FormatLabel) })
        {
            Host = host;
        }

        private AppHost Host { get; }

        public string TimeText => Snapshot.TimeText;
        public string DateText => Snapshot.DateText;
        public string ZoneText => Snapshot.ZoneText;
        public int TickCount => Snapshot.TickCount;

        public bool Use24Hour
        {
            get => Snapshot.Use24Hour;
            set => Host.TryDispatchWriteBack("Clock.Use24Hour", value);
        }

        public string FormatLabel => Snapshot.FormatLabel;
    }

    public sealed class FilesNode : GeneratedViewNode<CoreAppView, FilesView, Msg>
    {
        public FilesNode(AppHost host)
            : base(
                new Func<CoreAppView>(() => host.View),
                new Action<Msg>(host.Dispatch),
                new Action<IBindableSnapshotNode>(host.RegisterChildNode),
                new Func<CoreAppView, FilesView>(view => view.Files),
                new[] { nameof(Status), nameof(HasSelection), nameof(SelectedName), nameof(SelectedKind), nameof(SelectedSizeText), nameof(SelectedPreview), nameof(SelectedLocation) })
        {
        }

        public string Status => Snapshot.Status;
        public bool HasSelection => Snapshot.HasSelection;
        public string SelectedName => Snapshot.SelectedName;
        public string SelectedKind => Snapshot.SelectedKind;
        public string SelectedSizeText => Snapshot.SelectedSizeText;
        public string SelectedPreview => Snapshot.SelectedPreview;
        public string SelectedLocation => Snapshot.SelectedLocation;
    }

    public sealed class SvgNode : GeneratedViewNode<CoreAppView, SvgView, Msg>
    {
        private readonly ThemeNode _theme;

        public SvgNode(AppHost host)
            : base(
                new Func<CoreAppView>(() => host.View),
                new Action<Msg>(host.Dispatch),
                new Action<IBindableSnapshotNode>(host.RegisterChildNode),
                new Func<CoreAppView, SvgView>(view => view.Svg),
                new[] { nameof(ThemeIndex), nameof(ThemeCount), nameof(ThemeNames), nameof(Theme) })
        {
            Host = host;
            _theme = new ThemeNode(host, this);
        }

        private AppHost Host { get; }

        public int ThemeIndex
        {
            get => Snapshot.ThemeIndex;
            set => Host.TryDispatchWriteBack("Svg.ThemeIndex", value);
        }

        public int ThemeCount => Snapshot.ThemeCount;
        public IReadOnlyList<string> ThemeNames => Snapshot.ThemeNames;
        public ThemeNode Theme => _theme;

        public sealed class ThemeNode : GeneratedViewNode<CoreAppView, SvgThemeView, Msg>
        {
            public ThemeNode(AppHost host, SvgNode parent)
                : base(
                    new Func<CoreAppView>(() => host.View),
                    new Action<Msg>(host.Dispatch),
                    new Action<IBindableSnapshotNode>(parent.RegisterChildNode),
                    new Func<CoreAppView, SvgThemeView>(view => view.Svg.Theme),
                    new[] { nameof(Name), nameof(Caption), nameof(Background), nameof(GridLine), nameof(Sky), nameof(Sun), nameof(Hill), nameof(Accent), nameof(Rocket), nameof(Trail) })
            {
            }

            public string Name => Snapshot.Name;
            public string Caption => Snapshot.Caption;
            public string Background => Snapshot.Background;
            public string GridLine => Snapshot.GridLine;
            public string Sky => Snapshot.Sky;
            public string Sun => Snapshot.Sun;
            public string Hill => Snapshot.Hill;
            public string Accent => Snapshot.Accent;
            public string Rocket => Snapshot.Rocket;
            public string Trail => Snapshot.Trail;
        }
    }
}

public sealed class UserInputPreviewHost : AppHost
{
    public UserInputPreviewHost() : base(Core.App.getDesignViewFor(Example.UserInput))
    {
    }
}
