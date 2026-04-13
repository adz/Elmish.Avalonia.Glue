using OpsCenterSample.Core;

namespace OpsCenterSample.UI.Components;

internal static class ProjectionFactory
{
    public static MetricCardProjection MetricCard(Ui.MetricCard card)
    {
        var projection = new MetricCardProjection();
        projection.Update(card);
        return projection;
    }
}
