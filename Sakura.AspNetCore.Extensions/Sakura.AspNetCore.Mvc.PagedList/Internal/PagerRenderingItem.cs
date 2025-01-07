using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;

namespace Sakura.AspNetCore.Mvc.Internal;

/// <summary>
///     Represent as a single item in a paged list pager.
/// </summary>
public class PagerRenderingItem
{
	/// <summary>
	///     Initialize a new <see cref="PagerRenderingItem" />.
	/// </summary>
	/// <param name="list">The owner list of the pager item.</param>
	public PagerRenderingItem([NotNull] PagerRenderingList list)
	{
		List = list ?? throw new ArgumentNullException(nameof(list));
	}

	/// <summary>
	///     Get the owner list for the current pager item.
	/// </summary>
	[NotNull]
	public PagerRenderingList List { get; }

	/// <summary>
	///     Get or set the content of the pager item.
	/// </summary>
	public IHtmlContent Content { get; set; }

	/// <summary>
	///     Get or set the linked url address for this page (if any).
	/// </summary>
	public string Link { get; set; }

	/// <summary>
	///     Get or set the state of this item.
	/// </summary>
	public PagerRenderingItemState State { get; set; }

	/// <summary>
	///     Get or set the dictionary used to store all additional settings.
	/// </summary>
	public IDictionary<string, string> Settings { get; set; } = new Dictionary<string, string>();
}