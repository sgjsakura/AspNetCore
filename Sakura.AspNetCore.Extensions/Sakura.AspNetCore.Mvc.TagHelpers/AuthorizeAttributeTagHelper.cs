using System.Security.Claims;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sakura.AspNetCore.Mvc.TagHelpers;

/// <summary>
///     Provide authorization-based HTML generation control on existing HTML elements.
/// </summary>
[HtmlTargetElement("*", Attributes = PolicyAttributeName)]
public class AuthorizeAttributeTagHelper : AuthorizeTagHelperBase
{
	/// <summary>
	///     Initialize a new instance of <see cref="AuthorizeTagHelper" />.
	/// </summary>
	/// <param name="authorizationService">The <see cref="IAuthorizationService" /> instance.</param>
	[UsedImplicitly]
	public AuthorizeAttributeTagHelper(IAuthorizationService authorizationService)
		: base(authorizationService)
	{
	}

	/// <summary>
	///     Get or set the policy name which should be melted.
	/// </summary>
	[HtmlAttributeName(PolicyAttributeName)]
	[UsedImplicitly(ImplicitUseKindFlags.Assign)]
	public override string Policy { get; set; }

	/// <summary>
	///     Get or set the additional resource used to authorization check if necessary.
	/// </summary>
	/// <seealso cref="IAuthorizationService.AuthorizeAsync(ClaimsPrincipal, object, string)" />
	[HtmlAttributeName(ResourceAttributeName)]
	[UsedImplicitly(ImplicitUseKindFlags.Assign)]
	public override object Resource { get; set; }

	#region Tag Helper Constants

	/// <summary>
	///     Get the HTML attribute name associated with <see cref="Policy" /> property. This field is constant.
	/// </summary>
	[PublicAPI] public const string PolicyAttributeName = "asp-authorize-policy";

	/// <summary>
	///     Get the HTML attribute name associated with <see cref="Resource" /> property. This field is constant.
	/// </summary>
	[PublicAPI] public const string ResourceAttributeName = "asp-authorize-resource";

	#endregion
}