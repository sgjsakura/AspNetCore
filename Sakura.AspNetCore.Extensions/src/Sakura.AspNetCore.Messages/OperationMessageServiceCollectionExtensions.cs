using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sakura.AspNetCore;

// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	///     Provide extension method for add or configure the operation message service. This class is static.
	/// </summary>
	[PublicAPI]
	public static class OperatonMessageServiceCollectionExtensions
	{
		/// <summary>
		///     Add the operation message services to the specified service container.
		/// </summary>
		/// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
		/// <param name="configureOptions">A optional action for configure this service.</param>
		/// <returns>The <paramref name="services" /> parameter.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="services" /> is <c>null</c>.</exception>
		public static IServiceCollection AddOperationMessageAccessor([NotNull] this IServiceCollection services,
			[CanBeNull] Action<OperationMessageOptions> configureOptions = null)
		{
			// Check argument
			if (services == null)
			{
				throw new ArgumentNullException(nameof(services));
			}

			// Try add service
			services.TryAddScoped<IOperationMessageAccessor, DefaultOperationMessageAccessor>();

			// Configure the service
			if (configureOptions != null)
			{
				services.Configure(configureOptions);
			}


			return services;
		}

		/// <summary>
		///     Configure the options for <see cref="IOperationMessageAccessor" /> service.
		/// </summary>
		/// <param name="services">The service container to store the service configuration.</param>
		/// <param name="configureOptions">A action for configuring the service.</param>
		/// <returns>The <paramref name="services" /> parameter.</returns>
		/// <exception cref="ArgumentNullException">
		///     Either <paramref name="services" /> or <paramref name="configureOptions" /> is
		///     <c>null</c>.
		/// </exception>
		public static IServiceCollection ConfigureOperationMessages([NotNull] this IServiceCollection services,
			[NotNull] Action<OperationMessageOptions> configureOptions)
		{
			// Check argument
			if (services == null)
			{
				throw new ArgumentNullException(nameof(services));
			}

			if (configureOptions == null)
			{
				throw new ArgumentNullException(nameof(configureOptions));
			}

			services.Configure(configureOptions);

			return services;
		}
	}
}