using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;

namespace Sakura.AspNetCore.Authentication
{
	/// <summary>
	///     Provide methods used for external signing-in co-operations.
	/// </summary>
	public partial class ExternalSignInManager
	{
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
	}
}