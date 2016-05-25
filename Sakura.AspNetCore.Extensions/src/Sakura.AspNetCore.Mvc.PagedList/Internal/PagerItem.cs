using System.Collections.Generic;
using JetBrains.Annotations;

namespace Sakura.AspNetCore.Mvc.Internal
{
	/// <summary>
	///     Represent as a logic item in the pager.
	/// </summary>
	[PublicAPI]
	public class PagerItem
	{
		/// <summary>
		///     Get or set the page number associated with this pager item (if any). Negative value means the page number is not
		///     meaningful in current situation.
		/// </summary>
		public int PageNumber { get; set; }

		/// <summary>
		///     Get or set a value that indicates if this pager item is disabled according to the options.
		/// </summary>
		public bool IsDisabled { get; set; }

		/// <summary>
		///     Get or set the type for this pager item.
		/// </summary>
		public PagerItemType ItemType { get; set; }

		/// <summary>
		///     Get or set all additional settings for this pager item.
		/// </summary>
		public IDictionary<string, string> Settings { get; set; }
	}
}