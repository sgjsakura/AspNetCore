using System;
using System.Globalization;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;
using Sakura.AspNetCore.Mvc.Generators;
using Sakura.AspNetCore.Mvc.Internal;

namespace Sakura.AspNetCore.Mvc
{
	/// <summary>
	///     Provide shortcut generator implementation for <see cref="PagerItemOptions.Content" />. This class is static.
	/// </summary>
	[PublicAPI]
	public static class PagerItemContentGenerators
	{
		#region From Configuration

		/// <summary>
		///     Generate a <see cref="IPagerItemContentGenerator" /> according to the specified configuration text.
		/// </summary>
		/// <param name="configurationText">The configuration text which described the type and arguments of the generator.</param>
		/// <returns>
		///     The <see cref="IPagerItemContentGenerator" /> object generated accoding to
		///     <paramref name="configurationText" />.
		/// </returns>
		/// <exception cref="ArgumentNullException">The <paramref name="configurationText" /> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">The <paramref name="configurationText" /> cannot be parsed into excepted format.</exception>
		/// <exception cref="NotSupportedException">
		///     The generator type specified in <paramref name="configurationText" /> is not
		///     supported.
		/// </exception>
		public static IPagerItemContentGenerator FromConfiguration(string configurationText)
		{
			if (configurationText == null)
				throw new ArgumentNullException(nameof(configurationText));

			var pattern = @"^(?<type>.*?)(:(?<exp>.*)?)$";

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
				case "text":
					return Text(exp);
				case "html":
					return Html(exp);
				case "textformat":
					return TextFormat(exp);
				case "htmlformat":
					return HtmlFormat(exp);
				case "default":
					return Default;
				default:
					throw new NotSupportedException(
						$"The generator type string '{type}' in the configuration text is not a valid option.");
			}
		}

		#endregion

		#region Shortcuts

		/// <summary>
		///     Get the generator that use page number as the content of the pager item.
		/// </summary>
		public static IPagerItemContentGenerator Default { get; } = TextFormat("{0:d}");

		/// <summary>
		///     Create a pager item content generator that generates a fixed text content.
		/// </summary>
		/// <param name="text">The generated text content.</param>
		/// <returns>The pager item content generator object.</returns>
		public static IPagerItemContentGenerator Text(string text)
		{
			return new SimpleStringContentGenerator(text, true);
		}

		/// <summary>
		///     Create a pager item content generator that generates a fixed html content.
		/// </summary>
		/// <param name="html">The generated html content.</param>
		/// <returns>The pager item content generator object.</returns>
		public static IPagerItemContentGenerator Html(string html)
		{
			return new SimpleStringContentGenerator(html, false);
		}

		/// <summary>
		///     Create a pager item content generator that generates a <see cref="IHtmlContent" /> object.
		/// </summary>
		/// <param name="content">The generated html content.</param>
		/// <returns>The pager item content generator object.</returns>
		/// <returns></returns>
		public static IPagerItemContentGenerator Content(IHtmlContent content)
		{
			return new CustomHtmlContentGenerator(context => content);
		}

		/// <summary>
		///     Create a pager item content generator that generates text content using a format string.
		/// </summary>
		/// <param name="textFormat">The format string used to generate the text.</param>
		/// <param name="formatProvider">
		///     The format provider object. If this parameter is <c>null</c>,
		///     <see cref="CultureInfo.CurrentCulture" /> will be used.
		/// </param>
		/// <returns>The pager item content generator object.</returns>
		public static IPagerItemContentGenerator TextFormat(string textFormat, IFormatProvider formatProvider = null)
		{
			return new FormattedStringContentGenerator(textFormat, true, formatProvider);
		}

		/// <summary>
		///     Create a pager item content generator that generates html content using a format string.
		/// </summary>
		/// <param name="htmlFormat">The format string used to generate the html.</param>
		/// <param name="formatProvider">
		///     The format provider object. If this parameter is <c>null</c>,
		///     <see cref="CultureInfo.CurrentCulture" /> will be used.
		/// </param>
		/// <returns>The pager item content generator object.</returns>
		public static IPagerItemContentGenerator HtmlFormat(string htmlFormat, IFormatProvider formatProvider = null)
		{
			return new FormattedStringContentGenerator(htmlFormat, false, formatProvider);
		}

		#endregion

		#region Custom Text

		/// <summary>
		///     Create a pager item content generator that generates text content using a custom generating method.
		/// </summary>
		/// <param name="textGenerator">The generation method.</param>
		/// <returns>The pager item content generator object.</returns>
		public static IPagerItemContentGenerator CustomText(Func<PagerItemGenerationContext, string> textGenerator)
		{
			return new CustomStringContentGenerator(textGenerator, true);
		}

		/// <summary>
		///     Create a pager item content generator that generates text content using a custom generating method.
		/// </summary>
		/// <param name="textGenerator">The generation method.</param>
		/// <returns>The pager item content generator object.</returns>
		public static IPagerItemContentGenerator CustomText(Func<PagerItem, string> textGenerator)
		{
			return CustomText(context => textGenerator(context.PagerItem));
		}

		/// <summary>
		///     Create a pager item content generator that generates text content using a custom generating method.
		/// </summary>
		/// <param name="textGenerator">The generation method.</param>
		/// <returns>The pager item content generator object.</returns>
		public static IPagerItemContentGenerator CustomText(Func<int, string> textGenerator)
		{
			return CustomText(context => textGenerator(context.PagerItem.PageNumber));
		}

		/// <summary>
		///     Create a pager item content generator that generates text content using a custom generating method.
		/// </summary>
		/// <param name="textGenerator">The generation method.</param>
		/// <returns>The pager item content generator object.</returns>
		public static IPagerItemContentGenerator CustomText(Func<string> textGenerator)
		{
			return CustomText((PagerItemGenerationContext context) => textGenerator());
		}

		#endregion

		#region Custom Html

		/// <summary>
		///     Create a pager item content generator that generates html content using a custom generating method.
		/// </summary>
		/// <param name="htmlGenerator">The generation method.</param>
		/// <returns>The pager item content generator object.</returns>
		public static IPagerItemContentGenerator CustomHtml(Func<PagerItemGenerationContext, string> htmlGenerator)
		{
			return new CustomStringContentGenerator(htmlGenerator, false);
		}

		/// <summary>
		///     Create a pager item content generator that generates html content using a custom generating method.
		/// </summary>
		/// <param name="htmlGenerator">The generation method.</param>
		/// <returns>The pager item content generator object.</returns>
		public static IPagerItemContentGenerator CustomHtml(Func<PagerItem, string> htmlGenerator)
		{
			return CustomHtml(context => htmlGenerator(context.PagerItem));
		}

		/// <summary>
		///     Create a pager item content generator that generates html content using a custom generating method.
		/// </summary>
		/// <param name="htmlGenerator">The generation method.</param>
		/// <returns>The pager item content generator object.</returns>
		public static IPagerItemContentGenerator CustomHtml(Func<int, string> htmlGenerator)
		{
			return CustomHtml(context => htmlGenerator(context.PagerItem.PageNumber));
		}

		/// <summary>
		///     Create a pager item content generator that generates html content using a custom generating method.
		/// </summary>
		/// <param name="htmlGenerator">The generation method.</param>
		/// <returns>The pager item content generator object.</returns>
		public static IPagerItemContentGenerator CustomHtml(Func<string> htmlGenerator)
		{
			return CustomHtml((PagerItemGenerationContext context) => htmlGenerator());
		}

		#endregion

		#region Custom IHtmlContent

		/// <summary>
		///     Create a pager item content generator that generates html content using a custom generating method.
		/// </summary>
		/// <param name="contentGenerator">The generation method.</param>
		/// <returns>The pager item content generator object.</returns>
		public static IPagerItemContentGenerator CustomContent(
			Func<PagerItemGenerationContext, IHtmlContent> contentGenerator)
		{
			return new CustomHtmlContentGenerator(contentGenerator);
		}

		/// <summary>
		///     Create a pager item content generator that generates html content using a custom generating method.
		/// </summary>
		/// <param name="contentGenerator">The generation method.</param>
		/// <returns>The pager item content generator object.</returns>
		public static IPagerItemContentGenerator CustomContent(Func<PagerItem, IHtmlContent> contentGenerator)
		{
			return CustomContent(context => contentGenerator(context.PagerItem));
		}

		/// <summary>
		///     Create a pager item content generator that generates html content using a custom generating method.
		/// </summary>
		/// <param name="contentGenerator">The generation method.</param>
		/// <returns>The pager item content generator object.</returns>
		public static IPagerItemContentGenerator CustomContent(Func<int, IHtmlContent> contentGenerator)
		{
			return CustomContent(context => contentGenerator(context.PagerItem.PageNumber));
		}

		/// <summary>
		///     Create a pager item content generator that generates html content using a custom generating method.
		/// </summary>
		/// <param name="contentGenerator">The generation method.</param>
		/// <returns>The pager item content generator object.</returns>
		public static IPagerItemContentGenerator CustomContent(Func<IHtmlContent> contentGenerator)
		{
			return CustomContent((PagerItemGenerationContext context) => contentGenerator());
		}

		#endregion
	}
}