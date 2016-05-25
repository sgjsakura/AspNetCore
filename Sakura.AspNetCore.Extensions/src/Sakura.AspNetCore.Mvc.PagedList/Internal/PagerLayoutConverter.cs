using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sakura.AspNetCore.Mvc.Internal
{
	/// <summary>
	///     Convert a custom string to <see cref="PagerOptions.Layout" /> property value.
	/// </summary>
	public class PagerLayoutConverter : TypeConverter
	{
		/// <inheritdoc />
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		/// <inheritdoc />
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			var realValue = value as string;
			if (realValue == null)
			{
				throw new NotSupportedException();
			}

			var match = Regex.Match(realValue, @"^(?<type>.*?)(\:(?<exp>.*))?$",
				RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase | RegexOptions.Singleline);

			if (!match.Success)
			{
				throw new NotSupportedException();
			}

			var type = match.Groups["type"].Value;
			var exp = match.Groups["exp"].Value;

			switch (type.ToLowerInvariant())
			{
				case "default":
					return PagerLayouts.Default;
				case "custom":
					var items = exp.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
					return
						new PagerLayout(items.Select(i => (PagerLayoutElement) Enum.Parse(typeof(PagerLayoutElement), i.Trim(), true)));

				default:
					throw new NotSupportedException();
			}
		}
	}
}