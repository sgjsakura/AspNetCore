using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	/// <summary>
	/// Provide common feature used to generate pager links with a specified base URI.
	/// </summary>
	public abstract class PagerBaseUriLinkGeneratorTagHelper : PagerLinkGeneratorTagHelper
	{
		#region Constructor

		/// <summary>
		/// Initialize a new instance of <see cref="PagerBaseUriLinkGeneratorTagHelper"/>.
		/// </summary>
		/// <param name="urlHelperFactory">The <see cref="IUrlHelperFactory"/> service instance.</param>
		protected PagerBaseUriLinkGeneratorTagHelper(IUrlHelperFactory urlHelperFactory)
		{
			UrlHelperFactory = urlHelperFactory;
		}

		#endregion

		#region Base URI Related Properties

		/// <summary>
		/// Internal dictionary for route values.
		/// </summary>
		private IDictionary<string, string> _routeValues;

		/// <summary>The name of the action method.</summary>
		/// <remarks>Must be <c>null</c> if <see cref="Route" /> is non-<c>null</c>.</remarks>
		[HtmlAttributeName("asp-action")]
		public string Action { get; set; }

		/// <summary>The name of the controller.</summary>
		/// <remarks>Must be <c>null</c> if <see cref="Route" /> is non-<c>null</c>.</remarks>
		[HtmlAttributeName("asp-controller")]
		public string Controller { get; set; }

		/// <summary>The name of the area.</summary>
		/// <remarks>Must be <c>null</c> if <see cref="Route" /> is non-<c>null</c>.</remarks>
		[HtmlAttributeName("asp-area")]
		public string Area { get; set; }

		/// <summary>The protocol for the URL, such as "http" or "https".</summary>
		[HtmlAttributeName("asp-protocol")]
		public string Protocol { get; set; }

		/// <summary>The host name.</summary>
		[HtmlAttributeName("asp-host")]
		public string Host { get; set; }

		/// <summary>The URL fragment name.</summary>
		[HtmlAttributeName("asp-fragment")]
		public string Fragment { get; set; }

		/// <summary>Name of the route.</summary>
		/// <remarks>
		/// Must be <c>null</c> if <see cref="Action" /> or <see cref="Controller" /> is non-<c>null</c>.
		/// </remarks>
		[HtmlAttributeName("asp-route")]
		public string Route { get; set; }

		/// <summary>Additional parameters for the route.</summary>
		[HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]
		public IDictionary<string, string> RouteValues
		{
			get => _routeValues ?? (_routeValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));
			set => _routeValues = value;
		}

		#endregion

		#region Service Instances

		[ViewContext]
		[HtmlAttributeNotBound]
		public ViewContext ViewContext { get; set; }

		protected IUrlHelperFactory UrlHelperFactory { get; }

		#endregion

		/// <summary>
		/// Generate the base URI used to calculate the final URL.
		/// </summary>
		/// <returns></returns>
		protected string GenerateBaseUri()
		{
			var helper = UrlHelperFactory.GetUrlHelper(ViewContext);
			return helper.Action(Action, Controller, RouteValues, Protocol, Host, Fragment);
		}

		/// <inheritdoc />
		public override IPagerItemLinkGenerator GetLinkGenerator()
		{
			return GetLinkGenerator(GenerateBaseUri());
		}

		/// <summary>
		/// Generate the final URI according to a specified base URI.
		/// </summary>
		/// <param name="baseUri">The base URI.</param>
		/// <returns>The final URI.</returns>
		protected abstract IPagerItemLinkGenerator GetLinkGenerator(string baseUri);
	}
}
