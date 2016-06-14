using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	/// <summary>
	///     Support binding a flags enum value with multiple flag inputs.
	/// </summary>
	public class FlagsEnumModelBinder : IModelBinder
	{
		/// <summary>
		///     Async function to bind to a particular model.
		/// </summary>
		/// <param name="bindingContext">The binding context which has the object to be bound.</param>
		/// <returns>
		///     A Task which on completion returns a <see cref="T:Microsoft.AspNet.Mvc.ModelBinding.ModelBindingResult" /> which
		///     represents the result
		///     of the model binding process.
		/// </returns>
		/// <remarks>
		///     A <c>null</c> return value means that this model binder was not able to handle the request.
		///     Returning <c>null</c> ensures that subsequent model binders are run. If a non <c>null</c> value indicates
		///     that the model binder was able to handle the request.
		/// </remarks>
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			// Only accept enum values
			if (!bindingContext.ModelMetadata.IsFlagsEnum)
			{
				bindingContext.Result = null;
			}
			else
			{
				var provideValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

				// Do nothing if there is no actual values
				if (provideValue == ValueProviderResult.None)
				{
					bindingContext.Result = null;
				}
				else
				{
					// Get the real enum type
					var enumType = bindingContext.ModelType;
					enumType = Nullable.GetUnderlyingType(enumType) ?? enumType;

					// Each value self may contains a series of actual values, split it with comma
					var strs = provideValue.Values.SelectMany(s => s.Split(','));

					// Convert all items into enum items.
					var actualValues = strs.Select(valueString => Enum.Parse(enumType, valueString));

					// Merge to final result
					var result = actualValues.Aggregate(0, (current, value) => current | (int) value);

					// Convert to Enum object
					var realResult = Enum.ToObject(enumType, result);

					// Result
					bindingContext.Result = ModelBindingResult.Success(bindingContext.ModelName, realResult);
				}
			}

			return Task.FromResult(0);
		}
	}
}