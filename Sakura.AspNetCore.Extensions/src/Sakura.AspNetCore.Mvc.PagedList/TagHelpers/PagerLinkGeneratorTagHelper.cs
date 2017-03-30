using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	/// <summary>
	/// Base class for all pager link generation tag helper.
	/// </summary>
	public abstract class PagerLinkGeneratorTagHelper : TagHelper
	{
		#region Core Methods

		/// <summary>
		/// Get the <see cref="IPagerItemLinkGenerator"/> associated with this tag helper.
		/// </summary>
		/// <returns>The <see cref="IPagerItemLinkGenerator"/> associated with this tag helper.</returns>
		public abstract IPagerItemLinkGenerator GetLinkGenerator();

		#endregion

		#region Order

		/// <summary>
		/// Get the default <see cref="Order"/> value for this tag helper. This field is constant.
		/// </summary>
		public const int DefaultOrder = PagerTagHelper.DefaultOrder - 1;
		/// <summary>
		/// Get the default <see cref="Order"/> value for this tag helper. This field is constant.
		/// </summary>
		/// <inheritdoc />
		public override int Order { get; } = DefaultOrder;

		#endregion

		/// <inheritdoc />
		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			// Check for confliction
			if (context.AllAttributes.ContainsName(PagerTagHelper.ItemDefaultLinkGeneratorAttributeName))
			{
				throw new InvalidOperationException($"The '{PagerTagHelper.ItemDefaultLinkGeneratorAttributeName}' attribute has already set from code explicitly or from another tag helper.");
			}

			output.Attributes.SetAttribute(PagerTagHelper.ItemDefaultLinkGeneratorAttributeName, GetLinkGenerator());
		}
	}
}
