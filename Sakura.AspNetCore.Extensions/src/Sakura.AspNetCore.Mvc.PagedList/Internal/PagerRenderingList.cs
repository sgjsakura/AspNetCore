using System.Collections.Generic;

namespace Sakura.AspNetCore.Mvc.Internal
{
	/// <summary>
	///     Represent as the visual structure of a paged list pager.
	/// </summary>
	public class PagerRenderingList
	{
		/// <summary>
		///     Get or set the items in the list.
		/// </summary>
		public IReadOnlyList<PagerRenderingItem> Items { get; set; }

		/// <summary>
		///     Get or set the additional settings for the pager.
		/// </summary>
		public Dictionary<string, string> Settings { get; set; }
	}
}