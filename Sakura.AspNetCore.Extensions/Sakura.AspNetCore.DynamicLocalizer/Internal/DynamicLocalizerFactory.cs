using System;
using System.Collections.Concurrent;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;

namespace Sakura.AspNetCore.Localization.Internal
{
	/// <summary>
	///     Provide the default inplementation for <see cref="IDynamicLocalizerFactory" /> service.
	/// </summary>
	public class DynamicLocalizerFactory : IDynamicLocalizerFactory, IViewContextAware
	{

		[UsedImplicitly]
		public DynamicLocalizerFactory(IHtmlLocalizerFactory htmlLocalizerFactory, IStringLocalizerFactory textLocalizerFactory,
			IViewLocalizer viewLocalizer)
		{
			HtmlLocalizerFactory = htmlLocalizerFactory;
			TextLocalizerFactory = textLocalizerFactory;

			_View = new DynamicViewLocalizer(viewLocalizer);
		}

		#region Internal Services

		/// <summary>
		///     Get the internal <see cref="IHtmlLocalizerFactory" /> object.
		/// </summary>
		private IHtmlLocalizerFactory HtmlLocalizerFactory { get; }

		/// <summary>
		///     Get the internal <see cref="IStringLocalizerFactory" /> object.
		/// </summary>
		private IStringLocalizerFactory TextLocalizerFactory { get; }

		/// <summary>
		/// The internal <see cref="DynamicViewLocalizer"/> object.
		/// </summary>
		private readonly DynamicViewLocalizer _View;

		#endregion

		#region Caching Maintanance

		/// <summary>
		///     Get the internal dictionary used to store all <see cref="DynamicHtmlLocalizer" /> for <see cref="Type" />
		///     instances.
		/// </summary>
		private ConcurrentDictionary<Type, DynamicHtmlLocalizer> HtmlLocalizers { get; } =
			new ConcurrentDictionary<Type, DynamicHtmlLocalizer>();

		/// <summary>
		///     Get the internal dictionary used to store all <see cref="DynamicStringLocalizer" /> for <see cref="Type" />
		///     instances.
		/// </summary>
		private ConcurrentDictionary<Type, DynamicStringLocalizer> TextLocalizers { get; } =
			new ConcurrentDictionary<Type, DynamicStringLocalizer>();

		/// <summary>
		///     Create a new <see cref="DynamicHtmlLocalizer" /> for a specified <see cref="Type" />.
		/// </summary>
		/// <typeparam name="TResource">The resource type.</typeparam>
		/// <returns>The <see cref="DynamicHtmlLocalizer" /> object for the specified <typeparamref name="TResource"/>.</returns>
		private DynamicHtmlLocalizer<TResource> CreateHtmlLocalizer<TResource>()
		{
			return new DynamicHtmlLocalizer<TResource>(new HtmlLocalizer<TResource>(HtmlLocalizerFactory));
		}

		/// <summary>
		///     Create a new <see cref="DynamicStringLocalizer" /> for a specified <see cref="Type" />.
		/// </summary>
		/// <typeparam name="TResource">The resource type.</typeparam>
		/// <returns>The <see cref="DynamicStringLocalizer" /> object for the specified <typeparamref name="TResource"/>.</returns>
		private DynamicStringLocalizer<TResource> CreateTextLocalizer<TResource>()
		{
			return new DynamicStringLocalizer<TResource>(new StringLocalizer<TResource>(TextLocalizerFactory));
		}

		#endregion

		#region Public API

		/// <inheritdoc />
		public dynamic Html<T>()
		{
			return HtmlLocalizers.GetOrAdd(typeof(T), CreateHtmlLocalizer<T>());
		}

		/// <inheritdoc />
		public dynamic Text<T>()
		{
			return TextLocalizers.GetOrAdd(typeof(T), CreateTextLocalizer<T>());
		}

		/// <inheritdoc />
		public dynamic View => IsInViewContext
			? _View
			: throw new InvalidOperationException(
				"The localizer is not under a view context and the view localizer interface is not available.");

		#endregion

		#region View Context Detection

		/// <summary>
		/// Get or set a value that indicates if this localizer is under a view context environment.
		/// </summary>
		private static bool IsInViewContext { get; set; } = false;

		/// <inheritdoc />
		public void Contextualize(ViewContext viewContext)
		{
			IsInViewContext = true;
		}

		#endregion
	}
}