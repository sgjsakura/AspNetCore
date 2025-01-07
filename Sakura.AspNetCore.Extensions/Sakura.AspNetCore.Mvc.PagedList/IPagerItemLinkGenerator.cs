using System.ComponentModel;
using JetBrains.Annotations;
using Sakura.AspNetCore.Mvc.Internal;

namespace Sakura.AspNetCore.Mvc;

/// <summary>
///     Define the necessary feature for a pager item link generator.
/// </summary>
[TypeConverter(typeof(PagerItemLinkGeneratorConverter))]
public interface IPagerItemLinkGenerator
{
	/// <summary>
	///     Generate the link url for the specified <see cref="PagerItem" />.
	/// </summary>
	/// <param name="context">The generation context.</param>
	string? GenerateLink([NotNull] PagerItemGenerationContext context);
}