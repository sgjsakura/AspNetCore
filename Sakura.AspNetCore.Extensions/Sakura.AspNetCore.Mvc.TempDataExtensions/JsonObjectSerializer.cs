using System;

using JetBrains.Annotations;

using Microsoft.Extensions.Options;

#if NETCOREAPP3_0
using System.Text.Json;
#else
using Newtonsoft.Json;
#endif

namespace Sakura.AspNetCore.Mvc;

/// <summary>
///     Default <see cref="IObjectSerializer" /> using ASP.NET Core in-built JSON serialization Service.
/// </summary>
[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
public class JsonObjectSerializer : IObjectSerializer
{
	/// <summary>
	///     Initialize a new instance of <see cref="JsonObjectSerializer" /> service.
	/// </summary>
	/// <param name="options">The configuration options value.</param>
	public JsonObjectSerializer(IOptions<TempDataSerializationOptions> options)
	{
		Options = options.Value;

#if NETCOREAPP3_0
		JsonSerializerOptions = GenerateOptions();
#else
		JsonSerializerSettings = GenerateSettings();
#endif
	}

	/// <summary>
	///     The configuration options instance.
	/// </summary>
	private TempDataSerializationOptions Options { get; }

	/// <summary>
	///     Deserialize object to the original object.
	/// </summary>
	/// <param name="obj">The string to be deserializing.</param>
	/// <returns>The deserialized object.</returns>
	public object Deserialize(string obj)
	{
		if (string.IsNullOrEmpty(obj)) return null;


#if NETCOREAPP3_0
		var objInfo = JsonSerializer.Deserialize<SerializedObjectInfo>(obj);
#else
		var objInfo = JsonConvert.DeserializeObject<SerializedObjectInfo>(obj);
#endif

		var type = Type.GetType(objInfo.TypeName);

#if NETCOREAPP3_0
		return JsonSerializer.Deserialize(objInfo.Value, type, JsonSerializerOptions);
#else
		return JsonConvert.DeserializeObject(objInfo.Value, type, JsonSerializerSettings);
#endif
	}

	/// <summary>
	///     Serialize an object to another object.
	/// </summary>
	/// <param name="obj">The object to be serializing.</param>
	/// <returns>The serialized object.</returns>
	public string Serialize(object obj)
	{
		if (obj == null) return string.Empty;

		var typeName = obj.GetType().AssemblyQualifiedName;


#if NETCOREAPP3_0
		return JsonSerializer.Serialize(new SerializedObjectInfo
		{
			TypeName = typeName,
			Value = JsonSerializer.Serialize(obj, JsonSerializerOptions)
		});
#else
		return JsonConvert.SerializeObject(new SerializedObjectInfo
		{
			TypeName = typeName,
			Value = JsonConvert.SerializeObject(obj, JsonSerializerSettings)
		});
#endif
	}


#if NETCOREAPP3_0
	/// <summary>
	///     The <see cref="System.Text.Json.JsonSerializerOptions" /> instance used to controlling the JSON serialization
	///     process.
	/// </summary>
	private JsonSerializerOptions JsonSerializerOptions { get; }

	/// <summary>
	///     Generate the <see cref="JsonSerializerOptions" /> value.
	/// </summary>
	/// <returns>The generated <see cref="System.Text.Json.JsonSerializerOptions" /> instance.</returns>
	private JsonSerializerOptions GenerateOptions()
	{
		var result = new JsonSerializerOptions();

		// Add additional converters.
		if (Options.Converters != null)
			foreach (var item in Options.Converters)
				result.Converters.Add(item);

		return result;
	}
#else

	/// <summary>
	///     The <see cref="Newtonsoft.Json.JsonSerializerSettings" /> instance used to controlling the JSON serialization
	///     process.
	/// </summary>
	private JsonSerializerSettings JsonSerializerSettings { get; }

	/// <summary>
	///     Generate the <see cref="Newtonsoft.Json.JsonSerializerSettings" /> value.
	/// </summary>
	/// <returns>The generated <see cref="System.Text.Json.JsonSerializerOptions" /> instance.</returns>
	private JsonSerializerSettings GenerateSettings()
	{
		var result = new JsonSerializerSettings();

		if (Options.Converters != null)
		{
			foreach (var item in Options.Converters)
			{
				result.Converters.Add(item);
			}
		}

		return result;
	}

#endif
}