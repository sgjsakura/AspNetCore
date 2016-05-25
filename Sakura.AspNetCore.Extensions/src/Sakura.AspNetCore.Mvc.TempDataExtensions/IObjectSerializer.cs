namespace Sakura.AspNetCore.Mvc
{
	/// <summary>
	///     Define the serialization service for arbitrary object.
	/// </summary>
	public interface IObjectSerializer
	{
		/// <summary>
		///     Serialize an object to another object.
		/// </summary>
		/// <param name="obj">The object to be serializing.</param>
		/// <returns>The serialized object.</returns>
		object Serialize(object obj);

		/// <summary>
		///     Deserialize object to the original object.
		/// </summary>
		/// <param name="obj">The string to be deserializing.</param>
		/// <returns>The deserialized object.</returns>
		object Deserialize(object obj);
	}
}