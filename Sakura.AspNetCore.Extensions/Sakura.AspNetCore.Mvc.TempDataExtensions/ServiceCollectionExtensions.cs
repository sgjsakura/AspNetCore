using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sakura.AspNetCore.Mvc;
using System;

#if NETCOREAPP3_0
using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;
#else
using Microsoft.AspNetCore.Mvc.ViewFeatures;
#endif

// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Provide extension methods for service configurations. This class is static.
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	///     Replace the default temp data implementation in order to support complex data storage.
	/// </summary>
	/// <param name="services">The service container to adding the service.</param>
	/// <param name="configureOptions">Additional steps to configure the <see cref="TempDataSerializationOptions" />.</param>
	/// <exception cref="ArgumentNullException"><paramref name="services" /> is <c>null</c>.</exception>
	[PublicAPI]
	public static IServiceCollection AddEnhancedTempData([NotNull] this IServiceCollection services,
		Action<TempDataSerializationOptions> configureOptions = null)
	{
		// Argument check
		if (services == null)
			throw new ArgumentNullException(nameof(services));

		// Add the IObjectSerializer implementation
		services.TryAddSingleton<IObjectSerializer, JsonObjectSerializer>();
		services.AddOptions();

#if NETCOREAPP3_0
		services.Replace(new(typeof(TempDataSerializer),
			typeof(TypedJsonTempDataSerializer), ServiceLifetime.Singleton));
#else
		// Replace default ITempDataProvider
		services.Replace(new(typeof(ITempDataProvider), typeof(EnhancedSessionStateTempDataProvider),
			ServiceLifetime.Singleton));
#endif

		// Config options
		if (configureOptions != null) services.Configure(configureOptions);

		return services;
	}

	/// <summary>
	///     Enable serialization for <see cref="IHtmlContent" /> objects.
	/// </summary>
	/// <param name="options">The <see cref="TempDataSerializationOptions" /> to be configuring.</param>
	/// <returns>The <paramref name="options" /> argument.</returns>
	/// <remarks>
	///     This method use the <see cref="HtmlContentConverter" /> to convert between HTML content and raw HTML strings. This
	///     manner only guarantees an equivalent page rendering result, however, the actual <see cref="IHtmlContent" />
	///     instance maybe changed. Under such circumstance, please do not use this manner if you are trying to modify the
	///     inner structure for any <see cref="IHtmlContent" /> instance between actions.
	/// </remarks>
	[PublicAPI]
	public static TempDataSerializationOptions EnableHtmlContentSerialization(this TempDataSerializationOptions options)
	{
		// Add converter for IHtmlContent.
		options.Converters.Add(new HtmlContentConverter());
		return options;
	}
}