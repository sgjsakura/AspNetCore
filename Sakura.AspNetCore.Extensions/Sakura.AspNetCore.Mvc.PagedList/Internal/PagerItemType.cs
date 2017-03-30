namespace Sakura.AspNetCore.Mvc.Internal
{
	/// <summary>
	///     Represent as the type of a paged list pager item.
	/// </summary>
	public enum PagerItemType
	{
		/// <summary>
		///     This item is linked to a specified page according to its page number.
		/// </summary>
		Normal = 0,

		/// <summary>
		///     This item is linked to current page.
		/// </summary>
		Current,

		/// <summary>
		///     This item represents as a placeholder for omitted items.
		/// </summary>
		Omitted,

		/// <summary>
		///     This item is linked to the previous page.
		/// </summary>
		Previous,

		/// <summary>
		///     This item is linked to the next page.
		/// </summary>
		Next,

		/// <summary>
		///     This item is linked to the first page.
		/// </summary>
		First,

		/// <summary>
		///     This item is linked to the last page.
		/// </summary>
		Last
	}
}