using System.ComponentModel;
using Microsoft.AspNetCore.Html;
using Sakura.AspNetCore.Mvc.Internal;

namespace Sakura.AspNetCore.Mvc;

/// <summary>
///     Define the necessary feature for a pager item content generator.
/// </summary>
[TypeConverter(typeof(PagerItemContentGeneratorConverter))]
public interface IPagerItemContentGenerator
{
	/// <summary>
	///     Generate the content for a specified pager item.
	/// </summary>
	/// <param name="context">The generation context.</param>
	/// <returns>The generated HTML content for the pager item.</returns>
	IHtmlContent GenerateContent(PagerItemGenerationContext context);
}