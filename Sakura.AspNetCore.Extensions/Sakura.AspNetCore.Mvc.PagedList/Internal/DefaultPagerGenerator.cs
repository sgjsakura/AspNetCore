using System.Linq;
using Microsoft.AspNetCore.Html;

namespace Sakura.AspNetCore.Mvc.Internal;

/// <summary>
///     Default implementation for <see cref="IPagerGenerator" />.
/// </summary>
public class DefaultPagerGenerator : IPagerGenerator
{
	/// <summary>
	///     Initialize a new instance with all required services.
	/// </summary>
	/// <param name="listGenerator">
	///     A generator used to generate collection of <see cref="PagerItem" /> from the generation
	///     context.
	/// </param>
	/// <param name="renderingListGenerator">
	///     A generator used to generate <see cref="PagerRenderingList" /> from a collection
	///     of <see cref="PagerItem" />.
	/// </param>
	/// <param name="htmlGenerator">
	///     A generator used to generate final <see cref="IHtmlContent" /> from a
	///     <see cref="PagerRenderingList" />.
	/// </param>
	public DefaultPagerGenerator(IPagerListGenerator listGenerator, IPagerRenderingListGenerator renderingListGenerator,
		IPagerHtmlGenerator htmlGenerator)
	{
		ListGenerator = listGenerator;
		RenderingListGenerator = renderingListGenerator;
		HtmlGenerator = htmlGenerator;
	}

	private IPagerHtmlGenerator HtmlGenerator { get; }

	private IPagerRenderingListGenerator RenderingListGenerator { get; }

	private IPagerListGenerator ListGenerator { get; }

	/// <summary>
	///     Generate the entire HTML content for a pager renderling list.
	/// </summary>
	/// <returns>The entire HTML content for the generated pager.</returns>
	public IHtmlContent GeneratePager(PagerGenerationContext context)
	{
		// Hide Handling
		if (context.TotalPage <= 1 && context.Options.HideOnSinglePage)
			return new HtmlString(string.Empty);

		var list = ListGenerator.GeneratePagerItems(context);

		// Reverse handling
		if (context.Options.IsReversed)
			list = new PagerList(list.Items.Reverse());

		var renderingList = RenderingListGenerator.GenerateRenderingList(list, context);
		var html = HtmlGenerator.GeneratePager(renderingList, context);

		return html;
	}
}