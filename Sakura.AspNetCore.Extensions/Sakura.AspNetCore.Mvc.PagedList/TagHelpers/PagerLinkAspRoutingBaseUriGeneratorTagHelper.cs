using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	public class PagerLinkAspRoutingBaseUriGeneratorTagHelper : PagerLinkBaseUriAspRouteTagHelper
	{
		public PagerLinkAspRoutingBaseUriGeneratorTagHelper(IUrlHelperFactory urlHelperFactory)
			: base(urlHelperFactory)
		{
		}

		protected override IPagerItemLinkGenerator GetLinkGenerator(string baseUri)
		{
			throw new NotImplementedException();
		}

		#region Html Attributes

		[HtmlAttributeName("asp-controller")]
		[AspMvcController]
		public string ControllerName { get; set; }

		[HtmlAttributeName("asp-action")]
		[AspMvcAction]
		public string ActionName { get; set; }

		[HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]
		public Dictionary<string, string> RouteAttributes { get; set; }

		#endregion
	}
}
