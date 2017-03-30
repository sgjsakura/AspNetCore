using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
// ReSharper disable PossibleMultipleEnumeration
namespace Sakura.AspNetCore
{
	/// <summary>
	///     Provide extension methods for creating <see cref="IPagedList{T}" /> instances. This class is static.
	/// </summary>
	[PublicAPI]
	public static class PagedListCreationHelper
	{
		/// <summary>
		///     Create a snapshot for one page of a <see cref="IAsyncEnumerable{T}" /> object asynchronously.
		/// </summary>
		/// <typeparam name="T">The element type in the data source.</typeparam>
		/// <param name="source">The source <see cref="IEnumerable{T}" /> object to be converting.</param>
		/// <param name="pageSize">The size of each page.</param>
		/// <param name="pageIndex">The index of the current page. Page index starts from 1.</param>
		/// <returns>A <see cref="PagedList{TSource,TElement}" /> object created by paging the <paramref name="source" /> object asynchronously.</returns>
		public static async Task<PagedList<IAsyncEnumerable<T>, T>> ToPagedListAsync<T>([NotNull] this IAsyncEnumerable<T> source, int pageSize, int pageIndex = 1)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (pageSize <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "The page size must be positive.");
			}

			if (pageIndex <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(pageIndex), pageIndex, "The page index must be positive.");
			}

			var skipValue = pageSize * (pageIndex - 1);
			var takeValue = pageSize;

			var currentPage = await source.Skip(skipValue).Take(takeValue).ToArray();
			var totalCount = await source.Count();
			var totalPage = (totalCount - 1) / pageSize + 1;

			return new PagedList<IAsyncEnumerable<T>, T>(currentPage, source, pageSize, pageIndex, totalCount, totalPage);
		}
	}
}