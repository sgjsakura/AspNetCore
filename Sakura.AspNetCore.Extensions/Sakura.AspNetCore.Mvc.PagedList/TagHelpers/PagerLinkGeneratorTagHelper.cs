using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	/// <summary>
	///     Base class for all pager link generation tag helper.
	/// </summary>
	public abstract class PagerLinkGeneratorTagHelper : TagHelper
	{
		#region Core Methods

		/// <summary>
		///     Get the <see cref="IPagerItemLinkGenerator" /> associated with this tag helper.
		/// </summary>
		/// <returns>The <see cref="IPagerItemLinkGenerator" /> associated with this tag helper.</returns>
		public abstract IPagerItemLinkGenerator GetLinkGenerator();

		#endregion

		/// <inheritdoc />
		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			context.CheckAttributeConflicting(PagerTagHelper.ItemDefaultLinkGeneratorAttributeName);
			output.Attributes.SetAttribute(PagerTagHelper.ItemDefaultLinkGeneratorAttributeName, GetLinkGenerator());
		}

		#region Order

		/// <summary>
		///     Get the default <see cref="Order" /> value for this tag helper. This field is constant.
		/// </summary>
		public const int DefaultOrder = PagerTagHelper.DefaultOrder - 1;

		/// <summary>
		///     Get the default <see cref="Order" /> value for this tag helper. This field is constant.
		/// </summary>
		/// <inheritdoc />
		public override int Order { get; } = DefaultOrder;

		#endregion
	}
}