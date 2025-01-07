using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sakura.AspNetCore.Mvc.TagHelpers;

/// <summary>
///     Give the "select" element an optional label item.
/// </summary>
[HtmlTargetElement("select", Attributes = OptionLabelAttributeName)]
public class SelectOptionLabelTagHelper : TagHelper
{
	/// <summary>
	///     Synchronously executes the <see cref="T:Microsoft.AspNet.Razor.Runtime.TagHelpers.TagHelper" /> with the given
	///     <paramref name="context" /> and
	///     <paramref name="output" />.
	/// </summary>
	/// <param name="context">Contains information associated with the current HTML tag.</param>
	/// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
	public override void Process(TagHelperContext context, TagHelperOutput output)
	{
		var optionTag = new TagBuilder("option");

		// 默认值
		optionTag.Attributes["value"] = "";

		// 文字
		optionTag.InnerHtml.Append(OptionLabel);

		switch (OptionLabelPosition)
		{
			case OptionLabelPosition.First:
				output.PreContent.AppendHtml(optionTag);
				break;
			case OptionLabelPosition.Last:
				output.PostContent.AppendHtml(optionTag);
				break;
			default:
				throw new InvalidOperationException(
					"The value of OptionLabelPosition property is not a valid enum item.");
		}
	}

	#region Constant field

	/// <summary>
	///     Get the html attribute name associated with <see cref="OptionLabel" /> property. This field is constant.
	/// </summary>
	public const string OptionLabelAttributeName = "asp-option-label";

	/// <summary>
	///     Get the html attribute name associated with <see cref="OptionLabelPosition" /> property. This field is constant.
	/// </summary>
	public const string OptionLabelPositionAttributeName = "asp-option-label-position";

	#endregion

	#region Binding Properties

	/// <summary>
	///     Get or set the content string for option label.
	/// </summary>
	[HtmlAttributeName(OptionLabelAttributeName)]
	public string OptionLabel { get; set; }


	/// <summary>
	///     Get or set the existing location for the option label. The default value of this property is
	///     <see cref="OptionLabelPosition.First" />.
	/// </summary>
	[HtmlAttributeName(OptionLabelPositionAttributeName)]
	public OptionLabelPosition OptionLabelPosition { get; set; } = OptionLabelPosition.First;

	#endregion
}