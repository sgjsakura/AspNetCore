using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using Sakura.AspNetCore.Localization.Internal;

namespace Sakura.AspNetCore.Localization
{
	/// <summary>
	/// Provide the default inplementation for <see cref="IDynamicLocallizer"/> service.
	/// </summary>
	public class DynamicLocalizer : IDynamicLocallizer
	{
		[UsedImplicitly]
		public DynamicLocalizer(IHtmlLocalizerFactory htmlLocalizerFactory, IStringLocalizerFactory textLocalizerFactory, IHostingEnvironment environment)
		{
			HtmlLocalizerFactory = htmlLocalizerFactory;
			TextLocalizerFactory = textLocalizerFactory;
			Environment = environment;
		}

		/// <summary>
		/// Get the internal <see cref="IHtmlLocalizerFactory"/> object.
		/// </summary>
		private IHtmlLocalizerFactory HtmlLocalizerFactory { get; }

		/// <summary>
		/// Get the internal <see cref="IStringLocalizerFactory"/> object.
		/// </summary>
		private IStringLocalizerFactory TextLocalizerFactory { get; }

		/// <summary>
		/// Get the internal <see cref="IHostingEnvironment"/> object.
		/// </summary>
		private IHostingEnvironment Environment { get; }

		/// <inheritdoc />
		public dynamic Html<T>() => new DynamicHtmlLocalizer(new HtmlLocalizer<T>(HtmlLocalizerFactory));

		/// <inheritdoc />
		public dynamic Text<T>() => new DynamicStringLocalizer(new StringLocalizer<T>(TextLocalizerFactory));

		/// <inheritdoc />
		public dynamic View => new DynamicViewLocalizer(new ViewLocalizer(HtmlLocalizerFactory, Environment));
	}
}