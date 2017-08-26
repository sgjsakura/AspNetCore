using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	/// <summary>
	///     Provide the <see cref="Source" /> property for <see cref="PagerTagHelper" />.
	/// </summary>
	[HtmlTargetElement(PagerTagHelper.HtmlTagName, TagStructure = TagStructure.WithoutEndTag)]
	public class PagerSourceTagHelper : TagHelper
	{
		#region Core methods

		/// <inheritdoc />
		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			// Check for if source attribute is defined
			var hasSourceAttr = context.AllAttributes.ContainsName(SourceAttributeName);

			// Check for conflication
			if (context.AllAttributes.ContainsName(PagerTagHelper.CurrentPageAttributeName) ||
			    context.AllAttributes.ContainsName(PagerTagHelper.TotalPageAttributeName))
			{
				// Source attribute cannot be used when current-page or total-page is specified.
				if (hasSourceAttr)
					throw new InvalidOperationException(
						$"The '{SourceAttributeName}' attribute cannot be used together with either '{PagerTagHelper.CurrentPageAttributeName}' or '{PagerTagHelper.TotalPageAttributeName}' attribute.");

				// Do nothing is source is not specified and current-page or total-page is specified.
				return;
			}

			IPagedList realSource;

			if (hasSourceAttr)
				realSource =
					Source ?? throw new InvalidOperationException(
						$"The pager source specified with '{SourceAttributeName}' attribute cannot be null.");
			else
				realSource = ViewContext.ViewData.Model is IPagedList modelSource
					? modelSource
					: throw new InvalidOperationException(
						$"The model of current view is either null or an object that cannot be converted to '{typeof(IPagedList).AssemblyQualifiedName}' type.");

			output.Attributes.SetAttribute(PagerTagHelper.CurrentPageAttributeName, realSource.PageIndex);
			output.Attributes.SetAttribute(PagerTagHelper.TotalPageAttributeName, realSource.TotalPage);
		}

		#endregion

		#region Core Property

		/// <summary>
		///     Get or set the source of the pager.
		/// </summary>
		[HtmlAttributeName(SourceAttributeName)]
		public IPagedList Source { get; set; }

		/// <summary>
		///     Get the HTML attribute name for <see cref="Source" /> property. This field is constant.
		/// </summary>
		[PublicAPI] public const string SourceAttributeName = "source";

		/// <summary>
		///     Get or set the <see cref="Microsoft.AspNetCore.Mvc.Rendering.ViewContext" /> for this tag helper.
		/// </summary>
		[HtmlAttributeNotBound]
		[ViewContext]
		public ViewContext ViewContext { get; set; }

		#endregion

		#region Order

		/// <summary>
		///     Get the default <see cref="Order" /> value for this tag helper. This field is constant.
		/// </summary>
		public const int DefaultOrder = PagerTagHelper.DefaultOrder - 1;

		/// <inheritdoc />
		public override int Order { get; } = DefaultOrder;

		#endregion
	}
}