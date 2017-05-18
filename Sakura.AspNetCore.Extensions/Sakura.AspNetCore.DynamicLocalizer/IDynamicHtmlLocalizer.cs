using Microsoft.AspNetCore.Mvc.Localization;
using Sakura.AspNetCore.Localization.Internal;

namespace Sakura.AspNetCore.Localization
{
	/// <summary>
	/// Provide dynamic style localizable string resource accessibility for <see cref="IHtmlLocalizer{TResource}"/> service.
	/// </summary>
	public interface IDynamicHtmlLocalizer<TResource> : IDynamicLocalizer
	{

	}
}