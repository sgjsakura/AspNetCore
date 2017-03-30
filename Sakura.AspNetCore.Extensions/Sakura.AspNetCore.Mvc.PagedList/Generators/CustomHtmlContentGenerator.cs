using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;

namespace Sakura.AspNetCore.Mvc.Generators
{
	/// <summary>
	///     Generate <see cref="IHtmlContent" /> from a custom generator.
	/// </summary>
	public class CustomHtmlContentGenerator : IPagerItemContentGenerator
	{
		/// <summary>
		/// </summary>
		/// <param name="htmlContentGenerator">The string generator callback delegate.</param>
		/// <exception cref="ArgumentNullException"><paramref name="htmlContentGenerator" /> is <c>null</c>.</exception>
		public CustomHtmlContentGenerator([NotNull] Func<PagerItemGenerationContext, IHtmlContent> htmlContentGenerator)
		{
			HtmlContentGenerator = htmlContentGenerator ?? throw new ArgumentNullException(nameof(htmlContentGenerator));
		}

		/// <summary>
		///     Get the string generator callback delegate.
		/// </summary>
		[PublicAPI]
		[NotNull]
		public Func<PagerItemGenerationContext, IHtmlContent> HtmlContentGenerator { get; }

		/// <summary>
		///     Generate the content for a specified pager item.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <returns>The generated HTML content for the pager item.</returns>
		public IHtmlContent GenerateContent(PagerItemGenerationContext context)
		{
			return HtmlContentGenerator(context);
		}
	}
}