using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Localization;

namespace Sakura.AspNetCore.Localization.Internal;

/// <summary>
///     Provide the default implementation for <see cref="IDynamicStringLocalizer" /> service.
/// </summary>
public class DynamicStringLocalizer : IDynamicStringLocalizer
{
	/// <summary>
	///     Initialize a new instance of <see cref="DynamicStringLocalizer" /> service.
	/// </summary>
	/// <param name="localizer">The internal <see cref="IStringLocalizer" /> service.</param>
	/// <exception cref="ArgumentNullException">The <paramref name="localizer" /> is <c>null</c>.</exception>
	public DynamicStringLocalizer([NotNull] IStringLocalizer localizer)
	{
		Localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
		Text = new DynamicStringLocalizerWrapper(localizer);
	}

	/// <inheritdoc />
	public dynamic Text { get; }

	/// <inheritdoc />
	public IStringLocalizer Localizer { get; }
}

/// <summary>
///     Provide the default implementation for <see cref="DynamicStringLocalizer{TResource}" /> service.
/// </summary>
public class DynamicStringLocalizer<TResource> : DynamicStringLocalizer, IDynamicStringLocalizer<TResource>
{
	/// <summary>
	///     Initialize a new instance of <see cref="DynamicStringLocalizer{TResource}" /> service.
	/// </summary>
	/// <param name="localizer">The internal <see cref="IStringLocalizer{TResource}" /> service.</param>
	/// <exception cref="ArgumentNullException">The <paramref name="localizer" /> is <c>null</c>.</exception>
	[UsedImplicitly]
	public DynamicStringLocalizer([NotNull] IStringLocalizer<TResource> localizer) : base(localizer)
	{
		Localizer = localizer;
	}

	/// <inheritdoc />
	public new IStringLocalizer<TResource> Localizer { get; }
}