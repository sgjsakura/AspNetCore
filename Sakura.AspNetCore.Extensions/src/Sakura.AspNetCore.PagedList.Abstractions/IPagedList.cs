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
		///     The current data paged.
		/// </summary>
		[PublicAPI]
		int PageIndex { get; set; }

		/// <summary>
		///     The size of each page.
		/// </summary>
		[PublicAPI]
		int PageSize { get; set; }

		/// <summary>
		///     The total page count.
		/// </summary>
		[PublicAPI]
		int TotalPage { get; }

		/// <summary>
		///     The total count of data source.
		/// </summary>
		[PublicAPI]
		int TotalCount { get; }
	}

	/// <summary>
	///     Extend <see cref="IPagedList" /> in order to provide strong-typed data access.
	/// </summary>
	/// <typeparam name="T">The element type in the data page.</typeparam>
	// ReSharper disable once PossibleInterfaceMemberAmbiguity
	public interface IPagedList<out T> : IPagedList, IReadOnlyList<T>
	{
	}
}