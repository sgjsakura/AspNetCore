using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using JetBrains.Annotations;
using Sakura.AspNetCore.Mvc.Internal;

namespace Sakura.AspNetCore.Mvc
{
	/// <summary>
	///     Represent as a layout descriptor for a pager.
	/// </summary>
	[TypeConverter(typeof(PagerLayoutConverter))]
	public class PagerLayout
	{
		/// <summary>
		///     Initialize a new <see cref="PagerLayout" /> with specified layout elements.
		/// </summary>
		/// <param name="elements">The sequence of all layout elements.</param>
		public PagerLayout([NotNull] IEnumerable<PagerLayoutElement> elements)
		{
			if (elements == null)
			{
				throw new ArgumentNullException(nameof(elements));
			}

			Elements = new ReadOnlyCollection<PagerLayoutElement>(elements.ToArray());
		}

		/// <summary>
		///     Get the layout element sequence in this layout.
		/// </summary>
		[PublicAPI]
		[NotNull]
		public IEnumerable<PagerLayoutElement> Elements { get; }
	}
}