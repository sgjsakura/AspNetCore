using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;

namespace Sakura.AspNetCore.Mvc;

/// <summary>
/// Provide service for visual style detection and selection.
/// </summary>
public class VisualStyleService(IOptions<VisualStyleOptions> options)
{
    private VisualStyleOptions Options { get; } = options.Value;

    public VisualStyleMatchResult Match(IEnumerable<VisualStyle> allowedStyles)
    {
        var result =
            from i in Options.PreferredStyles
            from j in allowedStyles
            let matchType = VisualStyle.TryMatch(i, j)
            where matchType >= Options.MinimalMatchLevel
            orderby matchType descending
            select new VisualStyleMatchResult(i, j, matchType);

        return result.FirstOrDefault();
    }
}