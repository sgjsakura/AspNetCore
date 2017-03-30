using System;
using System.Globalization;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Sakura.AspNetCore.Mvc.Generators;
using Sakura.AspNetCore.Mvc.Internal;

namespace Sakura.AspNetCore.Mvc
{
	/// <summary>
	///     Provide shortcut generator implementation for <see cref="PagerItemOptions.Link" />. This class is static.
	/// </summary>
	[PublicAPI]
	public static class PagerItemLinkGenerators
	{
		#region Congfiguration

		/// <summary>
		///     Generate a <see cref="IPagerItemLinkGenerator" /> from a configuration string.
		/// </summary>
		/// <param name="configurationText">The configuration string.</param>
		/// <returns>The converted <see cref="IPagerItemLinkGenerator" />.</returns>
		public static IPagerItemLinkGenerator FromConfiguration(string configurationText)
		{
			if (configurationText == null)
				throw new ArgumentNullException(nameof(configurationText));

			if (configurationText == null)
				throw new ArgumentNullException(nameof(configurationText));

			const string pattern = @"^(?<type>.*?)(:(?<exp>.*))?$";

			var matchResult = Regex.Match(configurationText, pattern,
				RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase |
				RegexOptions.Singleline);

			if (!matchResult.Success)
				throw new ArgumentException("The configurationText cannot be parsed.", nameof(configurationText));

			// Type text, normalize first
			var type = matchResult.Groups["type"].Value.Trim().ToLowerInvariant();
			var exp = matchResult.Groups["exp"].Value;

			switch (type)
			{
				case "query":
					const string p2 = @"^(?<name>.*?)=(?<value>.*)$";
					var m2 = Regex.Match(
						exp,
						p2,
						RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase
						| RegexOptions.Singleline);
					if (!m2.Success)
						throw new NotSupportedException(
							$"The generator expression string '{exp}' in the configuration text cannot be condiser as a query expression.");
					return QueryNameAndValueFormat(m2.Groups["name"].Value, m2.Groups["value"].Value);
				case "queryname":
					return QueryName(exp);
				case "format":
					return Format(exp);
				case "fixed":
					return Fixed(exp);
				case "default":
					return Default;
				case "disabled":
					return Disabled;
				default:
					throw new NotSupportedException(
						$"The generator type string '{type}' in the configuration text is not a valid option.");
			}
		}

		#endregion

		#region Shortcut

		/// <summary>
		///     Get the generator that uses a fixed string as a query parameter name and a formatted string as a query parameter
		///     value.
		/// </summary>
		/// <param name="queryParameterName">The name of the query parameter.</param>
		/// <param name="queryParameterValueFormat">The format string of the query parameter value.</param>
		/// <param name="queryParameterValueFormatProvider">The format provider for format value generation.</param>
		/// <returns>The generated <see cref="FormattedQueryValueLinkGenerator" /> instance.</returns>
		public static FormattedQueryValueLinkGenerator QueryNameAndValueFormat([NotNull] string queryParameterName,
			[NotNull] string queryParameterValueFormat, IFormatProvider queryParameterValueFormatProvider = null)
		{
			return new FormattedQueryValueLinkGenerator(queryParameterName, queryParameterValueFormat,
				queryParameterValueFormatProvider);
		}

		/// <summary>
		///     Get the generator that uses a fixed string as a query parameter name and a page number as a query parameter value.
		/// </summary>
		/// <param name="queryParameterName">The name of the query parameter.</param>
		/// <returns>The generated <see cref="FormattedQueryValueLinkGenerator" /> instance.</returns>
		public static FormattedQueryValueLinkGenerator QueryName([NotNull] string queryParameterName)
		{
			return QueryNameAndValueFormat(queryParameterName, "{0:d}");
		}

		/// <summary>
		///     Get the generator that uses a "page" as a query parameter name and a page number as a query parameter value.
		/// </summary>
		/// <returns>The generated <see cref="FormattedQueryValueLinkGenerator" /> instance.</returns>
		public static FormattedQueryValueLinkGenerator Default { get; } = QueryName("page");

		/// <summary>
		///     Get the generator that disable the link generation feature.
		/// </summary>
		public static DisabledLinkGenerator Disabled { get; } = new DisabledLinkGenerator();

		/// <summary>
		///     Create a pager item link generator that generates link using a format string.
		/// </summary>
		/// <param name="format">The format string used to generate the link.</param>
		/// <param name="formatProvider">
		///     The format provider object. If this parameter is <c>null</c>,
		///     <see cref="CultureInfo.InvariantCulture" /> will be used.
		/// </param>
		/// <returns>The pager item link generator object.</returns>
		public static FormattedLinkGenerator Format([NotNull] string format, IFormatProvider formatProvider = null)
		{
			return new FormattedLinkGenerator(format, formatProvider);
		}

		/// <summary>
		///     Create a pager item link generator that generates link using a fixed string.
		/// </summary>
		/// <param name="text">The link url string.</param>
		/// <returns>The pager item link generator object.</returns>
		public static SimpleLinkGenerator Fixed([NotNull] string text)
		{
			return new SimpleLinkGenerator(text);
		}

		/// <summary>
		///     Create a pager item link generator that generates link using a custom generating method.
		/// </summary>
		/// <param name="linkGenerator">The generation method.</param>
		/// <returns>The pager item link generator object.</returns>
		public static CustomLinkGenerator Custom([NotNull] Func<PagerItemGenerationContext, string> linkGenerator)
		{
			return new CustomLinkGenerator(linkGenerator);
		}

		/// <summary>
		///     Create a pager item link generator that generates link using a custom generating method.
		/// </summary>
		/// <param name="linkGenerator">The generation method.</param>
		/// <returns>The pager item link generator object.</returns>
		public static CustomLinkGenerator Custom([NotNull] Func<PagerItem, string> linkGenerator)
		{
			return Custom(context => linkGenerator(context.PagerItem));
		}

		/// <summary>
		///     Create a pager item link generator that generates link using a custom generating method.
		/// </summary>
		/// <param name="linkGenerator">The generation method.</param>
		/// <returns>The pager item link generator object.</returns>
		public static CustomLinkGenerator Custom([NotNull] Func<int, string> linkGenerator)
		{
			return Custom(context => linkGenerator(context.PagerItem.PageNumber));
		}

		#endregion
	}
}