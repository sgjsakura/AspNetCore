using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	/// <summary>
	///     Provide the base URI for a series of <see cref="IPagerItemLinkGenerator" />.
	/// </summary>
	[HtmlTargetElement(PagerTagHelper.HtmlTagName, Attributes = BaseUriAttributeName)]
	public class PagerLinkBaseUriTagHelper : PagerLinkBaseUriSettingTagHelper
	{
		public const string BaseUriAttributeName = "base-uri";

		[HtmlAttributeName(BaseUriAttributeName)]
		public string BaseUri { get; set; }

		/// <inheritdoc />
		protected override string GetBaseUri()
		{
			return BaseUri;
		}
	}
}