using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	/// <summary>
	///     Provide select options geneartions for a model expression with enum type.
	/// </summary>
	[HtmlTargetElement("select", Attributes = EnumForAttributeName)]
	public class EnumForSelectTagHelper : EnumSelectTagHelper
	{
		/// <summary>
		///     Initialize an new instance with required services.
		/// </summary>
		/// <param name="generator">The HTML generator service.</param>
		/// <param name="stringLocalizerFactory">The service instance of <see cref="IStringLocalizerFactory" />.</param>
		public EnumForSelectTagHelper(IHtmlGenerator generator, IStringLocalizerFactory stringLocalizerFactory)
			: base(stringLocalizerFactory)
		{
			Generator = generator;
		}

		/// <summary>
		///     Get or set the view context object.
		/// </summary>
		[HtmlAttributeNotBound]
		[ViewContext]
		public ViewContext ViewContext { get; set; }

		/// <summary>
		///     Get the Html Generator service object.
		/// </summary>
		[PublicAPI]
		protected IHtmlGenerator Generator { get; }

		/// <summary>
		///     Return the actual enum type for generating the list.
		/// </summary>
		/// <returns></returns>
		protected override Type GetEnumType()
		{
			if (EnumFor == null)
				throw new InvalidOperationException($"The expression for `{EnumForAttributeName}` attribute cannot be null.");

			if (!EnumFor.Metadata.IsEnum)
				throw new InvalidOperationException(
					$"The expression type for `{EnumForAttributeName}` attribute is not a valid enum type nor a nullable enum type.");

			return EnumFor.Metadata.UnderlyingOrModelType;
		}

		/// <summary>
		///     Synchronously executes the <see cref="TagHelper" /> with the given <paramref name="context" /> and
		///     <paramref name="output" />.
		/// </summary>
		/// <param name="context">Contains information associated with the current HTML tag.</param>
		/// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			// Generate the list.
			var selectListItems = GenerateListForEnumType();

			// Get current value for model expression.
			var currentValues = Generator.GetCurrentValues(ViewContext, EnumFor.ModelExplorer, EnumFor.Name, false);


			var tag = Generator.GenerateSelect(ViewContext, EnumFor?.ModelExplorer, null, EnumFor?.Name, selectListItems,
				currentValues, false,
				null);

			if (tag != null)
			{
				output.MergeAttributes(tag);
				output.PostContent.AppendHtml(tag.InnerHtml);
			}
		}

		#region Constants for attributes

		/// <summary>
		///     Get the HTML name associated with the <see cref="EnumFor" /> property. This field is constant.
		/// </summary>
		[PublicAPI] public const string EnumForAttributeName = "asp-enum-for";

		/// <summary>
		///     Get or set the model expression used to determine the enum type used to generate the items.
		/// </summary>
		[HtmlAttributeName(EnumForAttributeName)]
		public ModelExpression EnumFor { get; set; }

		#endregion
	}
}