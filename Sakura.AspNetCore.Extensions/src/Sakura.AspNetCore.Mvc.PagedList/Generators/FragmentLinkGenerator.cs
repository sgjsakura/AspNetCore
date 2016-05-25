using System;
using JetBrains.Annotations;

namespace Sakura.AspNetCore.Mvc.Generators
{
	/// <summary>
	///     Generate pager link into current URL's hash (fragment) part.
	/// </summary>
	public abstract class FragmentLinkGenerator : CurrentUriBasedLinkGenerator
	{
		/// <summary>
		///     The core method for handling the current uri.
		/// </summary>
		/// <param name="currentUri">The URL to handle, this URL is ensured in absolute mode.</param>
		/// <param name="context">The generation context.</param>
		/// <returns></returns>
		/// <remarks>
		///     Most URI handling method requirest the URI be absolute format, however in the view page relative URI is
		///     recommended. The <paramref name="currentUri" /> argument in this method has been handled and is ensured to be
		///     absolute. The generator will correctly recover it to the original format after handling.
		/// </remarks>
		protected override string HandleUriCore(string currentUri, PagerItemGenerationContext context)
		{
			// Create URI and change fragment
			var uriBuilder = new UriBuilder(currentUri)
			{
				Fragment = GenerateFragment(context)
			};

			return uriBuilder.ToString();
		}


		/// <summary>
		///     When derived, generate the fragment part for a specified pager item.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <returns></returns>
		protected abstract string GenerateFragment([NotNull] PagerItemGenerationContext context);
	}
}