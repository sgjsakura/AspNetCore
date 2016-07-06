using System.Security.Claims;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Sakura.AspNetCore.Authentication
{
    /// <summary>
    /// Provide default implementation for <see cref="ISecurityStampValidator"/> service.
    /// </summary>
    [UsedImplicitly]
    public class ExternalSecurityStampValidator : ISecurityStampValidator
    {
        /// <summary>
        /// Validates a security stamp of an identity as an asynchronous operation, and rebuilds the identity if the validation succeeds, otherwise rejects
        /// the identity.
        /// </summary>
        /// <param name="context">The context containing the <see cref="ClaimsPrincipal" />
        /// and <see cref="AuthenticationProperties" /> to validate.</param>
        /// <returns>The <see cref="Task" /> that represents the asynchronous validation operation.</returns>
        public Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            return Task.FromResult(0);
        }
    }
}
