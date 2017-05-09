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
	[HtmlTargetElement(PagerTagHelper.HtmlTagName)]
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

		#region Constants

		/// <summary>
		/// Get the HTML attribute name associated with the <see cref="Action"/> property. This field is constant.
		/// </summary>
		private const string ActionAttributeName = "asp-action";
		/// <summary>
		/// Get the HTML attribute name associated with the <see cref="Controller"/> property. This field is constant.
		/// </summary>
		private const string ControllerAttributeName = "asp-controller";
		/// <summary>
		/// Get the HTML attribute name associated with the <see cref="Area"/> property. This field is constant.
		/// </summary>
		private const string AreaAttributeName = "asp-area";
		/// <summary>
		/// Get the HTML attribute name associated with the <see cref="Protocol"/> property. This field is constant.
		/// </summary>
		private const string ProtocolAttributeName = "asp-protocol";
		/// <summary>
		/// Get the HTML attribute name associated with the <see cref="Host"/> property. This field is constant.
		/// </summary>
		private const string HostAttributeName = "asp-host";
		/// <summary>
		/// Get the HTML attribute name associated with the <see cref="Fragment"/> property. This field is constant.
		/// </summary>
		private const string FragmentAttributeName = "asp-fragment";
		/// <summary>
		/// Get the HTML attribute name associated with the <see cref="Route"/> property. This field is constant.
		/// </summary>
		public const string RouteAttributeName = "asp-route";
		/// <summary>
		/// Get the HTML attribute name associated with the <see cref="RouteValues"/> property. This field is constant.
		/// </summary>
		private const string AllRouteDataAttributeName = "asp-all-route-data";
		/// <summary>
		/// Get the HTML attribute prefix associated with the <see cref="RouteValues"/> property. This field is constant.
		/// </summary>
		private const string RouteAttributePrefix = "asp-route-";

		#endregion

		#endregion

		#region Base URI Related Properties

		/// <summary>
		/// Internal dictionary for route values.
		/// </summary>
		private IDictionary<string, string> _routeValues;

		/// <summary>The name of the action method.</summary>
		/// <remarks>Must be <c>null</c> if <see cref="Route" /> is non-<c>null</c>.</remarks>
		[HtmlAttributeName(ActionAttributeName)]
		public string Action { get; set; }

		/// <summary>The name of the controller.</summary>
		/// <remarks>Must be <c>null</c> if <see cref="Route" /> is non-<c>null</c>.</remarks>
		[HtmlAttributeName(ControllerAttributeName)]
		public string Controller { get; set; }

		/// <summary>The name of the area.</summary>
		/// <remarks>Must be <c>null</c> if <see cref="Route" /> is non-<c>null</c>.</remarks>
		[HtmlAttributeName(AreaAttributeName)]
		public string Area { get; set; }

		/// <summary>The protocol for the URL, such as "http" or "https".</summary>
		[HtmlAttributeName(ProtocolAttributeName)]
		public string Protocol { get; set; }

		/// <summary>The host name.</summary>
		[HtmlAttributeName(HostAttributeName)]
		public string Host { get; set; }

		/// <summary>The URL fragment name.</summary>
		[HtmlAttributeName(FragmentAttributeName)]
		public string Fragment { get; set; }

		/// <summary>Name of the route.</summary>
		/// <remarks>
		/// Must be <c>null</c> if <see cref="Action" /> or <see cref="Controller" /> is non-<c>null</c>.
		/// </remarks>
		[HtmlAttributeName(RouteAttributeName)]
		public string Route { get; set; }

		/// <summary>Additional parameters for the route.</summary>
		[HtmlAttributeName(AllRouteDataAttributeName, DictionaryAttributePrefix = RouteAttributePrefix)]
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
