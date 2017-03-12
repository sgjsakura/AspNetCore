using System;
using JetBrains.Annotations;
using Sakura.AspNetCore.Mvc.Internal;

namespace Sakura.AspNetCore.Mvc.Generators
{
	/// <summary>
	///     Generate a link url string for a custom string generator.
	/// </summary>
	public class CustomLinkGenerator : IPagerItemLinkGenerator
	{
		/// <summary>
		///     Initialize a new instance of <see cref="CustomLinkGenerator" />.
		/// </summary>
		/// <param name="linkGenerator">The link genreator callback delegate.</param>
		public CustomLinkGenerator([NotNull] Func<PagerItemGenerationContext, string> linkGenerator)
		{
			LinkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
		}

		/// <summary>
		///     Get the link genreator callback delegate.
		/// </summary>
		[PublicAPI]
		[NotNull]
		public Func<PagerItemGenerationContext, string> LinkGenerator { get; }

		/// <summary>
		///     Generate the link url for the specified <see cref="PagerItem" />.
		/// </summary>
		/// <param name="context">The generation context.</param>
		public string GenerateLink(PagerItemGenerationContext context)
		{
			return LinkGenerator(context);
		}
	}
}