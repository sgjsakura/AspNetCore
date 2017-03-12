using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sakura.AspNetCore.Mvc.Internal;

namespace Sakura.AspNetCore.Mvc.Generators
{
	/// <summary>
	///     Provide the common feature for current uri based pager item link generator.
	/// </summary>
	public abstract class CurrentUriBasedLinkGenerator : IPagerItemLinkGenerator
	{
		/// <summary>
		///     Generate the link url for the specified <see cref="PagerItem" />.
		/// </summary>
		/// <param name="context">The generation context.</param>
		public string GenerateLink(PagerItemGenerationContext context)
		{
			var currentUri = GetCurrentUriWithQuery(context.ViewContext);

			// Hosted uri for API compatibility
			var hostedUri = new Uri("http://test.local/");

			// Generate uri object
			var uri = new Uri(currentUri, UriKind.RelativeOrAbsolute);

			// Start with slash detection.
			var isStartWithSlash = false;

			// If the uri is not absolute uri, convert it to an absolute one
			var isAbsolute = uri.IsAbsoluteUri;
			if (!isAbsolute)
			{
				isStartWithSlash = !string.IsNullOrEmpty(currentUri) && currentUri[0] == '/';
				uri = new Uri(hostedUri, uri);
			}

			// Core method to handle URI.
			var finalUri = HandleUriCore(uri, context);

			// If the original uri is not absolute, remove the hosted part
			if (!isAbsolute)
				finalUri = hostedUri.MakeRelativeUri(finalUri);

			var result = UriHelper.Encode(finalUri);

			// Add start slash if necessary
			if (!isAbsolute && isStartWithSlash)
				result = "/" + result;

			return result;
		}

		/// <summary>
		///     Get the current uri and query string from the current view context.
		/// </summary>
		/// <param name="viewContext">The view context object.</param>
		/// <returns>Current uri and query string of the view context.</returns>
		[Pure]
		protected static string GetCurrentUriWithQuery(ViewContext viewContext)
		{
			var request = viewContext.HttpContext.Request;
			return request.PathBase + request.Path + request.QueryString;
		}

		/// <summary>
		///     The core method for handling the current uri.
		/// </summary>
		/// <param name="currentUri">The URL to handle, this URL is ensured in absolute mode.</param>
		/// <param name="context">The generation context.</param>
		/// <returns></returns>
		/// <remarks>
		///     Most URI handling method requires the URI be absolute format, however in the view page relative URI is
		///     recommended. The <paramref name="currentUri" /> argument in this method has been handled and is ensured to be
		///     absolute. The generator will correctly recover it to the original format after handling.
		/// </remarks>
		protected abstract Uri HandleUriCore([NotNull] Uri currentUri, [NotNull] PagerItemGenerationContext context);
	}
}