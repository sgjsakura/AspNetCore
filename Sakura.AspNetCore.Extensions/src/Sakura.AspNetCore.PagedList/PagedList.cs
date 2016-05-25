using System.Collections.Generic;
using System.Linq;

namespace Sakura.AspNetCore
{
	/// <summary>
	///     Represent as a paged list.
	/// </summary>
	/// <typeparam name="T">The element type in the data source.</typeparam>
	public class PagedList<T> : PagedListBase<IEnumerable<T>, T>
	{
		/// <summary>
		///     Initialize a new instance with specified information.
		/// </summary>
		/// <param name="source">The data source to be paging.</param>
		/// <param name="pageSize">The size of each page.</param>
		/// <param name="pageIndex">The index of the current page. Page index starts from 1.</param>
		/// <param name="creationOptions">Additional options for the paged list.</param>
		public PagedList(IEnumerable<T> source, int pageSize, int pageIndex = 1,
			PagedListCreationOptions creationOptions = null) : base(source, pageSize, pageIndex, creationOptions)
		{
		}

		/// <summary>
		///     Get the total count of the data source.
		/// </summary>
		/// <returns>The total count of the data source.</returns>
		protected override int GetTotalCount() => Source.Count();

		/// <summary>
		///     Get the data in the current page.
		/// </summary>
		/// <returns>The data in the current page.</returns>
		protected override IEnumerable<T> GetCurrentPage() => Source.Skip((PageIndex - 1)*PageSize).Take(PageSize);

		/// <summary>
		///     Make a cached copy for the current page.
		/// </summary>
		/// <param name="source">The data source to be caching.</param>
		/// <returns>The cached data.</returns>
		protected override IEnumerable<T> CacheData(IEnumerable<T> source) => source.ToArray();
	}
}