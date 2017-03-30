using System;
using JetBrains.Annotations;
using Sakura.AspNetCore.Mvc.Internal;

namespace Sakura.AspNetCore.Mvc.Generators
{
	/// <summary>
	///     Generate a link url from a simple string.
	/// </summary>
	public class SimpleLinkGenerator : IPagerItemLinkGenerator
	{
		/// <summary>
		///     Initialize a new generator with specified information.
		/// </summary>
		/// <param name="text">The text content.</param>
		/// <exception cref="ArgumentNullException"><paramref name="text" /> is <c>null</c>.</exception>
		public SimpleLinkGenerator([NotNull] string text)
		{
			Text = text ?? throw new ArgumentNullException(nameof(text));
		}

		/// <summary>
		///     Get the text content.
		/// </summary>
		[PublicAPI]
		[NotNull]
		public string Text { get; }

		#region Implementation of IPagerItemLinkGenerator

		/// <summary>
		///     Generate the link url for the specified <see cref="PagerItem" />.
		/// </summary>
		/// <param name="context">The generation context.</param>
		public string GenerateLink(PagerItemGenerationContext context)
		{
			return Text;
		}

		#endregion
	}
}