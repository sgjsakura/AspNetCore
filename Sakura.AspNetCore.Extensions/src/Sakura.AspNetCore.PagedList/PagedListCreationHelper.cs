using System.Collections.Generic;
using System.Linq;

namespace Sakura.AspNetCore
{
	/// <summary>
	///     Provide extension methods for creating <see cref="IPagedList{T}" /> instances. This class is static.
	/// </summary>
	public static class PagedListCreationHelper
	{
		/// <summary>
		///     Convert <see cref="IEnumerable{T}" /> object into a <see cref="PagedList{T}" /> object.
		/// </summary>
		/// <typeparam name="T">The element type in the data source.</typeparam>
		/// <param name="source">The source <see cref="IEnumerable{T}" /> object to be converting.</param>
		/// <param name="pageSize">The size of each page.</param>
		/// <param name="pageIndex">The index of the current page. Page index starts from 1.</param>
		/// <param name="creationOptions">Options for the paged list.</param>
		/// <returns>A <see cref="PagedList{T}" /> object created by paging the <paramref name="source" /> object.</returns>
		public static PagedList<T> ToPagedList<T>(this IEnumerable<T> source, int pageSize, int pageIndex = 1,
			PagedListCreationOptions creationOptions = null) => new PagedList<T>(source, pageSize, pageIndex, creationOptions);

		/// <summary>
		///     Convert <see cref="IQueryable{T}" /> object into a <see cref="QueryablePagedList{T}" /> object.
		/// </summary>
		/// <typeparam name="T">The element type in the data source.</typeparam>
		/// <param name="source">The source <see cref="IQueryable{T}" /> object to be converting.</param>
		/// <param name="pageSize">The size of each page.</param>
		/// <param name="pageIndex">The index of the current page. Page index starts from 1.</param>
		/// <param name="creationOptions">Options for the paged list.</param>
		/// <returns>A <see cref="QueryablePagedList{T}" /> object created by paging the <paramref name="source" /> object.</returns>
		public static QueryablePagedList<T> ToPagedList<T>(this IQueryable<T> source, int pageSize, int pageIndex = 1,
				PagedListCreationOptions creationOptions = null)
			=> new QueryablePagedList<T>(source, pageSize, pageIndex, creationOptions);
	}
}