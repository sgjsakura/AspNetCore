using System;
using System.Collections.Concurrent;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using Sakura.AspNetCore.Localization.Internal;

namespace Sakura.AspNetCore.Localization
{
	/// <summary>
	///     Provide the default inplementation for <see cref="IDynamicLocalizer" /> service.
	/// </summary>
	public class DynamicLocalizer : IDynamicLocalizer
	{
		[UsedImplicitly]
		public DynamicLocalizer(IHtmlLocalizerFactory htmlLocalizerFactory, IStringLocalizerFactory textLocalizerFactory,
			IViewLocalizer viewLocalizer)
		{
			HtmlLocalizerFactory = htmlLocalizerFactory;
			TextLocalizerFactory = textLocalizerFactory;

			View = new DynamicViewLocalizer(viewLocalizer);
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
		/// <param name="resourceType">The associated <see cref="Type" /> for the <see cref="DynamicHtmlLocalizer" /> object.</param>
		/// <returns>The <see cref="DynamicHtmlLocalizer" /> object for the specified <paramref name="resourceType" />.</returns>
		private DynamicHtmlLocalizer CreateHtmlLocalizer(Type resourceType)
		{
			return new DynamicHtmlLocalizer(HtmlLocalizerFactory.Create(resourceType));
		}

		/// <summary>
		///     Create a new <see cref="DynamicStringLocalizer" /> for a specified <see cref="Type" />.
		/// </summary>
		/// <param name="resourceType">The associated <see cref="Type" /> for the <see cref="DynamicStringLocalizer" /> object.</param>
		/// <returns>The <see cref="DynamicStringLocalizer" /> object for the specified <paramref name="resourceType" />.</returns>
		private DynamicStringLocalizer CreateTextLocalizer(Type resourceType)
		{
			return new DynamicStringLocalizer(TextLocalizerFactory.Create(resourceType));
		}

		/// <summary>
		///     Low level method used to get HTML Localizer instance.
		/// </summary>
		/// <param name="resourceType">The resource type.</param>
		/// <returns>A dynamic object used to access localizable resources.</returns>
		private dynamic Html(Type resourceType)
		{
			return HtmlLocalizers.GetOrAdd(resourceType, CreateHtmlLocalizer);
		}

		/// <summary>
		///     Low level method used to get text Localizer instance.
		/// </summary>
		/// <param name="resourceType">The resource type.</param>
		/// <returns>A dynamic object used to access localizable resources.</returns>
		private dynamic Text(Type resourceType)
		{
			return TextLocalizers.GetOrAdd(resourceType, CreateTextLocalizer);
		}

		#endregion

		#region Public API

		/// <inheritdoc />
		public dynamic Html<T>()
		{
			return Html(typeof(T));
		}

		/// <inheritdoc />
		public dynamic Text<T>()
		{
			return Text(typeof(T));
		}

		/// <inheritdoc />
		public dynamic View { get; }

		#endregion
	}
}