using System.Security.Claims;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	/// <summary>
	///     Provide authorization-based HTML generation control.
	/// </summary>
	[HtmlTargetElement(ElementName)]
	public class AuthorizeTagHelper : AuthorizeTagHelperBase
	{
		/// <summary>
		///     Initialize a new instance of <see cref="AuthorizeTagHelper" />.
		/// </summary>
		/// <param name="authorizationService">The <see cref="IAuthorizationService" /> instance.</param>
		[UsedImplicitly]
		public AuthorizeTagHelper(IAuthorizationService authorizationService)
			: base(authorizationService)
		{
		}

		/// <summary>
		///     Get or set the policy name which should be melt.
		/// </summary>
		[HtmlAttributeName(PolicyAttributeName)]
		[UsedImplicitly(ImplicitUseKindFlags.Assign)]
		public override string Policy { get; set; }

		/// <summary>
		///     Get or set the additional resource used to authoirzation check if necessary.
		/// </summary>
		/// <seealso cref="IAuthorizationService.AuthorizeAsync(ClaimsPrincipal, object, string)" />
		[HtmlAttributeName(ResourceAttributeName)]
		[UsedImplicitly(ImplicitUseKindFlags.Assign)]
		public override object Resource { get; set; }

		#region Overrides of TagHelper

		#region Overrides of TagHelper

		/// <inheritdoc />
		public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			// Do not output this element itself.
			output.TagName = null;

			return base.ProcessAsync(context, output);
		}

		#endregion

		#endregion

		#region Tag Helper Constants

		/// <summary>
		///     Get the target HTML element name for this tag helper. This field is constant.
		/// </summary>
		[PublicAPI] public const string ElementName = "authorize";

		/// <summary>
		///     Get the HTML attribute name associated with <see cref="Policy" /> property. This field is constant.
		/// </summary>
		[PublicAPI] public const string PolicyAttributeName = "policy";


		/// <summary>
		///     Get the HTML attribute name associated with <see cref="Resource" /> property. This field is constant.
		/// </summary>
		[PublicAPI] public const string ResourceAttributeName = "resource";

		#endregion
	}
}