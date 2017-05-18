using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using Sakura.AspNetCore.Localization.Internal;

namespace Sakura.AspNetCore.Localization
{
	/// <summary>
	/// Provide dynamic style localizable string resource accessibility for <see cref="IStringLocalizer{T}"/> service.
	/// </summary>
	public interface IDynamicStringLocalizer<TResource> : IDynamicLocalizer
	{
		
	}
}