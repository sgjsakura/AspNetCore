using Microsoft.AspNetCore.Mvc.Localization;
using Sakura.AspNetCore.Localization.Internal;

namespace Sakura.AspNetCore.Localization
{
	/// <summary>
	/// Provide dynamic style localizable string resource accessibility for <see cref="IViewLocalizer"/> service.
	/// </summary>
	public interface IDynamicViewLocalizer : IDynamicLocalizer
	{

	}
}