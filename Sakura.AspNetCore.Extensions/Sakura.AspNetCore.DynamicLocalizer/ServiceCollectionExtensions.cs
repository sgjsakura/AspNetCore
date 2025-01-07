using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sakura.AspNetCore.Localization.Internal;

namespace Sakura.AspNetCore.Localization;

/// <summary>
///     Provide extension methods to add services. This class is static.
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	///     Add default implementation for all dynamic resource accessing services.
	/// </summary>
	/// <param name="services">The service collection container.</param>
	public static void AddDynamicLocalizer([NotNull] this IServiceCollection services)
	{
		if (services == null)
			throw new ArgumentNullException(nameof(services));

		services.TryAddTransient<IDynamicViewLocalizer, DynamicViewLocalizer>();
		services.TryAddTransient(typeof(IDynamicStringLocalizer<>), typeof(DynamicStringLocalizer<>));
		services.TryAddTransient(typeof(IDynamicHtmlLocalizer<>), typeof(DynamicHtmlLocalizer<>));
	}
}