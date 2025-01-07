using System;
using System.Globalization;
using JetBrains.Annotations;
using Sakura.AspNetCore.Mvc.Internal;

namespace Sakura.AspNetCore.Mvc.Generators;

/// <summary>
///     Generate link url string from a formatted string. The page number will be used as the format argument {0}.	///
/// </summary>
public class FormattedLinkGenerator : IPagerItemLinkGenerator
{
	/// <summary>
	///     Initialize a new generator with specified parameters.
	/// </summary>
	/// <param name="format">The format string used to be generate the link.</param>
	/// <param name="formatProvider">
	///     The format provider used to generate the link. If this parameter is <c>null</c>,
	///     <see cref="CultureInfo.InvariantCulture" /> will be used.
	/// </param>
	/// <exception cref="ArgumentNullException"><paramref name="format" /> is <c>null</c>.</exception>
	public FormattedLinkGenerator([NotNull] string format,
		IFormatProvider formatProvider = null)
	{
		Format = format ?? throw new ArgumentNullException(nameof(format));
		FormatProvider = formatProvider ?? CultureInfo.InvariantCulture;
	}

	/// <summary>
	///     Get the format string used to be generate the link.
	/// </summary>
	[PublicAPI]
	[NotNull]
	public string Format { get; }

	/// <summary>
	///     Get the format provider used to generate the link.
	/// </summary>
	[PublicAPI]
	[NotNull]
	public IFormatProvider FormatProvider { get; }

	#region Implementation of IPagerItemLinkGenerator

	/// <summary>
	///     Generate the link url for the specified <see cref="PagerItem" />.
	/// </summary>
	/// <param name="context">The generation context.</param>
	public string GenerateLink(PagerItemGenerationContext context)
	{
		return string.Format(FormatProvider, Format, context.PagerItem.PageNumber);
	}

	#endregion
}