using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Sakura.AspNetCore
{
	/// <summary>
	///     Provide the configuration options for operation message services.
	/// </summary>
	public class OperationMessageOptions
	{
		/// <summary>
		///     Define the default value for the <see cref="TempDataKeyName" /> property. This field is constant.
		/// </summary>
		[PublicAPI] public const string DefaultTempDataKeyName = "ASP_OperationMessages";

		/// <summary>
		///     Get or set the key used to store the message collections in TempData dictionary. This key must not be used in any
		///     other manner. The default value of this property is <see cref="DefaultTempDataKeyName" />.
		/// </summary>
		[PublicAPI]
		public string TempDataKeyName { get; set; } = DefaultTempDataKeyName;

		/// <summary>
		///     Get of set a value that control the behavior when there is an value stored associated with the key defined as
		///     <see cref="TempDataKeyName" /> but it cannot be converted into <see cref="ICollection{T}" /> type. If the property
		///     is set to <c>true</c>, the original value will be automatically discarded and replaced with a new instance of
		///     <see cref="ICollection{OperationMessage}" /> type. Otherwise, <see cref="InvalidOperationException" /> will be
		///     throwed.
		/// </summary>
		[PublicAPI]
		public bool AutomaticOverwriteOnTypeError { get; set; }
	}
}