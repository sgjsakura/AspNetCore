using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sakura.AspNetCore.Mvc;

/// <summary>
/// Provide options for visual style settings.
/// </summary>
public class VisualStyleOptions
{
    /// <summary>
    /// The preferred visual styles. Item orders indicate the preference priority.
    /// </summary>
    public IList<VisualStyle> PreferredStyles { get; } = new Collection<VisualStyle>();


    /// <summary>
    /// The minimal match level for the style selection.
    /// </summary>
    public VisualStyleMatchType MinimalMatchLevel { get; set; } = VisualStyleMatchType.None;
}