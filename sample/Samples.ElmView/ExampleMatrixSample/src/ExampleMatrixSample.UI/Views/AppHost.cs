using System;
using Elmish.Avalonia.Glue.ElmView;
using ExampleMatrixSample.ElmView.Core;
using CoreAppView = ExampleMatrixSample.ElmView.Core.AppView;

namespace ExampleMatrixSample.ElmView.UI.Views;

public sealed class AppHost : RuntimeGeneratedViewHost<CoreAppView, Msg>
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
        };

    private readonly UserInputNode _userInput;

    public AppHost() : base(Core.App.getDesignView(), ConfigureBindings)
    {
        _userInput = new UserInputNode(this);
    }

    public override System.Collections.Generic.IEnumerable<string> GeneratedPropertyNames =>
        ["UserInput"];

    public UserInputNode UserInput => _userInput;

    public void Navigate(Example example) => Dispatch(Msg.NewNavigate(example));
    public void SetName(string value) => TryDispatchWriteBack("UserInput.Name", value);
    public void SetEmail(string value) => TryDispatchWriteBack("UserInput.Email", value);
    public void SetNewsletter(bool value) => TryDispatchWriteBack("UserInput.Newsletter", value);
    public void SetFavoriteLanguage(string value) => TryDispatchWriteBack("UserInput.FavoriteLanguage", value);
    public void SetExperience(int value) => TryDispatchWriteBack("UserInput.Experience", value);
    public void SetNotes(string value) => TryDispatchWriteBack("UserInput.Notes", value);
    public void SubmitForm() => Dispatch(Msg.SubmitForm);
    public void RollDice() => Dispatch(Msg.RollDice);
    public void RefreshHttp() => Dispatch(Msg.RefreshHttp);
    public void ToggleClockFormat() => Dispatch(Msg.ToggleClockFormat);
    public void FileOpened(SelectedFileView file) => Dispatch(Msg.NewFileOpened(file));
    public void ClearFileSelection() => Dispatch(Msg.ClearFileSelection);
    public void NextSvgTheme() => Dispatch(Msg.NextSvgTheme);

    public sealed class UserInputNode : GeneratedViewNode<CoreAppView, FormView, Msg>
    {
        public UserInputNode(AppHost host)
            : base(
                new Func<CoreAppView>(() => host.View),
                new Action<Msg>(host.Dispatch),
                new Action<IGeneratedViewNode>(host.RegisterChildNode),
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
}
