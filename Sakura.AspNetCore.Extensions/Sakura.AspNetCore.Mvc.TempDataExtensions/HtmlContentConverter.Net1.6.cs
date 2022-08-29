#if !NETCOREAPP3_0
using System;
using System.IO;
using System.Reflection;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;

namespace Sakura.AspNetCore.Mvc;

/// <summary>
///     Provide conversion ability between <see cref="IHtmlContent" /> and raw HTML strings.
/// </summary>
public class HtmlContentConverter : JsonConverter
{
	/// <inheritdoc />
	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		var realValue = value as IHtmlContent;

		if (realValue == null)
		{
			writer.WriteNull();
			return;
		}

		using var sw = new StringWriter();
		realValue.WriteTo(sw, HtmlEncoder.Default);
		writer.WriteValue(sw.ToString());
		;
	}

	/// <inheritdoc />
	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		var str = existingValue as string;

		if (str == null) return null;

		var value = new HtmlString(str);
		return value;
	}

	/// <inheritdoc />
	public override bool CanConvert(Type objectType)
	{
		return typeof(IHtmlContent).IsAssignableFrom(objectType);
	}
}
#endif