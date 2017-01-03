using System;
using System.Collections.Generic;
using System.Linq;
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
	/// Provide common implementations for both <see cref="AuthorizeTagHelper"/> and <see cref="AuthorizeAttributeTagHelper"/>.
	/// </summary>
	public abstract class AuthorizeTagHelperBase : TagHelper
	{
		/// <summary>
		/// Initialize a new instance of <see cref="AuthorizeTagHelperBase"/>.
		/// </summary>
		/// <param name="authorizationService">The <see cref="IAuthorizationService"/> instance.</param>
		protected AuthorizeTagHelperBase(IAuthorizationService authorizationService)
		{
			AuthorizationService = authorizationService;
		}

		private IAuthorizationService AuthorizationService { get; }

		/// <summary>
		/// Get the authoization policy name which current user must melt in order to see the content.
		/// </summary>
		public abstract string Policy { get; set; }

		/// <summary>
		/// Get or set the additional resource used to authoirzation check if necessary.
		/// </summary>
		/// <seealso cref="IAuthorizationService.AuthorizeAsync(ClaimsPrincipal, object, string)"/>
		public abstract object Resource { get; set; }

		/// <summary>
		/// Get or set the view context for this tag helper.
		/// </summary>
		[HtmlAttributeNotBound]
		[ViewContext]
		[UsedImplicitly(ImplicitUseKindFlags.Assign)]
		public ViewContext ViewContext { get; set; }

		#region Overrides of TagHelper

		/// <inheritdoc />
		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			// Omit the start and end tag of itself.
			output.TagName = null;

			// If authorization fails, do not output anything.
			if (!await AuthorizationService.AuthorizeAsync(ViewContext.HttpContext.User, Resource, Policy))
			{
				output.SuppressOutput();
			}
		}

		#endregion
	}
}
