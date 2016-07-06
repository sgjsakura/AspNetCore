using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Sakura.AspNetCore.Authentication
{
	/// <summary>
	/// Provide utility methods for identity operations. This class is static.
	/// </summary>
	[PublicAPI]
	public static class IdentityHelper
	{

		/// <summary>
		///     Create a new <see cref="ClaimsIdentity"/> based on a existing <see cref="ClaimsIdentity"/> with a new authentication type.
		/// </summary>
		/// <param name="identity">The identity to be cloned.</param>
		/// <param name="authenticationType">The authentication type of the new identity.</param>
		/// <returns>The cloned new identity.</returns>
		/// <exception cref="ArgumentNullException">The <paramref name="identity" /> or <paramref name="authenticationType" /> is <c>null</c>.</exception>
		public static ClaimsIdentity CloneAs(this ClaimsIdentity identity, string authenticationType)
		{
			if (identity == null)
			{
				throw new ArgumentNullException(nameof(identity));
			}
			if (authenticationType == null)
			{
				throw new ArgumentNullException(nameof(authenticationType));
			}

			return new ClaimsIdentity(identity.Claims, authenticationType, identity.NameClaimType,
				identity.RoleClaimType);
		}

		/// <summary>
		///     Create a new <see cref="ClaimsPrincipal"/> based on a existing <see cref="ClaimsPrincipal"/> with a new authentication type for all of its identities.
		/// </summary>
		/// <param name="principal">The principal to be cloned.</param>
		/// <param name="authenticationType">The authentication type of the new identity.</param>
		/// <returns>The cloned new principal.</returns>
		/// <exception cref="ArgumentNullException">The <paramref name="principal" /> or <paramref name="authenticationType" /> is <c>null</c>.</exception>
		public static ClaimsPrincipal CloneAs(this ClaimsPrincipal principal, string authenticationType)
		{
			if (principal == null)
			{
				throw new ArgumentNullException(nameof(principal));
			}
			if (authenticationType == null)
			{
				throw new ArgumentNullException(nameof(authenticationType));
			}

			var newIdentities = principal.Identities.Select(i => i.CloneAs(authenticationType));

			return new ClaimsPrincipal(newIdentities);
		}
	}
}
