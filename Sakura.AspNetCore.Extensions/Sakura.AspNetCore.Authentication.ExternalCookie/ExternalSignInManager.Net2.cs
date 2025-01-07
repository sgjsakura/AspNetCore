using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Sakura.AspNetCore.Authentication;
#if NETSTANDARD2_0 || NETCOREAPP3_0

	public partial class ExternalSignInManager
	{
		/// <summary>
		///     Initialize a new instance of <see cref="ExternalSignInManager" />.
		/// </summary>
		/// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor" /> service instance.</param>
		/// <param name="authenticationSchemeProvider">the <see cref="IAuthenticationSchemeProvider" /> service instance.</param>
		/// <param name="identityOptions">The Options of <see cref="Microsoft.AspNetCore.Identity.IdentityOptions" />.</param>
		public ExternalSignInManager(IHttpContextAccessor httpContextAccessor,
			IAuthenticationSchemeProvider authenticationSchemeProvider, IOptions<IdentityOptions> identityOptions)
		{
			HttpContext = httpContextAccessor.HttpContext;
			AuthenticationSchemeProvider = authenticationSchemeProvider;
			IdentityOptions = identityOptions.Value;
		}

		/// <summary>
		///     Get the <see cref="Microsoft.AspNetCore.Http.HttpContext" /> instance.
		/// </summary>
		private HttpContext HttpContext { get; }

		/// <summary>
		///     Get thse <see cref="Microsoft.AspNetCore.Identity.IdentityOptions" /> value.
		/// </summary>
		private IdentityOptions IdentityOptions { get; }

		/// <summary>
		///     Get the <see cref="IAuthenticationSchemeProvider" /> service.
		/// </summary>
		private IAuthenticationSchemeProvider AuthenticationSchemeProvider { get; }

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
			var newScheme = IdentityConstants.ApplicationScheme;

			await HttpContext.SignInAsync(externalLoginInfo.CloneAs(newScheme));
			return externalLoginInfo;
		}

		/// <summary>
		///     Get the external principal stored in the external cookie.
		/// </summary>
		/// <returns>A <see cref="ClaimsPrincipal" /> object represents as the external principal.</returns>
		[PublicAPI]
		public async Task<ClaimsPrincipal> GetExternalPrincipalAsync()
		{
			var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
			return result.Principal;
		}

		/// <summary>
		///     Sign out current user.
		/// </summary>
		/// <returns>A task represents as an async operation.</returns>
		[PublicAPI]
		public async Task SignOutAsync()
		{
			await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
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

			return principal.Identities.Any(i => i.AuthenticationType == IdentityConstants.ApplicationScheme);
		}

		/// <summary>
		///     Get all external authentication schemes registered in the current appliation.
		/// </summary>
		/// <returns>The collection of <see cref="AuthenticationDescription" /> for all registered external authentication schemes.</returns>
		[PublicAPI]
		[NotNull]
		[ItemNotNull]
		public async Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync()
		{
			var allSchemes = await AuthenticationSchemeProvider.GetAllSchemesAsync();
			return allSchemes.Where(i => !string.IsNullOrEmpty(i.DisplayName));
		}
	}

#endif