#if NETCOREAPP3_0
using System;
using System.Diagnostics;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Html;

namespace Sakura.AspNetCore.Mvc
{
	/// <summary>
	///     Provide conversion ability between <see cref="IHtmlContent" /> and raw HTML strings.
	/// </summary>
	public class HtmlContentConverter : JsonConverter<IHtmlContent>
	{
		/// <inheritdoc />
		public override IHtmlContent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var str = JsonSerializer.Deserialize<string>(ref reader);
			return new HtmlString(str);
		}

		/// <inheritdoc />
		public override void Write(Utf8JsonWriter writer, IHtmlContent value, JsonSerializerOptions options)
		{
			using var sw = new StringWriter();
			value.WriteTo(sw, HtmlEncoder.Default);

			JsonSerializer.Serialize(writer, sw.ToString());
		}

		/// <inheritdoc />
		public override bool CanConvert(Type typeToConvert)
		{
			var result = typeof(IHtmlContent).IsAssignableFrom(typeToConvert);
			Debug.WriteLine("result = {0}, Type = {1}", result, typeToConvert);

			return result;
		}
	}

	public class HtmlContentConverterFactory : JsonConverterFactory
	{
		private static HtmlContentConverter Converter { get; } = new();

		/// <inheritdoc />
		public override bool CanConvert(Type typeToConvert)
		{
			return typeof(IHtmlContent).IsAssignableFrom(typeToConvert);
		}

		/// <inheritdoc />
		public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
		{
			return Converter;
		}
	}
}

#endif