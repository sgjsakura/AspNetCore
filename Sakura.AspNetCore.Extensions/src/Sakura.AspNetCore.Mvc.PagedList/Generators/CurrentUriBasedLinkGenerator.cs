using JetBrains.Annotations;
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
			return HandleUriCore(currentUri, context);
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
			return request.Path + request.QueryString;
		}

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
		protected abstract string HandleUriCore([NotNull] string currentUri, [NotNull] PagerItemGenerationContext context);
	}
}