using Sakura.AspNetCore.Mvc.Internal;

namespace Sakura.AspNetCore.Mvc.Generators
{
	/// <summary>
	///     This type is used to disable link generation for a pager item.
	/// </summary>
	public class DisabledLinkGenerator : IPagerItemLinkGenerator
	{
		/// <summary>
		///     Generate the link url for the specified <see cref="PagerItem" />.
		/// </summary>
		/// <param name="context">The generation context.</param>
		public string GenerateLink(PagerItemGenerationContext context)
		{
			return null;
		}
	}
}