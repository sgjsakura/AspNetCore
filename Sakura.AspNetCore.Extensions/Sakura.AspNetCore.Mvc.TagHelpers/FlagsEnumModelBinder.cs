using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	/// <summary>
	///     Support binding a flags enum value with multiple flag inputs.
	/// </summary>
	public class FlagsEnumModelBinder : IModelBinder
	{
		/// <summary>Attempts to bind a model.</summary>
		/// <param name="bindingContext">The <see cref="ModelBindingContext" />.</param>
		/// <returns>
		///     <para>
		///         A <see cref="System.Threading.Tasks.Task" /> which will complete when the model binding process completes.
		///     </para>
		///     <para>
		///         If model binding was successful, the <see cref="ModelBindingContext.Result" /> should have
		///         <see cref="ModelBindingResult.IsModelSet" /> set to <c>true</c>.
		///     </para>
		///     <para>
		///         A model binder that completes successfully should set <see cref="ModelBindingContext.Result" /> to
		///         a value returned from <see cref="ModelBindingResult.Success(object)" />.
		///     </para>
		/// </returns>
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			// Only accept enum values
			if (!bindingContext.ModelMetadata.IsFlagsEnum)
				return TaskCache.CompletedTask;

			var provideValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

			// Do nothing if there is no actual values
			if (provideValue == ValueProviderResult.None)
				return TaskCache.CompletedTask;

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
			bindingContext.Result = ModelBindingResult.Success(realResult);

			return TaskCache.CompletedTask;
		}
	}
}