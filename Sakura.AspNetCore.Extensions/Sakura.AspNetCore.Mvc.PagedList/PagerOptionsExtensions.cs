using System;
using JetBrains.Annotations;

namespace Sakura.AspNetCore.Mvc;

/// <summary>
///     Provide extension methods for pager options. This class is static.
/// </summary>
public static class PagerOptionsExtensions
{
	/// <summary>
	///     Configure a <see cref="PagerOptions" /> instance to set all items as default setting.
	/// </summary>
	/// <param name="options">The <see cref="PagerOptions" /> to be set.</param>
	/// <exception cref="ArgumentNullException">The <paramref name="options" /> is <c>null</c>.</exception>
	[PublicAPI]
	public static void ConfigureDefault([NotNull] this PagerOptions options)
	{
		// Argument check
		if (options == null)
			throw new ArgumentNullException(nameof(options));

		options.Layout = PagerLayouts.Default;
		options.ExpandPageItemsForCurrentPage = 2;
		options.PagerItemsForEndings = 3;

		options.ItemOptions.Default.Content = PagerItemContentGenerators.Default;
		options.ItemOptions.Default.Link = PagerItemLinkGenerators.Default;
		options.ItemOptions.Default.InactiveBehavior = SpecialPagerItemInactiveBehavior.Disable;
		options.ItemOptions.Default.ActiveMode = FirstAndLastPagerItemActiveMode.NotInCurrentPage;

		options.ItemOptions.PreviousPageButton.Content = PagerItemContentGenerators.Html("&lsaquo;");
		options.ItemOptions.NextPageButton.Content = PagerItemContentGenerators.Html("&rsaquo;");
		options.ItemOptions.FirstPageButton.Content = PagerItemContentGenerators.Html("&laquo;");
		options.ItemOptions.LastPageButton.Content = PagerItemContentGenerators.Html("&raquo;");

		options.ItemOptions.Omitted.Content = PagerItemContentGenerators.Text("...");
		options.ItemOptions.Omitted.Link = PagerItemLinkGenerators.Disabled;
	}
}