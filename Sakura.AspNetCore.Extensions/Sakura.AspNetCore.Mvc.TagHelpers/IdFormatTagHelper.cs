using System;
using System.Collections.Concurrent;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sakura.AspNetCore.Mvc.TagHelpers;

/// <summary>
///     A tag helper used to generate
/// </summary>
[HtmlTargetElement("*", Attributes = IdFormatAttributeName)]
public class IdFormatTagHelper : TagHelper
{
	/// <summary>
	///     Get the attribute name used for <see cref="IdFormat" /> property. This field is constant.
	/// </summary>
	public const string IdFormatAttributeName = "asp-id-format";

	/// <summary>
	///     Get or set the format string for the id.
	/// </summary>
	[HtmlAttributeName(IdFormatAttributeName)]
	public string IdFormat { get; set; }

	/// <summary>
	///     Get or set the start value for each format string.
	/// </summary>
	public static int IdStart { get; set; } = 1;

	/// <summary>
	///     Get or set the key used to store the id-format counting information.
	/// </summary>
	public static string ViewDataKey { get; set; } = "ASP_IdFormatCountDictionary";

	/// <summary>
	///     Get or set the <see cref="ViewContext" /> related to this <see cref="TagHelper" />.
	/// </summary>
	[ViewContext]
	[HtmlAttributeNotBound] public ViewContext ViewContext { get; set; }

	/// <summary>
	///     Get the dictionary stored the id format counting inforamtion.
	/// </summary>
	protected ConcurrentDictionary<string, int> IdFormatCountDictionary
	{
		get
		{
			if (ViewDataKey == null)
				throw new InvalidOperationException($"The property of '{nameof(ViewDataKey)}' cannot be null.");

			if (ViewContext.ViewData.TryGetValue(ViewDataKey, out var result) &&
			    result is ConcurrentDictionary<string, int> dic)
				return dic;

			var newDic = new ConcurrentDictionary<string, int>(StringComparer.Ordinal);
			ViewContext.ViewData[ViewDataKey] = newDic;
			return newDic;
		}
	}

	/// <summary>
	///     Get next count for current id format.
	/// </summary>
	/// <returns>The next count for current id format.</returns>
	protected int GetNextCount()
	{
		if (string.IsNullOrEmpty(IdFormat))
			throw new InvalidOperationException(
				$"The value of html attribute '{IdFormatAttributeName}' cannot be null.");

		return IdFormatCountDictionary.AddOrUpdate(IdFormat, IdStart, (key, value) => value + 1);
	}

	/// <summary>
	///     Synchronously executes the <see cref="TagHelper" /> with the given <paramref name="context" /> and
	///     <paramref name="output" />.
	/// </summary>
	/// <param name="context">Contains information associated with the current HTML tag.</param>
	/// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
	public override void Process(TagHelperContext context, TagHelperOutput output)
	{
		if (string.IsNullOrEmpty(IdFormat))
			throw new InvalidOperationException(
				$"The value of html attribute '{IdFormatAttributeName}' cannot be null.");

		if (context.AllAttributes.ContainsName("id"))
			throw new InvalidOperationException(
				$"You cannot specify both 'id' and '{IdFormatAttributeName}' attribute on one element.");

		output.Attributes.SetAttribute("id", string.Format(CultureInfo.InvariantCulture, IdFormat, GetNextCount()));
	}
}