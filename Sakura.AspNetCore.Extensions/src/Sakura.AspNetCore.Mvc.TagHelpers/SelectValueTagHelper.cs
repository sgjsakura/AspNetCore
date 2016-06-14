using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	/// <summary>
	///     Add support for set automatically mark selected state for options.
	/// </summary>
	[HtmlTargetElement("select", Attributes = ValueAttributeName)]
	[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
	public class SelectValueTagHelper : TagHelper
	{
		/// <inheritdoc />
		public override void Init(TagHelperContext context)
		{
			base.Init(context);
			SelectValueOptionTagHelper.SetSelectedValue(context, Value, ValueCompareMode);
		}

		#region Html bound Properties

		/// <summary>
		///     Get the HTML name associated with the <see cref="Value" /> property. This field is constant.
		/// </summary>
		[PublicAPI] public const string ValueAttributeName = "asp-value";

		/// <summary>
		///     Get or set the selected item's value of the current element.
		/// </summary>
		[HtmlAttributeName(ValueAttributeName)]
		public string Value { get; set; }

		/// <summary>
		///     Get the HTML name associated with the <see cref="ValueCompareMode" /> property. This field is constant.
		/// </summary>
		public const string ValueCompareModeAttributeName = "asp-value-compare-mode";

		/// <summary>
		///     Get or set the value comparison mode for the selected value. The default value of this property is
		///     <see cref=" SelectValueOptionTagHelper.DefaultSelectValueCompareMode" />.
		/// </summary>
		[HtmlAttributeName(ValueCompareModeAttributeName)]
		public StringComparison ValueCompareMode { get; set; } = SelectValueOptionTagHelper.DefaultSelectValueCompareMode;

		#endregion
	}
}