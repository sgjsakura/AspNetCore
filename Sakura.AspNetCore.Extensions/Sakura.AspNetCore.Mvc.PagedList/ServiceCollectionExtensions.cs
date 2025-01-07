using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sakura.AspNetCore.Mvc.Internal;

namespace Sakura.AspNetCore.Mvc;

/// <summary>
///     Add support for dependency injections. This class is static.
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	///     Add necessary services to generate bootstrap styled pager.
	/// </summary>
	/// <param name="services">Service collections.</param>
	public static void AddBootstrapPagerGenerator(this IServiceCollection services)
	{
		// Depended services
		services.TryAddTransient<IPagerListGenerator, DefaultPagerListGenerator>();
		services.TryAddTransient<IPagerRenderingListGenerator, DefaultPagerRenderingListGenerator>();
		services.TryAddTransient<IPagerHtmlGenerator, BootstrapPagerHtmlGenerator>();

		// Main service
		services.TryAddTransient<IPagerGenerator, DefaultPagerGenerator>();
	}


	/// <summary>
	///     Add necessary services to generate bootstrap styled pager and configure additional options.
	/// </summary>
	/// <param name="services">Service collections.</param>
	/// <param name="setupAction">Additional setup action method.</param>
	public static void AddBootstrapPagerGenerator(this IServiceCollection services,
		Action<PagerOptions> setupAction)
	{
		// Depended services
		services.AddBootstrapPagerGenerator();


		// Configuration
		if (setupAction != null)
			services.Configure(setupAction);
	}
}