using System;
using System.Globalization;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;

namespace Sakura.AspNetCore.Mvc.Generators
{
	/// <summary>
	///     Generate <see cref="IHtmlContent" /> from a formatted string. The page number will be used as the format argument
	///     {0}.
	/// </summary>
	public class FormattedStringContentGenerator : StringContentGenerator
	{
		/// <summary>
		///     Initialize a new generator with specified parameters.
		/// </summary>
		/// <param name="format">The format string used to be generate the content string..</param>
		/// <param name="encodeText">Whether the format result should be HTML encoded before be written to page.</param>
		/// <param name="formatProvider">
		///     The format provider used to generate the content string. If this parameter is <c>null</c>,
		///     <see cref="CultureInfo.CurrentCulture" /> will be used.
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="format" /> is <c>null</c>.</exception>
		public FormattedStringContentGenerator([LocalizationRequired] [NotNull] string format, bool encodeText,
			IFormatProvider formatProvider = null) : base(encodeText)
		{
			if (format == null)
			{
				throw new ArgumentNullException(nameof(format));
			}

			Format = format;
			FormatProvider = formatProvider ?? CultureInfo.CurrentCulture;
		}

		/// <summary>
		///     Get the format string used to be generate the content string.
		/// </summary>
		[PublicAPI]
		[NotNull]
		public string Format { get; }

		/// <summary>
		///     Get the format provider used to generate the content string.
		/// </summary>
		[PublicAPI]
		[NotNull]
		public IFormatProvider FormatProvider { get; }

		/// <summary>
		///     When be derived, generate the content string.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <returns>The generated content string.</returns>
		protected override string GenerateContentString(PagerItemGenerationContext context)
			=> string.Format(FormatProvider, Format, context.PagerItem.PageNumber);
	}
}