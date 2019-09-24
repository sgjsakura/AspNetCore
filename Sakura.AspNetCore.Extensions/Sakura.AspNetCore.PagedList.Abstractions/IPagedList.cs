using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Sakura.AspNetCore
{
	/// <summary>
	///     Define the necessary feature needed for data paging.
	/// </summary>
	public interface IPagedList : IList
	{
		/// <summary>
		///     Get the index of the current page in the original data source.
		/// </summary>
		/// <remarks>
		///     The index is start from one (not zero).
		/// </remarks>
		[PublicAPI]
		int PageIndex { get; }

		/// <summary>
		///     Get the size of each page.
		/// </summary>
		[PublicAPI]
		int PageSize { get; }

		/// <summary>
		///     Get the total page count.
		/// </summary>
		[PublicAPI]
		int TotalPage { get; }

		/// <summary>
		///     Get the total item count of data source.
		/// </summary>
		[PublicAPI]
		int TotalCount { get; }

#if NETCOREAPP3_0

		/// <summary>
		/// Get a value that indicates whether the paged list is currently at the first page.
		/// </summary>
		public bool IsFirstPage => PageIndex == 1;

		/// <summary>
		/// Get a value that indicates whether the paged list is currently at the last page.
		/// </summary>
		public bool IsLastPage => PageIndex == TotalPage;


#endif

	}

	/// <summary>
	///     Extend <see cref="IPagedList" /> in order to provide strong-typed data access.
	/// </summary>
	/// <typeparam name="T">The element type in the data page.</typeparam>
	public interface IPagedList<out T> : IPagedList, IReadOnlyList<T>
	{
		/// <summary>
		///     Get the element count in the collection.
		/// </summary>
		new int Count { get; }

		/// <summary>
		///     Get the element at the specified location.
		/// </summary>
		/// <param name="index">The zero-based index of the specified element.</param>
		/// <returns>The element at the <paramref name="index" /> location.</returns>
		new T this[int index] { get; }
	}
}