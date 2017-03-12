using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Sakura.AspNetCore.Mvc.Internal
{
	/// <summary>
	///     Represent as a pager.
	/// </summary>
	public class PagerList
	{
		/// <summary>
		///     Initialize a new pager.
		/// </summary>
		/// <param name="items">All items included in the pager.</param>
		public PagerList([NotNull] [ItemNotNull] IEnumerable<PagerItem> items)
		{
			Items = items ?? throw new ArgumentNullException(nameof(items));
		}

		/// <summary>
		///     Get all items in the pager.
		/// </summary>
		[NotNull]
		[ItemNotNull]
		public IEnumerable<PagerItem> Items { get; }
	}
}