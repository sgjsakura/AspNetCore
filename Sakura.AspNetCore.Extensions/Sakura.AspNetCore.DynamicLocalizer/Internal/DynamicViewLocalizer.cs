using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Sakura.AspNetCore.Localization.Internal;

/// <summary>
///     Provide the dynamic style implementation for <see cref="IHtmlLocalizer" /> object.
/// </summary>
public class DynamicViewLocalizer : DynamicHtmlLocalizer, IDynamicViewLocalizer, IViewContextAware
{
	/// <summary>
	///     Initialize a new instance of <see cref="DynamicViewLocalizer" /> object.
	/// </summary>
	/// <param name="localizer">The internal <see cref="IViewLocalizer" /> object.</param>
	[UsedImplicitly]
	public DynamicViewLocalizer(IViewLocalizer localizer)
		: base(localizer)
	{
		Localizer = localizer;
	}

	/// <inheritdoc />
	public new IViewLocalizer Localizer { get; }

	/// <inheritdoc />
	void IViewContextAware.Contextualize(ViewContext viewContext)
	{
		if (Localizer is IViewContextAware viewLocalizer)
			viewLocalizer.Contextualize(viewContext);
	}
}