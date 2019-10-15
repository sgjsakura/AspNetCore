#if NETCOREAPP3_0

using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;

namespace Sakura.AspNetCore.Mvc
{
	/// <summary>
	/// Enhanced temp data serializer used in ASP.NET Core 3.0 Apps.
	/// </summary>
	/// <inheritdoc />
	public class TypedJsonTempDataSerializer : TempDataSerializer
	{
		/// <summary>
		/// Initialize a new instance of <see cref="TypedJsonTempDataSerializer"/>.
		/// </summary>
		/// <param name="objectSerializer">The internal <see cref="IObjectSerializer"/> service instance.</param>
		public TypedJsonTempDataSerializer(IObjectSerializer objectSerializer)
		{
			ObjectSerializer = objectSerializer;
		}

		/// <summary>
		/// Internal helper used to serialize between object and string.
		/// </summary>
		IObjectSerializer ObjectSerializer { get; }

		/// <inheritdoc />
		public override IDictionary<string, object> Deserialize(byte[] unprotectedData)
		{
			var dic = JsonSerializer.Deserialize<Dictionary<string, object>>(unprotectedData);

			var result = new Dictionary<string, object>();

			foreach (var (key, value) in dic)
			{
				result.Add(key, ObjectSerializer.Deserialize(value));
			}

			return result;
		}

		/// <inheritdoc />
		public override byte[] Serialize(IDictionary<string, object> values)
		{
			if (values == null || values.Count == 0)
			{
				return Array.Empty<byte>();
			}

			var realDictionary = new Dictionary<string, object>();

			foreach (var (key, value) in values)
			{
				realDictionary.Add(key, ObjectSerializer.Serialize(value));
			}

			return JsonSerializer.SerializeToUtf8Bytes(realDictionary);
		}
	}
}

#endif