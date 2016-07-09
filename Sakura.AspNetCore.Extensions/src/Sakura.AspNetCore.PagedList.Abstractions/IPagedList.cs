using System;
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
		/// The index is start from one (not zero).
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
		/// 
		[PublicAPI]
		int TotalCount { get; }
	}

	/// <summary>
	///     Extend <see cref="IPagedList" /> in order to provide strong-typed data access.
	/// </summary>
	/// <typeparam name="T">The element type in the data page.</typeparam>
	public interface IPagedList<out T> : IPagedList, IReadOnlyList<T>
	{
	}
}
