using System.Collections.Generic;
using System.Collections.ObjectModel;

#if NETCOREAPP3_0
using JsonConverter = System.Text.Json.Serialization.JsonConverter;
#else
using JsonConverter = Newtonsoft.Json.JsonConverter;
#endif

namespace Sakura.AspNetCore.Mvc;

/// <summary>
///     Provide optional configuration data for temp data serialization process.
/// </summary>
public class TempDataSerializationOptions
{
	/// <summary>
	///     A collection of <see cref="JsonConverter" /> should be used during the JSON serialization.
	/// </summary>
	public IList<JsonConverter> Converters { get; } = new Collection<JsonConverter>();
}