using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

// ReSharper disable MustUseReturnValue

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	/// <summary>
	///     Helper class for HTML generation. The code in this class are copy from ASP.NET official implementation for
	///     <see cref="DefaultHtmlGenerator" />.
	/// </summary>
	internal static class HtmlGeneratorHelper
	{
		/// <remarks>
		///     Not used directly in HtmlHelper. Exposed for use in DefaultDisplayTemplates.
		/// </remarks>
		public static TagBuilder GenerateOption(SelectListItem item, string text)
		{
			var tagBuilder = new TagBuilder("option");
			tagBuilder.InnerHtml.SetContent(text);

			if (item.Value != null)
			{
				tagBuilder.Attributes["value"] = item.Value;
			}

			if (item.Selected)
			{
				tagBuilder.Attributes["selected"] = "selected";
			}

			if (item.Disabled)
			{
				tagBuilder.Attributes["disabled"] = "disabled";
			}

			return tagBuilder;
		}

		public static IHtmlContent GenerateGroupsAndOptions(string optionLabel, IEnumerable<SelectListItem> selectList)
		{
			var listItemBuilder = new DefaultTagHelperContent();

			// Make optionLabel the first item that gets rendered.
			if (optionLabel != null)
			{
				listItemBuilder.AppendLine(GenerateOption(new SelectListItem
				{
					Text = optionLabel,
					Value = string.Empty,
					Selected = false
				}));
			}

			// Group items in the SelectList if requested.
			// Treat each item with Group == null as a member of a unique group
			// so they are added according to the original order.
			var groupedSelectList = selectList.GroupBy(item => item.Group?.GetHashCode() ?? item.GetHashCode());

			foreach (var group in groupedSelectList)
			{
				var optGroup = group.First().Group;
				if (optGroup != null)
				{
					var groupBuilder = new TagBuilder("optgroup");
					if (optGroup.Name != null)
					{
						groupBuilder.MergeAttribute("label", optGroup.Name);
					}

					if (optGroup.Disabled)
					{
						groupBuilder.MergeAttribute("disabled", "disabled");
					}

					groupBuilder.InnerHtml.AppendLine();
					foreach (var item in group)
					{
						groupBuilder.InnerHtml.AppendLine(GenerateOption(item));
					}

					listItemBuilder.AppendLine(groupBuilder);
				}
				else
				{
					foreach (var item in group)
					{
						listItemBuilder.AppendLine(GenerateOption(item));
					}
				}
			}

			return listItemBuilder;
		}

		public static IHtmlContent GenerateOption(SelectListItem item)
		{
			var tagBuilder = GenerateOption(item, item.Text);
			return tagBuilder;
		}
	}
}