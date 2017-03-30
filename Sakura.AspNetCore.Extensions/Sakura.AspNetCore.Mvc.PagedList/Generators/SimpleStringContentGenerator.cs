using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;

namespace Sakura.AspNetCore.Mvc.Generators
{
	/// <summary>
	///     Generate <see cref="IHtmlContent" /> from a simple encoded or not encoded string.
	/// </summary>
	public class SimpleStringContentGenerator : StringContentGenerator
	{
		/// <summary>
		///     Initialize a new generator with specified information.
		/// </summary>
		/// <param name="text">The text content.</param>
		/// <param name="encodeText">
		///     Indicate that whether the generated string content should be HTML encoded before be written to
		///     page.
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="text" /> is <c>null</c>.</exception>
		public SimpleStringContentGenerator([LocalizationRequired] [NotNull] string text, bool encodeText) : base(encodeText)
		{
			Text = text ?? throw new ArgumentNullException(nameof(text));
		}

		/// <summary>
		///     Get the text content.
		/// </summary>
		[PublicAPI]
		[NotNull]
		public string Text { get; }

		/// <summary>
		///     When be derived, generate the content string.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <returns>The generated content string.</returns>
		protected override string GenerateContentString(PagerItemGenerationContext context)
		{
			return Text;
		}
	}
}