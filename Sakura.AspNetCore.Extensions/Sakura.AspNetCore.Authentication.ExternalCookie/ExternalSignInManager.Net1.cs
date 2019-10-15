#if NET451 || NETSTANDARD1_3

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.Extensions.Options;

namespace Sakura.AspNetCore.Authentication
{

	public partial class ExternalSignInManager
	{
		/// <summary>
		///     Initialize a new instance.
		/// </summary>
		/// <param name="httpContextAccessor">HTTP context accessor.</param>
		/// <param name="identityOptions">Identity options.</param>
		public ExternalSignInManager(IHttpContextAccessor httpContextAccessor, IOptions<IdentityOptions> identityOptions)
		{
			Authentication = httpContextAccessor.HttpContext.Authentication;
			IdentityOptions = identityOptions.Value;
		}


		/// <summary>
		///     Get the <see cref="IdentityOptions" /> instance.
		/// </summary>
		private IdentityOptions IdentityOptions { get; }

		private AuthenticationManager Authentication { get; }

		/// <summary>
		///     Sign in current user using stored external cookie information.
		/// </summary>
		/// <returns>
		///     If signing-in is succeeded, returns the generated <see cref="ClaimsPrincipal" /> object for current user;
		///     otherwise, returns <c>null</c>.
		/// </returns>
		[PublicAPI]
		public async Task<ClaimsPrincipal> SignInFromExternalCookieAsync()
		{
			var externalLoginInfo = await GetExternalPrincipalAsync();

			if (externalLoginInfo == null)
				return null;

			// the new schame to replace the old one
			var newScheme = IdentityOptions.Cookies.ApplicationCookieAuthenticationScheme;

			await Authentication.SignInAsync(newScheme, externalLoginInfo.CloneAs(newScheme));
			return externalLoginInfo;
		}

		/// <summary>
		///     Get the external principal stored in the external cookie.
		/// </summary>
		/// <returns>A <see cref="ClaimsPrincipal" /> object represents as the external principal.</returns>
		[PublicAPI]
		public Task<ClaimsPrincipal> GetExternalPrincipalAsync()
		{
			return Authentication.AuthenticateAsync(IdentityOptions.Cookies.ExternalCookieAuthenticationScheme);
		}

		/// <summary>
		///     Sign out current user.
		/// </summary>
		/// <returns>A task represents as an async operation.</returns>
		[PublicAPI]
		public async Task SignOutAsync()
		{
			await Authentication.SignOutAsync(IdentityOptions.Cookies.ApplicationCookieAuthenticationScheme);
			await Authentication.SignOutAsync(IdentityOptions.Cookies.ExternalCookieAuthenticationScheme);
		}

		/// <summary>
		///     Get a value that indicates whether the current user is signed-in with application's primary authentication scheme.
		/// </summary>
		/// <param name="principal">The user principal instance.</param>
		/// <exception cref="ArgumentNullException">The <paramref name="principal" /> is <c>null</c>.</exception>
		/// <returns>
		///     If the current user is signed-in with application's primary authentication scheme, returns <c>true</c>;
		///     otherwise, returns <c>false</c>.
		/// </returns>
		[PublicAPI]
		public bool IsSignedIn([NotNull] ClaimsPrincipal principal)
		{
			if (principal == null)
				throw new ArgumentNullException(nameof(principal));

			return principal.Identities.Any(i => i.AuthenticationType == IdentityOptions.Cookies.ApplicationCookieAuthenticationScheme);
		}

		/// <summary>
		///     Get all external authentication schemes registered in the current appliation.
		/// </summary>
		/// <returns>The collection of <see cref="AuthenticationDescription" /> for all registered external authentication schemes.</returns>
		[PublicAPI]
		[NotNull]
		[ItemNotNull]
		public IEnumerable<AuthenticationDescription> GetExternalAuthenticationSchemes()
		{
			return Authentication.GetAuthenticationSchemes().Where(i => !string.IsNullOrEmpty(i.DisplayName));
		}

	}
}

#endif
