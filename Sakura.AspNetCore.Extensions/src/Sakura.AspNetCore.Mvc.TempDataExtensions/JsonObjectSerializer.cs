using System;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Sakura.AspNetCore.Mvc
{
	/// <summary>
	///     Default <see cref="IObjectSerializer" /> using <see cref="JsonConvert" /> Service.
	/// </summary>
	[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
	public class JsonObjectSerializer : IObjectSerializer
	{
		/// <summary>
		///     Deserialize object to the original object.
		/// </summary>
		/// <param name="obj">The string to be deserializing.</param>
		/// <returns>The deserialized object.</returns>
		public object Deserialize(object obj)
		{
			var arr = obj as string[];

			// argument check
			if (arr == null || arr.Length != 2)
				throw new InvalidOperationException();

			var typeName = arr[0];
			var data = arr[1];

			var type = Type.GetType(typeName);
			return JsonConvert.DeserializeObject(data, type);
		}

		/// <summary>
		///     Serialize an object to another object.
		/// </summary>
		/// <param name="obj">The object to be serializing.</param>
		/// <returns>The serialized object.</returns>
		public object Serialize(object obj)
		{
			var typeName = obj.GetType().AssemblyQualifiedName;
			var data = JsonConvert.SerializeObject(obj);

			return new[] {typeName, data};
		}
	}
}