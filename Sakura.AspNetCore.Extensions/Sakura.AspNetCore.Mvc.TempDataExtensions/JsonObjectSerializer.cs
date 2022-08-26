using System;
using System.IO;
using System.Text.Encodings.Web;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Html;

#if NETCOREAPP3_0
using System.Text.Json;
#else
using Newtonsoft.Json;
#endif

namespace Sakura.AspNetCore.Mvc
{
	/// <summary>
	///     Default <see cref="IObjectSerializer" /> using ASP.NET Core in-built JSON serialization Service.
	/// </summary>
	[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
	public class JsonObjectSerializer : IObjectSerializer
	{
		/// <summary>
		///     Deserialize object to the original object.
		/// </summary>
		/// <param name="obj">The string to be deserializing.</param>
		/// <returns>The deserialized object.</returns>
		public object Deserialize(string obj)
		{

			if (string.IsNullOrEmpty(obj))
			{
				return null;
			}


#if NETCOREAPP3_0
			var objInfo = JsonSerializer.Deserialize<SerializedObjectInfo>(obj);
#else
			var objInfo = JsonConvert.DeserializeObject<SerializedObjectInfo>(obj);
#endif

			var type = Type.GetType(objInfo.TypeName);

			if (type == typeof(IHtmlContent))
			{
				return new HtmlString(objInfo.Value);
			}

#if NETCOREAPP3_0
			return JsonSerializer.Deserialize(objInfo.Value, type);
#else
			return JsonConvert.DeserializeObject(objInfo.Value, type);
#endif

		}

		/// <summary>
		///     Serialize an object to another object.
		/// </summary>
		/// <param name="obj">The object to be serializing.</param>
		/// <returns>The serialized object.</returns>
		public string Serialize(object obj)
		{
			if (obj == null)
			{
				return string.Empty;
			}

			var typeName = obj.GetType().AssemblyQualifiedName;

			// Special handling for HTML Content.
			if (obj is IHtmlContent htmlContent)
			{
				var builder = new HtmlContentBuilder();
				// ReSharper disable once MustUseReturnValue
				builder.AppendHtml(htmlContent);

				using var sb = new StringWriter();
				builder.WriteTo(sb, HtmlEncoder.Default);

				typeName = typeof(IHtmlContent).AssemblyQualifiedName;
				obj = sb.ToString();
				;
			}

#if NETCOREAPP3_0
			return JsonSerializer.Serialize(new SerializedObjectInfo
			{
				TypeName = typeName,
				Value = JsonSerializer.Serialize(obj)
			});
#else
			return JsonConvert.SerializeObject(new SerializedObjectInfo
			{
				TypeName = typeName,
				Value = JsonConvert.SerializeObject(obj)
			});
#endif
		}
	}
}