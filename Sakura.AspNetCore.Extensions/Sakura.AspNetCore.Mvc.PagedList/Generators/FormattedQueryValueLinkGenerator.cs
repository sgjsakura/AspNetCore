using System;
using System.Globalization;
using JetBrains.Annotations;
using Sakura.AspNetCore.Mvc.Internal;

namespace Sakura.AspNetCore.Mvc.Generators
{
	/// <summary>
	///     Generate the query parammeter value from a formatted string. The page number will be used as the format argument
	///     {0}.
	/// </summary>
	public class FormattedQueryValueLinkGenerator : QueryValueLinkGenerator
	{
		/// <summary>
		///     Initialize a new instance of the generator.
		/// </summary>
		/// <param name="queryParameterName">The query parameter name when generating the link URL.</param>
		/// <param name="format">The format string used to be generate the content string..</param>
		/// <param name="formatProvider">
		///     The format provider used to generate the content string. If this parameter is <c>null</c>,
		///     <see cref="CultureInfo.InvariantCulture" /> will be used.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///     The <paramref name="queryParameterName" /> or <paramref name="format" /> is
		///     <c>null</c>.
		/// </exception>
		public FormattedQueryValueLinkGenerator([NotNull] string queryParameterName, [NotNull] string format,
			IFormatProvider formatProvider = null) : base(queryParameterName)
		{
			Format = format ?? throw new ArgumentNullException(nameof(format));
			FormatProvider = formatProvider ?? CultureInfo.InvariantCulture;
		}

		/// <summary>
		///     Get the format string used to be generate the query parameter value.
		/// </summary>
		[PublicAPI]
		[NotNull]
		public string Format { get; }

		/// <summary>
		///     Get the format provider used to generate the query parameter value.
		/// </summary>
		[PublicAPI]
		[NotNull]
		public IFormatProvider FormatProvider { get; }


		/// <summary>
		///     Generate the query parameter value for the specified <see cref="PagerItem" />.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <returns>The query parameter name for current pager item.</returns>
		public override string GenerateQueryParameterValue(PagerItemGenerationContext context)
		{
			return string.Format(FormatProvider, Format, context.PagerItem.PageNumber);
		}
	}
}