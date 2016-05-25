using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;
using Sakura.AspNetCore.Mvc.Internal;

namespace Sakura.AspNetCore.Mvc.Generators
{
	/// <summary>
	///     Represent as generic base class for all <see cref="IPagerItemContentGenerator" /> which generate
	///     <see cref="IHtmlContent" /> from simple <see cref="string" /> value.
	/// </summary>
	public abstract class StringContentGenerator : IPagerItemContentGenerator
	{
		/// <summary>
		///     Initialize a new generator with specified information.
		/// </summary>
		/// <param name="encodeText">
		///     Indicate that whether the generated string content should be HTML encoded before be written to
		///     page.
		/// </param>
		protected StringContentGenerator(bool encodeText)
		{
			EncodeText = encodeText;
		}

		/// <summary>
		///     Get a value that indicates that whether the generated string content should be HTML encoded before be written to
		///     page.
		/// </summary>
		[PublicAPI]
		public bool EncodeText { get; }

		/// <summary>
		///     Generate the content for a specified pager item.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <returns>The generated HTML content for the pager item.</returns>
		public IHtmlContent GenerateContent(PagerItemGenerationContext context)
			=> GenerateContentString(context).ToHtmlContent(EncodeText);

		/// <summary>
		///     When be derived, generate the content string.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <returns>The generated content string.</returns>
		protected abstract string GenerateContentString(PagerItemGenerationContext context);
	}
}