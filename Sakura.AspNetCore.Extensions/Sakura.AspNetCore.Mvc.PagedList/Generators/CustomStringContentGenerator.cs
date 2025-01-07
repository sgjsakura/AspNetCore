using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;

namespace Sakura.AspNetCore.Mvc.Generators;

/// <summary>
///     Generate <see cref="IHtmlContent" /> from a custom string generator.
/// </summary>
public class CustomStringContentGenerator : StringContentGenerator
{
	/// <summary>
	///     Initialize a new instance of <see cref="CustomStringContentGenerator" />.
	/// </summary>
	/// <param name="stringContentGenerator">The string generator callback delegate.</param>
	/// <param name="encodeText">Whether the format result should be HTML encoded before be written to page.</param>
	/// <exception cref="ArgumentNullException"><paramref name="stringContentGenerator" /> is <c>null</c>.</exception>
	public CustomStringContentGenerator([NotNull] Func<PagerItemGenerationContext, string> stringContentGenerator,
		bool encodeText) : base(encodeText)
	{
		StringContentGenerator = stringContentGenerator ?? throw new ArgumentNullException(nameof(stringContentGenerator));
	}

	/// <summary>
	///     Get the string generator callback delegate.
	/// </summary>
	[PublicAPI]
	[NotNull]
	public Func<PagerItemGenerationContext, string> StringContentGenerator { get; }

	/// <summary>
	///     Generate the content string.
	/// </summary>
	/// <param name="context">The generation context.</param>
	/// <returns>The generated content string.</returns>
	protected override string GenerateContentString(PagerItemGenerationContext context)
	{
		return StringContentGenerator(context);
	}
}