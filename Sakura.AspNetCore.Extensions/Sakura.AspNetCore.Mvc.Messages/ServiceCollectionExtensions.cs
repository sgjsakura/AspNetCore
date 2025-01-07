using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sakura.AspNetCore;
using Sakura.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace

namespace Microsoft.Framework.DependencyInjection;

/// <summary>
///     Provide extension methods for service injection. This class is static.
/// </summary>
[PublicAPI]
public static class ServiceCollectionExtensions
{
	/// <summary>
	///     Add operation messages UI generation services.
	/// </summary>
	/// <param name="serviceCollection">The <see cref="IServiceCollection" /> object.</param>
	public static void AddOperationMessageUIGenerators(this IServiceCollection serviceCollection)
	{
		serviceCollection
			.TryAddSingleton<IOperationMessageLevelClassMapper, DefaultOperationMessageLevelClassMapper>();
		serviceCollection.TryAddScoped<IOperationMessageHtmlGenerator, DefaultOperationMessageHtmlGenerator>();
	}

	/// <summary>
	///     Add operation messages and all related services.
	/// </summary>
	/// <param name="serviceCollection">The <see cref="IServiceCollection" /> object.</param>
	/// <param name="setupAction">Optional setup actions.</param>
	public static void AddOperationMessages([NotNull] this IServiceCollection serviceCollection,
		Action<OperationMessageOptions> setupAction = null)
	{
		serviceCollection.AddOperationMessageAccessor(setupAction);
		serviceCollection.AddOperationMessageUIGenerators();
	}
}