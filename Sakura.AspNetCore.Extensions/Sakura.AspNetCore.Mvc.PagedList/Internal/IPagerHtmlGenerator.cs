using Microsoft.AspNetCore.Html;

namespace Sakura.AspNetCore.Mvc.Internal;

/// <summary>
///     Define the feature needed to generate a pager's HTML for an <see cref="IPagedList" /> object.
/// </summary>
public interface IPagerHtmlGenerator
{
	/// <summary>
	///     Generate the entire HTML content for a pager renderling list.
	/// </summary>
	/// <param name="list">The rendering list to be generating.</param>
	/// <param name="context">The pager generation context.</param>
	/// <returns>The entire HTML content for the generated pager.</returns>
	IHtmlContent GeneratePager(PagerRenderingList list, PagerGenerationContext context);
}