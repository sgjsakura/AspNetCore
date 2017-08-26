using System.Security.Claims;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	/// <summary>
	///     Provide common implementations for both <see cref="AuthorizeTagHelper" /> and
	///     <see cref="AuthorizeAttributeTagHelper" />.
	/// </summary>
	public abstract class AuthorizeTagHelperBase : TagHelper
	{
		/// <summary>
		///     Initialize a new instance of <see cref="AuthorizeTagHelperBase" />.
		/// </summary>
		/// <param name="authorizationService">The <see cref="IAuthorizationService" /> instance.</param>
		protected AuthorizeTagHelperBase(IAuthorizationService authorizationService)
		{
			AuthorizationService = authorizationService;
		}

		private IAuthorizationService AuthorizationService { get; }

		/// <summary>
		///     Get the authoization policy name which current user must melt in order to see the content.
		/// </summary>
		public abstract string Policy { get; set; }

		/// <summary>
		///     Get or set the additional resource used to authoirzation check if necessary.
		/// </summary>
		/// <seealso cref="IAuthorizationService.AuthorizeAsync(ClaimsPrincipal, object, string)" />
		public abstract object Resource { get; set; }

		/// <summary>
		///     Get or set the view context for this tag helper.
		/// </summary>
		[HtmlAttributeNotBound]
		[ViewContext]
		[UsedImplicitly(ImplicitUseKindFlags.Assign)]
		public ViewContext ViewContext { get; set; }

#if NETSTANDARD2_0

		/// <summary>
		///     Get a value that indicates if the current user is authorized.
		/// </summary>
		/// <returns>If the current user is authorized, returns <c>true</c>; otherwise, returns <c>false</c>.</returns>
		protected async Task<bool> IsAuthorizedAsync()
		{
			var result = await AuthorizationService.AuthorizeAsync(ViewContext.HttpContext.User, Resource, Policy);
			return result.Succeeded;
		}

#else
		/// <summary>
		///     Get a value that indicates if the current user is authorized.
		/// </summary>
		/// <returns>If the current user is authorized, returns <c>true</c>; otherwise, returns <c>false</c>.</returns>
		protected Task<bool> IsAuthorizedAsync()
		{
			return AuthorizationService.AuthorizeAsync(ViewContext.HttpContext.User, Resource, Policy);
		}

#endif

		#region Overrides of TagHelper

		/// <inheritdoc />
		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			// If the authroization rule is not melt, do not output anything.
			if (!await IsAuthorizedAsync())
				output.SuppressOutput();
		}

		#endregion
	}
}