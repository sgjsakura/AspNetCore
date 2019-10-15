using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	/// <summary>
	///     Provide select options geneartions for enum items.
	/// </summary>
	[HtmlTargetElement("select", Attributes = EnumTypeAttributeName)]
	[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
	public class EnumTypeSelectTagHelper : EnumSelectTagHelper
	{
		#region Constrcutor

		/// <summary>
		///     Create a new instance of <see cref="EnumTypeSelectTagHelper" />.
		/// </summary>
		/// <param name="serviceProvider">The service instance of <see cref="IServiceProvider" />.</param>
		public EnumTypeSelectTagHelper(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		#endregion

		/// <summary>
		///     Return the actual enum type for generating the list.
		/// </summary>
		/// <returns></returns>
		protected override Type GetEnumType()
		{
			if (EnumType == null)
				throw new InvalidOperationException(
					$"The expression for '{EnumTypeAttributeName}' attribute cannot be null.");

			// Remove nullable if necessary
			var type = Nullable.GetUnderlyingType(EnumType) ?? EnumType;

			if (!type.GetTypeInfo().IsEnum)
				throw new InvalidOperationException(
					$"The specified type '{EnumType.AssemblyQualifiedName}' for '{EnumTypeAttributeName}' attribute is not a valid enum type nor a nullable enum type.");

			return type;
		}

		/// <summary>
		///     Synchronously executes the <see cref="T:Microsoft.AspNet.Razor.TagHelpers.TagHelper" /> with the given
		///     <paramref name="context" /> and
		///     <paramref name="output" />.
		/// </summary>
		/// <param name="context">Contains information associated with the current HTML tag.</param>
		/// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			// Get list and convert to array (avoiding duplicate enumeration)
			var items = GenerateListForEnumType().ToArray();

			// Get value
			var selectedValue = EnumValue == null ? string.Empty : GetValueForMember(EnumValue.GetMember());

			// Set selected Item
			var selectedItem = items.FirstOrDefault(i => i.Value == selectedValue);

			if (selectedItem != null)
			{
				selectedItem.Selected = true;
			}

			// Append list items
			output.PostContent.AppendHtml(HtmlGeneratorHelper.GenerateGroupsAndOptions(null, items));
		}

		#region Constants for attributes

		/// <summary>
		///     Get the HTML name associated with the <see cref="EnumType" /> property. This field is constant.
		/// </summary>
		[PublicAPI] public const string EnumTypeAttributeName = "asp-enum-type";

		/// <summary>
		///     Get the HTML name associated with the <see cref="EnumValueAttributeName" /> property. This field is constant.
		/// </summary>
		[PublicAPI] public const string EnumValueAttributeName = "asp-enum-value";

		/// <summary>
		///     Get or set the enum type used to generate the items.
		/// </summary>
		[HtmlAttributeName(EnumTypeAttributeName)]
		public Type EnumType { get; set; }

		/// <summary>
		///     Get or set the the selected value for the select tag.
		/// </summary>
		[HtmlAttributeName(EnumValueAttributeName)]
		public Enum EnumValue { get; set; }

		#endregion
	}
}