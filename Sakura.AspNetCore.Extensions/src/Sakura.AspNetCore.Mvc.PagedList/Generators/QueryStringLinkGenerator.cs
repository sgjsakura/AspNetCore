using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Sakura.AspNetCore.Mvc.Internal;

namespace Sakura.AspNetCore.Mvc.Generators
{
	/// <summary>
	///     Represent as base class for all query string based link generator.
	/// </summary>
	public abstract class QueryStringLinkGenerator : BaseUriLinkGenerator
	{
		/// <summary>
		///     Change a specified parameter value for an uri.
		/// </summary>
		/// <param name="baseUri">The uri string which need to be changed.</param>
		/// <param name="queryParameterName">The changing query parameter.</param>
		/// <param name="queryParameterValue">The new value of the query parameter.</param>
		/// <returns>
		///     A new uri string which the specified query parameter value is changed; If no matched query parameter is found,
		///     append the new value to the end.
		/// </returns>
		[PublicAPI]
		[Pure]
		protected static Uri ChangeQueryParameterValue(Uri baseUri, string queryParameterName,
			string queryParameterValue)
		{
			// Extract query string and change parameter
			var qs = QueryHelpers.ParseQuery(baseUri.Query);
			qs[queryParameterName] = new[] {queryParameterValue};

			var qb = new QueryBuilder();
			foreach (var item in qs)
				qb.Add(item.Key, (IEnumerable<string>) item.Value);

			// Rebuild uri
			var builder = new UriBuilder(baseUri)
			{
				Query = qb.ToString().Remove(0, 1) // Remove leading "?" mark
			};

			return builder.Uri;
		}

		/// <summary>
		///     When derived, generate the query parameter name for the specified <see cref="PagerItem" />.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <returns>The query parameter name for current pager item.</returns>
		public abstract string GenerateQueryParameterName([NotNull] PagerItemGenerationContext context);

		/// <summary>
		///     When derived, generate the query parameter value for the specified <see cref="PagerItem" />.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <returns>The query parameter name for current pager item.</returns>
		public abstract string GenerateQueryParameterValue([NotNull] PagerItemGenerationContext context);

		/// <summary>
		///     The core method for handling the current uri.
		/// </summary>
		/// <param name="baseUri">The URL to handle, this URL is ensured in absolute mode.</param>
		/// <param name="context">The generation context.</param>
		/// <returns>The generated URL string.</returns>
		/// <remarks>
		///     Most URI handling method requirest the URI be absolute format, however in the view page relative URI is
		///     recommended. The <paramref name="baseUri" /> argument in this method has been handled and is ensured to be
		///     absolute. The generator will correctly recover it to the original format after handling.
		/// </remarks>
		protected override Uri HandleUriCore(Uri baseUri, PagerItemGenerationContext context)
		{
			// Get name and value
			var name = GenerateQueryParameterName(context);
			var value = GenerateQueryParameterValue(context);

			// Generate result
			return ChangeQueryParameterValue(baseUri, name, value);
		}
	}
}