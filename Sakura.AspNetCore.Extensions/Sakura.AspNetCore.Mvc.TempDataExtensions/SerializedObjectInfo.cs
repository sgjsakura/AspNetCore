using System;
using System.Collections.Generic;
using System.Text;

namespace Sakura.AspNetCore.Mvc
{
	/// <summary>
	/// Represents as the information for a serialized object.
	/// </summary>
	public class SerializedObjectInfo
	{
		/// <summary>
		/// The type name of the object.
		/// </summary>
		public string TypeName { get; set; }

		/// <summary>
		/// The value text of the object.
		/// </summary>
		public string Value { get; set; }
	}
}
