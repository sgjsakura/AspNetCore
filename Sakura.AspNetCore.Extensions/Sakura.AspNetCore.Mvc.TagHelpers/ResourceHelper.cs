using Microsoft.Extensions.Localization;

namespace Sakura.AspNetCore.Mvc.TagHelpers;

/// <summary>
///     Provide helper methods for resource operations. This class is static.
/// </summary>
public static class ResourceHelper
{
	/// <summary>
	///     Try to get the localized text from a given <see cref="IStringLocalizer" />.
	/// </summary>
	/// <param name="localizer">The <see cref="IStringLocalizer" /> service used to pick up the localized text.</param>
	/// <param name="text">The original text object.</param>
	/// <returns>
	///     If the <paramref name="localizer" /> is not <c>null</c>, this method will pick the localized text from it;
	///     otherwise, this method will return the <paramref name="text" /> unmodified.
	/// </returns>
	public static string TryGetLocalizedText(this IStringLocalizer localizer, string text)
	{
		return localizer != null ? localizer[text] : text;
	}
}