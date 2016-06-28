using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Sakura.AspNetCore.Mvc.Internal
{
	/// <summary>
	///     Provide helper methods to convert string value to <see cref="IHtmlContent" /> object. This class is static.
	/// </summary>
	public static class StringToHtmlContentConverter
	{
		/// <summary>
		///     Convert a string value to <see cref="IHtmlContent" /> object.
		/// </summary>
		/// <param name="value">The value of the string.</param>
		/// <param name="encodeContent">
		///     Control whether the <paramref name="value" /> should be HTML-encoded before be written to a
		///     page.
		/// </param>
		/// <returns>The converted <see cref="IHtmlContent" /> object.</returns>
		public static IHtmlContent ToHtmlContent(this string value, bool encodeContent)
		{
			return encodeContent ? (IHtmlContent) new StringHtmlContent(value) : new HtmlString(value);
		}
	}
}