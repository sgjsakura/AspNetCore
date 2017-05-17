using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Sakura.AspNetCore.Localization
{
	/// <summary>
	/// Provide extension methods to add services. This class is static.
	/// </summary>
    public static class ServiceCollectionExtensions
    {
		/// <summary>
		/// Add default implementation for <see cref="IDynamicLocallizer"/> service.
		/// </summary>
		/// <param name="services">The service collection container.</param>
	    public static void AddDynamicLocalizer([NotNull] this ServiceCollection services)
	    {
		    if (services == null)
		    {
			    throw new ArgumentNullException(nameof(services));
		    }

			services.TryAddTransient<IDynamicLocallizer, DynamicLocalizer>();
	    }
    }
}
