using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace

namespace Microsoft.AspNet.Builder
{
	/// <summary>
	///     Provide extension methods for <seealso cref="IApplicationBuilder" />. This class is static.
	/// </summary>
	public static class ApplicationBuilderExtensions
	{
		/// <summary>
		///     Configure a application to enable all cookie-related authentication schemes.
		/// </summary>
		/// <param name="app">The <see cref="IApplicationBuilder"/> object.</param>
		/// <exception cref="ArgumentNullException">The <paramref name="app"/> is <c>null</c>.</exception>
		[PublicAPI]
		public static void UseAllCookies([NotNull] this IApplicationBuilder app)
		{
			if (app == null)
			{
				throw new ArgumentNullException(nameof(app));
			}

			// Get the identity options
			var identityOptions = app.ApplicationServices.GetService<IOptions<IdentityOptions>>().Value;

			// Configure cookie authentications with specified order
			app.UseCookieAuthentication(identityOptions.Cookies.ExternalCookie);
			app.UseCookieAuthentication(identityOptions.Cookies.TwoFactorRememberMeCookie);
			app.UseCookieAuthentication(identityOptions.Cookies.TwoFactorUserIdCookie);
			app.UseCookieAuthentication(identityOptions.Cookies.ApplicationCookie);
		}
	}
}