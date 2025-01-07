using System;
using System.Collections.Generic;

namespace Sakura.AspNetCore.Mvc;

/// <summary>
///     Contain all options which affect the behavior for a paged list pager.
/// </summary>
public class PagerOptions
{
	/// <summary>
	///     Get or set the additional settings dictionary for the pager. The key of the setting is case-insensitive.
	/// </summary>
	public Dictionary<string, string> AdditionalSettings { get; private set; } =
		new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

	/// <summary>
	///     Get a set for all <see cref="PagerItemOptions" /> for different pager items.
	/// </summary>
	public PagerItemOptionsSet ItemOptions { get; private set; } = new PagerItemOptionsSet();

	/// <summary>
	///     Get or set the layout for different pager elements.
	/// </summary>
	/// <remarks>
	///     If this property is <c>null</c>, <see cref="PagerLayouts.Default" /> will be used.
	/// </remarks>
	/// <seealso cref="PagerLayouts" />
	public PagerLayout Layout { get; set; }

	/// <summary>
	///     Get or set how many links for previous and next pages will appear near the current page.
	/// </summary>
	/// <remarks>
	///     The value 0 means no expand page links will be shown (only shows the current page). The value 1 means showing the
	///     first one next and previous pages near the current page., etc. You may use any negative value to force show all
	///     pages (note: show all pages may badly reduce your page performance).
	/// </remarks>
	public int ExpandPageItemsForCurrentPage { get; set; }

	/// <summary>
	///     Get or set how many links for the beginning and ending pages will appear in the pager.
	/// </summary>
	/// <remarks>
	///     The value 0 means no page links will be shown in the beginning and ending position or the pager. The value 1 means
	///     showing the first and last one page links, etc.
	/// </remarks>
	public int PagerItemsForEndings { get; set; }

	/// <summary>
	///     Get or set a value that indicates whether the pager should reverse the layout and all items (including the
	///     numbers).
	/// </summary>
	public bool IsReversed { get; set; }

	/// <summary>
	///     Get or set a value that indicates whether the pager should generate nothing if there is a single page in the
	///     source.
	/// </summary>
	public bool HideOnSinglePage { get; set; }

	/// <summary>
	///     Get a clone of this object.
	/// </summary>
	/// <returns>A clone of this object.</returns>
	public virtual PagerOptions Clone()
	{
		var result = (PagerOptions) MemberwiseClone();

		result.ItemOptions = ItemOptions.Clone();
		result.Layout = new PagerLayout(Layout.Elements);
		result.AdditionalSettings = new Dictionary<string, string>(AdditionalSettings);

		return result;
	}
}