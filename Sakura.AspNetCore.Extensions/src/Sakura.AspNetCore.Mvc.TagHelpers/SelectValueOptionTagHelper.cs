using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	/// <summary>
	///     Dynamically set the option as selected if the value of the option matches the target value provided by the parent
	///     select element.
	/// </summary>
	[HtmlTargetElement("option")]
	[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
	public class SelectValueOptionTagHelper : TagHelper
	{
		/// <summary>
		///     Get the default mode when comparing values. This field is constant.
		/// </summary>
		public const StringComparison DefaultSelectValueCompareMode = StringComparison.OrdinalIgnoreCase;

		/// <summary>
		///     Get the key object to retrieve selected value from the executing context.
		/// </summary>
		public static object SelectedValueItemKey { get; } = new object();

		public static object SelectedValueCompareModeKey { get; } = new object();

		/// <summary>
		///     Provide a standard manner to add the selected value to the context.
		/// </summary>
		/// <param name="context">The context object.</param>
		/// <param name="value">The value to be added.</param>
		/// <param name="comparison">
		///     The comparison mode for the <paramref name="value" />. The default value of this parameter is
		///     defined in <see cref="DefaultSelectValueCompareMode" />.
		/// </param>
		[PublicAPI]
		public static void SetSelectedValue(TagHelperContext context, string value,
			StringComparison comparison = DefaultSelectValueCompareMode)
		{
			context.Items[SelectedValueItemKey] = value;
			context.Items[SelectedValueCompareModeKey] = comparison;
		}

		/// <summary>
		///     Try to get the selected value string from the context. If no selected value is set, this method will returns
		///     <c>null</c>.
		/// </summary>
		/// <param name="context">The tag helper context.</param>
		/// <returns>The selected value string from the context. If no selected value is set, this method will returns <c>null</c>.</returns>
		protected string GetSelectedValueFromContext(TagHelperContext context)
		{
			object result;
			return context.Items.TryGetValue(SelectedValueItemKey, out result) ? result as string : null;
		}

		/// <summary>
		///     Try to get the selected value comparison mode from the context. If no mode is set, this method will returns
		///     <see cref="DefaultSelectValueCompareMode" />.
		/// </summary>
		/// <param name="context">The tag helper context.</param>
		/// <returns>
		///     The selected value comparison mode from the context. If no mode is set, this method will returns
		///     <see cref="DefaultSelectValueCompareMode" />.
		/// </returns>
		protected StringComparison GetSelectedValueCompareModeFromContext(TagHelperContext context)
		{
			return DefaultSelectValueCompareMode;
		}

		/// <summary>
		///     Make the current option as selected.
		/// </summary>
		/// <param name="output">Tag output context.</param>
		protected virtual void MarkAsSelected(TagHelperOutput output)
		{
			output.Attributes.SetAttribute("selected", "selected");
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
			var selectedValue = GetSelectedValueFromContext(context);

			// Do nothing if no selected value is set.
			if (selectedValue == null)
			{
				return;
			}

			// Get the actual value
			var value = context.AllAttributes["value"]?.Value as string;

			// Compare value
			if (string.Equals(value, selectedValue, GetSelectedValueCompareModeFromContext(context)))
			{
				MarkAsSelected(output);
			}
		}
	}
}