using Microsoft.AspNetCore.Mvc.Localization;

namespace Sakura.AspNetCore.Localization;

/// <summary>
///     Define the necessary feature for dynamic style HTML resource accessing.
/// </summary>
public interface IDynamicHtmlLocalizer
{
	/// <summary>
	///     Get the dynamic object used to access resource strings as HTML format.
	/// </summary>
	dynamic Html { get; }

	/// <summary>
	///     Get the dynamic object used to access resource strings as text format.
	/// </summary>
	dynamic Text { get; }

	/// <summary>
	///     Get the internal <see cref="IHtmlLocalizer" /> service instance.
	/// </summary>
	IHtmlLocalizer Localizer { get; }
}


/// <summary>
///     Provide strong typed resource class for dynamic style HTML resource accessing.
/// </summary>
/// <typeparam name="TResource">The resource type.</typeparam>
public interface IDynamicHtmlLocalizer<TResource> : IDynamicHtmlLocalizer
{
	/// <summary>
	///     Get the strong typed internal <see cref="IHtmlLocalizer{TResource}" /> service instance.
	/// </summary>
	new IHtmlLocalizer<TResource> Localizer { get; }
}