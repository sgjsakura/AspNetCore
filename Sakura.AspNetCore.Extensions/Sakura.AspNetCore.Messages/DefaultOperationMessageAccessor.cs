using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;

namespace Sakura.AspNetCore
{
	/// <summary>
	///     Provide the default implementation for <see cref="IOperationMessageAccessor" /> service.
	/// </summary>
	public class DefaultOperationMessageAccessor : IOperationMessageAccessor
	{
		/// <summary>
		///     Initialize a new service instance with required services.
		/// </summary>
		/// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor" /> service.</param>
		/// <param name="tempDataFactory">The TempData dictionary factory in the context.</param>
		/// <param name="options">The <see cref="OperationMessageOptions" /> instance used to configure the accessor.</param>
		public DefaultOperationMessageAccessor(IHttpContextAccessor httpContextAccessor,
			ITempDataDictionaryFactory tempDataFactory, IOptions<OperationMessageOptions> options)
		{
			Options = options.Value;
			TempData = tempDataFactory.GetTempData(httpContextAccessor.HttpContext);
		}


		/// <summary>
		///     Get the ASP.NET TempData dictionary in the context.
		/// </summary>
		[PublicAPI]
		protected ITempDataDictionary TempData { get; }

		/// <summary>
		///     Get the options for this accessor.
		/// </summary>
		[PublicAPI]
		protected OperationMessageOptions Options { get; }

		/// <summary>
		///     Get the collection to store all messages in the current execution context.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		///     The configured TempDataKeyName is <c>null</c>; Or, the temp data dictionary
		///     already contains an value associated with the key used for operation message, however the value cannot be converted
		///     into <see cref="ICollection{T}" /> type.
		/// </exception>
		public ICollection<OperationMessage> Messages
		{
			get
			{
				// Retrieve and check the key
				var key = Options.TempDataKeyName;

				if (key == null)
					throw new InvalidOperationException(
						"The TempDataKeyName property in the configured OperationMessageOptions instance cannot be null。");

				ICollection<OperationMessage> result = null;

				// Retrieve the data and convert to the collection type
				if (TempData.TryGetValue(key, out var value))
				{
					result = value as ICollection<OperationMessage>;

					// Check type and raise exception
					if (result == null && !Options.AutomaticOverwriteOnTypeError)
						throw new InvalidOperationException(
							$"TempData has an value with the key name '{key}', however it cannot be converted to ICollection<OperationMessage> type.");
				}

				// Automatically create a new collection when the valuect is not existed or cannot be converted to the target type
				if (result == null)
				{
					result = new List<OperationMessage>();
					TempData[key] = result;
				}

				return result;
			}
		}
	}
}