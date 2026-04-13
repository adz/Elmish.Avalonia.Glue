using CommunityToolkit.Mvvm.ComponentModel;
using Ui = OpsCenterSample.Core.Ui;

namespace OpsCenterSample.UI.Components;

public partial class MetricCardProjection : ObservableObject
{
    [ObservableProperty] public partial string Label { get; set; } = "";
    [ObservableProperty] public partial string Value { get; set; } = "";
    [ObservableProperty] public partial string Hint { get; set; } = "";

    public void Update(string nextLabel, string nextValue, string nextHint)
    {
        Label = nextLabel;
        Value = nextValue;
        Hint = nextHint;
    }

    public void Update(Ui.MetricCard card)
    {
        Label = card.Label;
        Value = card.Value;
        Hint = card.Hint;
    }
}
