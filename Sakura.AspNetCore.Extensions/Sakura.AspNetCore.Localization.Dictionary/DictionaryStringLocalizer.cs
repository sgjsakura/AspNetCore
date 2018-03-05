using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.Localization;

namespace Sakura.AspNetCore.Localization
{
	/// <summary>
	/// Provide localizable resource string from a dictiony.
	/// </summary>
	public class DictionaryStringLocalizer : IStringLocalizer
	{
		public IReadOnlyDictionary<string, string> LocalizedStrings { get; set; }

		/// <inheritdoc />
		public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public IStringLocalizer WithCulture(CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		LocalizedString IStringLocalizer.this[string name]
		{
			get
			{
				var culture = new CultureInfo();
				var found = TryFetchStringCore(LocalizedStrings, )
			}
		}

		/// <inheritdoc />
		LocalizedString IStringLocalizer.this[string name, params object[] arguments]
		{
			get { throw new NotImplementedException(); }
		}

		/// <returns></returns>
		private static bool TryFetchStringCore(IReadOnlyDictionary<string, string> data, CultureInfo culture, out string result)
		{
			do
			{
				if (data.TryGetValue(culture.Name, out var value))
				{
					result = value;
					return true;
				}

				culture = culture.Parent;
			} while (culture.Equals(culture.Parent));

			result = null;
			return false;
		}
	}
}
