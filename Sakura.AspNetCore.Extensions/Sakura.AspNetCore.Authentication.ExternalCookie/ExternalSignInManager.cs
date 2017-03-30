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
	/// <summary>
	///     Provde methods used for external signing-in cooperations.
	/// </summary>
	public class ExternalSignInManager
	{
		/// <summary>
		///     Initialize a new instance.
		/// </summary>
		/// <param name="httpContextAccessor">HTTP context accessor.</param>
		/// <param name="identityOptions">Identity options.</param>
		public ExternalSignInManager(IHttpContextAccessor httpContextAccessor, IOptions<IdentityOptions> identityOptions)
		{
			AuthenticationManager = httpContextAccessor.HttpContext.Authentication;
			IdentityOptions = identityOptions.Value;
		}

		/// <summary>
		///     Get the <see cref="AuthenticationManager" /> instance.
		/// </summary>
		private AuthenticationManager AuthenticationManager { get; }

		/// <summary>
		///     Get the <see cref="IdentityOptions" /> instance.
		/// </summary>
		private IdentityOptions IdentityOptions { get; }

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
			{
				return null;
			}

			// the new schame to replace the old one
			var newScheme = IdentityOptions.Cookies.ApplicationCookieAuthenticationScheme;

			await AuthenticationManager.SignInAsync(newScheme, externalLoginInfo.CloneAs(newScheme));
			return externalLoginInfo;
		}

		/// <summary>
		/// Get the external principal stored in the external cookie.
		/// </summary>
		/// <returns>A <see cref="ClaimsPrincipal"/> object represents as the external principal.</returns>
		[PublicAPI]
		public Task<ClaimsPrincipal> GetExternalPrincipalAsync()
		{
			return AuthenticationManager.AuthenticateAsync(IdentityOptions.Cookies.ExternalCookieAuthenticationScheme);
		}

		/// <summary>
		///     Sign out current user.
		/// </summary>
		/// <returns>A task represents as an async operation.</returns>
		[PublicAPI]
		public async Task SignOutAsync()
		{
			await AuthenticationManager.SignOutAsync(IdentityOptions.Cookies.ApplicationCookieAuthenticationScheme);
			await AuthenticationManager.SignOutAsync(IdentityOptions.Cookies.ExternalCookieAuthenticationScheme);
		}

		/// <summary>
		///     Get the user name from a <see cref="ClaimsIdentity" />.
		/// </summary>
		/// <param name="identity">The <see cref="ClaimsIdentity" /> instance.</param>
		/// <exception cref="ArgumentNullException">The <paramref name="identity" /> is <c>null</c>.</exception>
		/// <returns>
		///     The user name included in the <paramref name="identity" />. If no user name is included, this method will
		///     return <c>null</c>.
		/// </returns>
		[CanBeNull]
		[PublicAPI]
		public string GetUserName([NotNull] ClaimsIdentity identity)
		{
			if (identity == null)
				throw new ArgumentNullException(nameof(identity));

			return identity.FindFirst(IdentityOptions.ClaimsIdentity.UserNameClaimType)?.Value;
		}

		/// <summary>
		///     Get the user name from a <see cref="ClaimsPrincipal" />.
		/// </summary>
		/// <param name="principal">The <see cref="ClaimsPrincipal" /> instance.</param>
		/// <exception cref="ArgumentNullException">The <paramref name="principal" /> is <c>null</c>.</exception>
		/// <returns>
		///     The user name included in the <paramref name="principal" />. If no user name is included, this method will
		///     return <c>null</c>.
		/// </returns>
		[CanBeNull]
		[PublicAPI]
		public string GetUserName([NotNull] ClaimsPrincipal principal)
		{
			if (principal == null)
				throw new ArgumentNullException(nameof(principal));

			return principal.FindFirst(IdentityOptions.ClaimsIdentity.UserNameClaimType)?.Value;
		}

		/// <summary>
		///     Get the user identifier from a <see cref="ClaimsIdentity" />.
		/// </summary>
		/// <param name="identity">The <see cref="ClaimsIdentity" /> instance.</param>
		/// <exception cref="ArgumentNullException">The <paramref name="identity" /> is <c>null</c>.</exception>
		/// <returns>
		///     The user identifier included in the <paramref name="identity" />. If no user identifier is included, this
		///     method will return <c>null</c>.
		/// </returns>
		[CanBeNull]
		[PublicAPI]
		public string GetUserId([NotNull] ClaimsIdentity identity)
		{
			if (identity == null)
				throw new ArgumentNullException(nameof(identity));

			return identity.FindFirst(IdentityOptions.ClaimsIdentity.UserIdClaimType)?.Value;
		}


		/// <summary>
		///     Get the user identifier from a <see cref="ClaimsPrincipal" />.
		/// </summary>
		/// <param name="principal">The <see cref="ClaimsPrincipal" /> instance.</param>
		/// <exception cref="ArgumentNullException">The <paramref name="principal" /> is <c>null</c>.</exception>
		/// <returns>
		///     The user identifier included in the <paramref name="principal" />. If no user identifier is included, this
		///     method will return <c>null</c>.
		/// </returns>
		[CanBeNull]
		[PublicAPI]
		public string GetUserId([NotNull] ClaimsPrincipal principal)
		{
			if (principal == null)
				throw new ArgumentNullException(nameof(principal));

			return principal.FindFirst(IdentityOptions.ClaimsIdentity.UserIdClaimType)?.Value;
		}

		/// <summary>
		///     Get the the collection of all roles for a <see cref="ClaimsPrincipal" />.
		/// </summary>
		/// <param name="principal">The <see cref="ClaimsPrincipal" /> instance.</param>
		/// <exception cref="ArgumentNullException">The <paramref name="principal" /> is <c>null</c>.</exception>
		/// <returns>
		///     A collection that including all the roles for the <paramref name="principal" />. If the user has no roles,
		///     this method will return a empty collection.
		/// </returns>
		[NotNull]
		[PublicAPI]
		public IEnumerable<string> GetUserRoles([NotNull] ClaimsPrincipal principal)
		{
			if (principal == null)
				throw new ArgumentNullException(nameof(principal));

			return principal.FindAll(IdentityOptions.ClaimsIdentity.RoleClaimType).Select(i => i.Value);
		}

		/// <summary>
		///     Get the the collection of all roles for a <see cref="ClaimsIdentity" />.
		/// </summary>
		/// <param name="identity">The <see cref="ClaimsIdentity" /> instance.</param>
		/// <exception cref="ArgumentNullException">The <paramref name="identity" /> is <c>null</c>.</exception>
		/// <returns>
		///     A collection that including all the roles for the <paramref name="identity" />. If the user has no roles, this
		///     method will return a empty collection.
		/// </returns>
		[NotNull]
		[PublicAPI]
		public IEnumerable<string> GetUserRoles([NotNull] ClaimsIdentity identity)
		{
			if (identity == null)
				throw new ArgumentNullException(nameof(identity));

			return identity.FindAll(IdentityOptions.ClaimsIdentity.RoleClaimType).Select(i => i.Value);
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

			return principal.Identities.Any(
				i => i.AuthenticationType == IdentityOptions.Cookies.ApplicationCookieAuthenticationScheme);
		}

		/// <summary>
		/// Get all external authentication schemes registered in the current appliation.
		/// </summary>
		/// <returns>The collection of <see cref="AuthenticationDescription"/> for all registered external authentication schemes.</returns>
		[PublicAPI]
		[NotNull]
		[ItemNotNull]
		public IEnumerable<AuthenticationDescription> GetExternalAuthenticationSchemes()
		{
			return AuthenticationManager.GetAuthenticationSchemes().Where(i => !string.IsNullOrEmpty(i.DisplayName));
		}
	}
}