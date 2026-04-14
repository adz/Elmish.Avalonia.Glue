using System;
using Elmish.Avalonia.Glue.ElmView;
using CoreAppView = ExampleMatrixSample.ElmView.Core.AppView;
using ExampleMatrixSample.ElmView.Core;

namespace ExampleMatrixSample.ElmView.UI.Views;

public sealed class AppHost : RuntimeViewHost<CoreAppView>
{
    private Action<Msg> _dispatch = _ => { };

    public AppHost() : base(Core.App.getDesignView())
    {
    }

    public void SetDispatch(Action<Msg> dispatch) => _dispatch = dispatch;

    public void Navigate(Example example) => _dispatch(Msg.NewNavigate(example));
    public void SetName(string value) => _dispatch(Msg.NewSetName(value));
    public void SetEmail(string value) => _dispatch(Msg.NewSetEmail(value));
    public void SetNewsletter(bool value) => _dispatch(Msg.NewSetNewsletter(value));
    public void SetFavoriteLanguage(string value) => _dispatch(Msg.NewSetFavoriteLanguage(value));
    public void SetExperience(int value) => _dispatch(Msg.NewSetExperience(value));
    public void SetNotes(string value) => _dispatch(Msg.NewSetNotes(value));
    public void SubmitForm() => _dispatch(Msg.SubmitForm);
    public void RollDice() => _dispatch(Msg.RollDice);
    public void RefreshHttp() => _dispatch(Msg.RefreshHttp);
    public void ToggleClockFormat() => _dispatch(Msg.ToggleClockFormat);
    public void FileOpened(SelectedFileView file) => _dispatch(Msg.NewFileOpened(file));
    public void ClearFileSelection() => _dispatch(Msg.ClearFileSelection);
    public void NextSvgTheme() => _dispatch(Msg.NextSvgTheme);
}
