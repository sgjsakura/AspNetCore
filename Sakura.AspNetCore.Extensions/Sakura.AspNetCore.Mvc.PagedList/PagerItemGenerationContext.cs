using System;
using JetBrains.Annotations;
using Sakura.AspNetCore.Mvc.Internal;

namespace Sakura.AspNetCore.Mvc;

/// <summary>
///     Provide necessary information for pager item generation.
/// </summary>
public class PagerItemGenerationContext : PagerGenerationContext
{
	/// <summary>
	///     Create a new <see cref="PagerItemGenerationContext" /> from a base <see cref="PagerGenerationContext" />.
	/// </summary>
	/// <param name="baseContext">The base <see cref="PagerGenerationContext" /> instance.</param>
	/// <param name="pagerItem">The current page item.</param>
	/// <param name="pagerItemOptions">The current page item options.</param>
	public PagerItemGenerationContext([NotNull] PagerGenerationContext baseContext, PagerItem pagerItem,
		PagerItemOptions pagerItemOptions)
	{
		if (baseContext == null)
			throw new ArgumentNullException(nameof(baseContext));

		CurrentPage = baseContext.CurrentPage;
		TotalPage = baseContext.TotalPage;
		Options = baseContext.Options;
		ViewContext = baseContext.ViewContext;
		GenerationMode = baseContext.GenerationMode;

		PagerItem = pagerItem;
		PagerItemOptions = pagerItemOptions;
	}

	/// <summary>
	///     Get the current pager item to be generating.
	/// </summary>
	public PagerItem PagerItem { get; }

	/// <summary>
	///     Get the options for the current pager item.
	/// </summary>
	public PagerItemOptions PagerItemOptions { get; }
}