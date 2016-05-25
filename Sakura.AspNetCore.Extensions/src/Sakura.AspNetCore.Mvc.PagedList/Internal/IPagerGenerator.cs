using Microsoft.AspNetCore.Html;

namespace Sakura.AspNetCore.Mvc.Internal
{
	/// <summary>
	///     Define the necessary feature needed to generate a pager.
	/// </summary>
	public interface IPagerGenerator
	{
		/// <summary>
		///     Generate the entire HTML content for a pager tag.
		/// </summary>
		/// <param name="context">The context information for pager generation.</param>
		/// <returns>The entire HTML content for the generated pager.</returns>
		IHtmlContent GeneratePager(PagerGenerationContext context);
	}
}