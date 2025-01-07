namespace Sakura.AspNetCore.Mvc;

public class VisualStyleMatchResult(
    VisualStyle requiredStyle,
    VisualStyle selectedStyle,
    VisualStyleMatchType matchType)
{
    public VisualStyle RequiredStyle { get; } = requiredStyle;

    public VisualStyle SelectedStyle { get; } = selectedStyle;

    public VisualStyleMatchType MatchType { get; } = matchType;
}