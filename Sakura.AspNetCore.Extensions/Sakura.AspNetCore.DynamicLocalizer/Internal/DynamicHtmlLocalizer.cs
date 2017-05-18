using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Localization;

namespace Sakura.AspNetCore.Localization.Internal
{
	/// <summary>
	///     Provide the default implementation for <see cref="IDynamicHtmlLocalizer" /> service.
	/// </summary>
	public abstract class DynamicHtmlLocalizer : IDynamicHtmlLocalizer
	{
		/// <summary>
		///     Initialize a new instance of <see cref="DynamicHtmlLocalizer" /> service.
		/// </summary>
		/// <param name="localizer">The internal <see cref="IHtmlLocalizer" /> service.</param>
		/// <exception cref="ArgumentNullException">The <paramref name="localizer" /> is <c>null</c>.</exception>
		protected DynamicHtmlLocalizer([NotNull] IHtmlLocalizer localizer)
		{
			Localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));

			Html = new DynamicHtmlLocalizerWrapper(localizer);
			Text = new DynamicHtmlTextLocalizerWrapper(localizer);
		}

		/// <inheritdoc />
		public dynamic Html { get; }

		/// <inheritdoc />
		public dynamic Text { get; }

		/// <inheritdoc />
		public IHtmlLocalizer Localizer { get; }
	}

	/// <summary>
	///     Provide the default implementation for <see cref="IDynamicHtmlLocalizer{TResource}" /> service.
	/// </summary>
	public class DynamicHtmlLocalizer<TResource> : DynamicHtmlLocalizer, IDynamicHtmlLocalizer<TResource>
	{
		/// <summary>
		///     Initialize a new instance of <see cref="DynamicHtmlLocalizer{TResource}" /> service.
		/// </summary>
		/// <param name="localizer">The internal <see cref="IHtmlLocalizer{TResource}" /> service.</param>
		/// <exception cref="ArgumentNullException">The <paramref name="localizer" /> is <c>null</c>.</exception>
		[UsedImplicitly]
		public DynamicHtmlLocalizer([NotNull] IHtmlLocalizer<TResource> localizer)
			: base(localizer)
		{
			Localizer = localizer;
		}

		/// <inheritdoc />
		public new IHtmlLocalizer<TResource> Localizer { get; }
	}
}