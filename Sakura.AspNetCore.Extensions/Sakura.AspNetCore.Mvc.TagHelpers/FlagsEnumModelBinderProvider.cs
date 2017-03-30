using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	/// <summary>
	///     An <see cref="IModelBinderProvider" /> used to provider <see cref="FlagsEnumModelBinder" /> instances.
	/// </summary>
	public class FlagsEnumModelBinderProvider : IModelBinderProvider
	{
		/// <summary>
		///     Creates a <see cref="IModelBinder" /> based on <see cref="ModelBinderProviderContext" />.
		/// </summary>
		/// <param name="context">The <see cref="ModelBinderProviderContext" />.</param>
		/// <returns>An <see cref="IModelBinder" />.</returns>
		public IModelBinder GetBinder([NotNull] ModelBinderProviderContext context)
		{
			if (context == null)
				throw new ArgumentNullException(nameof(context));

			return context.Metadata.IsFlagsEnum ? new FlagsEnumModelBinder() : null;
		}
	}
}