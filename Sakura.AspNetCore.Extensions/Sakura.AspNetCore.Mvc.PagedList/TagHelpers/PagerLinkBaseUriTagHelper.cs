using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Sakura.AspNetCore.Mvc;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	/// <summary>
	/// Provide the base URI for a series of <see cref="IPagerItemLinkGenerator"/>.
	/// </summary>
	[HtmlTargetElement(PagerTagHelper.HtmlTagName, Attributes = BaseUriAttributeName)]
	public class PagerLinkBaseUriTagHelper : PagerLinkBaseUriSettingTagHelper
	{
		[HtmlAttributeName(BaseUriAttributeName)]
		public string BaseUri { get; set; }

		public const string BaseUriAttributeName = "base-uri";

		/// <inheritdoc />
		protected override string GetBaseUri() => BaseUri;
	}
}
