using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;
using Sakura.AspNetCore.Mvc.TagHelpers;

// ReSharper disable once CheckNamespace

namespace Microsoft.Framework.DependencyInjection;

/// <summary>
///     Provide extension methods to add <see cref="FlagsEnumModelBinder" /> to MVC application.
/// </summary>
[PublicAPI]
public static class FlagsEnumModelBinderServiceCollectionExtensions
{
	/// <summary>
	///     Find the index of the first item which satisify the condition defined in <paramref name="predicate" />. If no item
	///     is found, this method will return -1.
	/// </summary>
	/// <typeparam name="T">The element type of the source list.</typeparam>
	/// <param name="source">The source element list.</param>
	/// <param name="predicate">The delegate method to determine if the element satisify the condition.</param>
	/// Find the index of the first item which satisify the condition defined in
	/// <paramref name="predicate" />
	/// . If no item is found, this method will return -1.
	/// <returns>The index of the first item which satisify the condition defined in <paramref name="predicate" />. </returns>
	private static int FirstIndexOfOrDefault<T>(this IEnumerable<T> source, Func<T, bool> predicate)
	{
		var result = 0;

		foreach (var item in source)
		{
			if (predicate(item))
				return result;

			result++;
		}

		return -1;
	}

	/// <summary>
	/// Find the insert location for <see cref="FlagsEnumModelBinderProvider"/> instance.
	/// </summary>
	/// <param name="modelBinderProviders">The MVC <see cref="IModelBinderProvider"/> list.</param>
	/// <returns></returns>
	private static int FindModelBinderProviderInsertLocation(this IList<IModelBinderProvider> modelBinderProviders)
	{
#if NETSTANDARD1_6 || NET451 || NETCOREAPP2_0
		return modelBinderProviders.FirstIndexOfOrDefault(i => i is SimpleTypeModelBinderProvider);
#else
			var index = modelBinderProviders.FirstIndexOfOrDefault(i => i is FloatingPointTypeModelBinderProvider);
			return index < 0 ? index : index + 1;
#endif
	}

	/// <summary>
	///     Insert the <see cref="FlagsEnumModelBinder" /> in the correct location of a list of model binders.
	/// </summary>
	/// <param name="modelBinderProviders">The list of model binders.</param>
	/// <remarks>
	///     This method will insert <see cref="FlagsEnumModelBinder" /> in MVC model binder chain in order to
	///     replace the default behavior for simple enum values. If no insert location is found, this
	///     method will add the <see cref="FlagsEnumModelBinder" /> to the end of the <paramref name="modelBinderProviders" />.
	/// </remarks>
	public static void InsertFlagsEnumModelBinderProvider(this IList<IModelBinderProvider> modelBinderProviders)
	{
		// Argument Check
		if (modelBinderProviders == null)
			throw new ArgumentNullException(nameof(modelBinderProviders));

		var providerToInsert = new FlagsEnumModelBinderProvider();

		// Find the location of SimpleTypeModelBinder, the FlagsEnumModelBinder must be inserted before it.
		var index = modelBinderProviders.FindModelBinderProviderInsertLocation();

		if (index != -1)
			modelBinderProviders.Insert(index, providerToInsert);
		else
			modelBinderProviders.Add(providerToInsert);
	}

	/// <summary>
	///     Configue the <see cref="MvcOptions" /> to insert the <see cref="FlagsEnumModelBinder" />.
	/// </summary>
	/// <param name="options">The <see cref="MvcOptions" /> object to be configuring.</param>
	/// <returns>The <paramref name="options" /> object.</returns>
	public static MvcOptions AddFlagsEnumModelBinderProvider(this MvcOptions options)
	{
		if (options == null)
			throw new ArgumentNullException(nameof(options));

		options.ModelBinderProviders.InsertFlagsEnumModelBinderProvider();
		return options;
	}

	/// <summary>
	///     Configue the <see cref="IMvcBuilder" /> to insert the <see cref="FlagsEnumModelBinder" />.
	/// </summary>
	/// <param name="builder">The <see cref="IMvcBuilder" /> object to be configuring.</param>
	/// <returns>The <paramref name="builder" /> object.</returns>
	public static IMvcBuilder AddFlagsEnumModelBinderProvider(this IMvcBuilder builder)
	{
		builder.AddMvcOptions(options => AddFlagsEnumModelBinderProvider(options));
		return builder;
	}
}