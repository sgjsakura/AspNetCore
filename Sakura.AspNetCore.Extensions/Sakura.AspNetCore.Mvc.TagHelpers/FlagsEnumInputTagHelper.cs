using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	/// <summary>
	///     Support binding checkbox with enum flags.
	/// </summary>
	[HtmlTargetElement("input", Attributes = EnumFlagForAttributeName + "," + EnumFlagValueAttributeName)]
	public class FlagsEnumInputTagHelper : TagHelper
	{
		/// <summary>
		///     Define the attribute name for <see cref="EnumFlagValue" /> property. This field is constant.
		/// </summary>
		[PublicAPI] public const string EnumFlagValueAttributeName = "asp-enum-flag-value";

		/// <summary>
		///     Define the attribute name for <see cref="EnumFlagFor" /> property. This field is constant.
		/// </summary>
		[PublicAPI] public const string EnumFlagForAttributeName = "asp-enum-flag-for";

		/// <summary>
		///     Creates a new <see cref="InputTagHelper" />.
		/// </summary>
		[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
		public FlagsEnumInputTagHelper(IHtmlGenerator htmlGenerator, IHtmlHelper htmlHelper)
		{
			HtmlGenerator = htmlGenerator;
			HtmlHelper = htmlHelper;
		}

		/// <inheritdoc />
		/// <remarks>
		///     Default order is <c>0</c>.
		/// </remarks>
		public override int Order => new InputTagHelper(HtmlGenerator).Order - 10;

		/// <summary>
		///     The <see cref="IHtmlGenerator" /> Service object.
		/// </summary>
		[PublicAPI]
		protected IHtmlGenerator HtmlGenerator { get; }

		/// <summary>
		///     The <see cref="IHtmlHelper" /> Service object.
		/// </summary>
		[PublicAPI]
		protected IHtmlHelper HtmlHelper { get; }

		/// <summary>
		///     The <see cref="ViewContext" /> Service object.
		/// </summary>
		[ViewContext]
		protected ViewContext ViewContext { get; set; }

		/// <summary>
		///     Get or set the model expression to retrieve the enum flag value from model.
		/// </summary>
		[HtmlAttributeName(EnumFlagForAttributeName)]
		public ModelExpression EnumFlagFor { get; set; }

		/// <summary>
		///     Get or set the actual flag value to compare.
		/// </summary>
		[HtmlAttributeName(EnumFlagValueAttributeName)]
		public Enum EnumFlagValue { get; set; }

		/// <inheritdoc />
		/// <remarks>
		///     Does nothing if <see cref="InputTagHelper.For" /> is <c>null</c>.
		/// </remarks>
		/// <exception cref="InvalidOperationException">
		///     Thrown if <see cref="InputTagHelper.Format" /> is non-<c>null</c> but <see cref="InputTagHelper.For" /> is
		///     <c>null</c>.
		/// </exception>
		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			// Argument check
			if (EnumFlagValue == null)
				throw new InvalidOperationException(
					$"The value of the {EnumFlagValueAttributeName} attribute cannot be empty.");

			if (!EnumFlagFor.ModelExplorer.Metadata.IsFlagsEnum)
				throw new InvalidOperationException(
					$"The model expression type must be enum flag when {EnumFlagValueAttributeName} is specified");

			// Get base type and remove nullable
			var type = EnumFlagFor.ModelExplorer.ModelType;
			type = Nullable.GetUnderlyingType(type) ?? type;

			((IViewContextAware) HtmlHelper).Contextualize(ViewContext);

			// Get the defined enum name
			var enumName = Enum.GetName(type, EnumFlagValue);
			if (enumName == null)
				throw new InvalidOperationException(
					$"The value of the {EnumFlagValueAttributeName} attribute is not a valid enum flag item.");

			
			// Now you can use the HtmlFieldPrefix if set
			var name = ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix;
			if (!string.IsNullOrEmpty(name))
			{
				name += ".";
			}

			name += EnumFlagFor.Name;

			// Set name and value
			output.Attributes.SetAttribute("id", HtmlHelper.GenerateIdFromName($"{name}.{enumName}"));
			output.Attributes.SetAttribute("name", name);
			output.Attributes.SetAttribute("value", enumName);

			// Set checked attribute
			if (EnumFlagFor.Model is Enum value && value.HasFlag(EnumFlagValue))
			{
				output.Attributes.SetAttribute("checked", "checked");
			}
			// Remove checked attribute
			else
			{
				var attr = output.Attributes["checked"];
				if (attr != null)
					output.Attributes.Remove(attr);
			}
		}
	}
}