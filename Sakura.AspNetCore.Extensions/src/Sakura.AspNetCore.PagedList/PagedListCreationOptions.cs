namespace Sakura.AspNetCore
{
	/// <summary>
	///     Define additional creation options when create a paged list.
	/// </summary>
	public class PagedListCreationOptions
	{
		/// <summary>
		///     The default cache mode for total count.
		/// </summary>
		public CacheMode TotalCountCacheMode { get; set; }

		/// <summary>
		///     The default cache mode for current page data.
		/// </summary>
		public CacheMode CurrentPageCacheMode { get; set; }
	}
}