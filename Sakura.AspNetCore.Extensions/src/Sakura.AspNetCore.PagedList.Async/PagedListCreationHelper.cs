using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
// ReSharper disable PossibleMultipleEnumeration
namespace Sakura.AspNetCore
{
	/// <summary>
	///     Provide extension methods for creating <see cref="IPagedList{T}" /> instances asynchronously. This class is static.
	/// </summary>
	[PublicAPI]
	public static class AsyncPagedListCreationHelper
	{
		/// <summary>
		///     Create a snapshot for one page of a <see cref="IQueryable{T}" /> object.
		/// </summary>
		/// <typeparam name="T">The element type in the data source.</typeparam>
		/// <param name="source">The source <see cref="IQueryable{T}" /> object to be converting.</param>
		/// <param name="pageSize">The size of each page.</param>
		/// <param name="pageIndex">The index of the current page. Page index starts from 1.</param>
		/// <returns>A <see cref="IPagedList{T}" /> object created by paging the <paramref name="source" /> object.</returns>

		public static Task<PagedList<IQueryable<T>, T>> ToPagedListAsync<T>([NotNull] this IQueryable<T> source, int pageSize,
				int pageIndex = 1)
			=>
			CreatePagedListCoreAsync(source, pageSize, pageIndex, (data, skip, take) => data.Skip(skip).Take(take).ToArrayAsync(),
				data => data.CountAsync());


		/// <summary>
		/// Core method used to generate an instance of <see cref="PagedList{TSource,TElement}"/>.
		/// </summary>
		/// <typeparam name="TSource">The source type.</typeparam>
		/// <typeparam name="TElement">The element type in the paged list.</typeparam>
		/// <param name="source">The source to be paged.</param>
		/// <param name="pageSize">The size of page.</param>
		/// <param name="pageIndex">The index of the currnet page.</param>
		/// <param name="pageFunc">A paging function that skip some items, and then take some items, finally convert to a list.</param>
		/// <param name="countFunc">A function to count the items in the source.</param>
		/// <returns></returns>
		private static async Task<PagedList<TSource, TElement>> CreatePagedListCoreAsync<TSource, TElement>([NotNull] this TSource source,
			int pageSize, int pageIndex, Func<TSource, int, int, Task<TElement[]>> pageFunc, Func<TSource, Task<int>> countFunc)
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

			var currentPage = await pageFunc(source, skipValue, takeValue);
			var totalCount = await countFunc(source);
			var totalPage = (totalCount - 1) / pageSize + 1;

			return new PagedList<TSource, TElement>(currentPage, source, pageSize, pageIndex, totalCount, totalPage);
		}
	}
}