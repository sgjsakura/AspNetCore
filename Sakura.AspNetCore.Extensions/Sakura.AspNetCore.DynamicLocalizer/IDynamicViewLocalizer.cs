using Microsoft.AspNetCore.Mvc.Localization;

namespace Sakura.AspNetCore.Localization
{
	/// <summary>
	///     Provide dynamic style localizable string resource accessibility for <see cref="IViewLocalizer" /> service.
	/// </summary>
	public interface IDynamicViewLocalizer : IDynamicHtmlLocalizer
	{
		/// <summary>
		///     Get the internal <see cref="IViewLocalizer" /> service instance.
		/// </summary>
		new IViewLocalizer Localizer { get; }
	}
}