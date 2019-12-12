using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sakura.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	///     Provide extension methods for service configurations. This class is static.
	/// </summary>
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		///     Replace the default temp data implementation in order to support complex data storage.
		/// </summary>
		/// <param name="services">The service container to adding the service.</param>
		/// <exception cref="ArgumentNullException"><paramref name="services" /> is <c>null</c>.</exception>
		[PublicAPI]
		public static IServiceCollection AddEnhancedTempData([NotNull] this IServiceCollection services)
		{
			// Argument check
			if (services == null)
				throw new ArgumentNullException(nameof(services));

			// Add the IObjectSerializer implementation
			services.TryAddSingleton<IObjectSerializer, JsonObjectSerializer>();

#if NETCOREAPP3_0
			services.Replace(new ServiceDescriptor(typeof(Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure.TempDataSerializer),
				typeof(TypedJsonTempDataSerializer), ServiceLifetime.Singleton));
#else
			// Replace default ITempDataProvider
			services.Replace(new ServiceDescriptor(typeof(ITempDataProvider), typeof(EnhancedSessionStateTempDataProvider),
				ServiceLifetime.Singleton));
#endif
			return services;
		}
	}
}