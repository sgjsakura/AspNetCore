using System;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	/// <summary>
	///     Provide <see cref="QueryName" /> property to help generate pager links.
	/// </summary>
	[HtmlTargetElement(PagerTagHelper.HtmlTagName, Attributes = QueryNameAttributeName)]
	public class PagerLinkQueryNameTagHelper : PagerLinkBaseUriAspRouteTagHelper
	{
		#region Constructor

		/// <summary>
		///     Initialize a new instance of <see cref="PagerLinkQueryNameTagHelper" />.
		/// </summary>
		/// <param name="urlHelperFactory">The <see cref="IUrlHelperFactory" /> service instance.</param>
		public PagerLinkQueryNameTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory)
		{
		}

		#endregion

		#region Core Methods

		/// <inheritdoc />
		protected override IPagerItemLinkGenerator GetLinkGenerator(string baseUri)
		{
			if (string.IsNullOrEmpty(QueryName))
				throw new InvalidOperationException(
					$"The value of '{QueryNameAttributeName}' attribute cannot be null or empty string.");

			var result = PagerItemLinkGenerators.QueryName(QueryName);
			result.BaseUri = baseUri;

			return result;
		}

		#endregion

		#region Core Properties

		/// <summary>
		///     Get or set the query name used to generate the link.
		/// </summary>
		[HtmlAttributeName(QueryNameAttributeName)]
		public string QueryName { get; set; }

		/// <summary>
		///     The HTML attribute name for <see cref="QueryName" /> attribute. This field is constant.
		/// </summary>
		public const string QueryNameAttributeName = "link-query-name";

		#endregion
	}
}