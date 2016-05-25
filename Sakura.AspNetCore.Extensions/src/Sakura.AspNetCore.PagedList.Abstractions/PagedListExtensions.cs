using System;
using JetBrains.Annotations;

namespace Sakura.AspNetCore
{
	/// <summary>
	///     Provide additional extension method for <see cref="IPagedList{T}" /> objects. This class is static.
	/// </summary>
	[PublicAPI]
	public static class PagedListExtensions
	{
		/// <summary>
		///     Check the argument.
		/// </summary>
		/// <typeparam name="T">The element type in the paged list.</typeparam>
		/// <param name="pagedList">The paged list to be checking.</param>
		/// <exception cref="ArgumentNullException"><paramref name="pagedList" /> is null.</exception>
		private static void CheckArgument<T>(IPagedList<T> pagedList)
		{
			if (pagedList == null)
			{
				throw new ArgumentNullException(nameof(pagedList));
			}
		}


		/// <summary>
		///     Get a value that indicates if the paged list is currently in the first page.
		/// </summary>
		/// <typeparam name="T">The element type in the paged list.</typeparam>
		/// <param name="pagedList">The paged list to be checking.</param>
		/// <returns>If the paged list is currently in the first page, returns <c>true</c>; otherwise,returns <c>false</c>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="pagedList" /> is null.</exception>
		public static bool IsFirstPage<T>(this IPagedList<T> pagedList)
		{
			CheckArgument(pagedList);

			return pagedList.PageIndex == 1;
		}

		/// <summary>
		///     Get a value that indicates if the paged list is currently in the last page.
		/// </summary>
		/// <typeparam name="T">The element type in the paged list.</typeparam>
		/// <param name="pagedList">The paged list to be checking.</param>
		/// <returns>If the paged list is currently in the last page, returns <c>true</c>; otherwise,returns <c>false</c>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="pagedList" /> is null.</exception>
		public static bool IsLastPage<T>(this IPagedList<T> pagedList)
		{
			CheckArgument(pagedList);

			return pagedList.PageIndex == pagedList.TotalPage;
		}

		/// <summary>
		///     Move the paged list to the first page.
		/// </summary>
		/// <typeparam name="T">The element type in the paged list.</typeparam>
		/// <param name="pagedList">The paged list to be moving.</param>
		/// <exception cref="ArgumentNullException"><paramref name="pagedList" /> is null.</exception>
		public static void GoToFirstPage<T>(this IPagedList<T> pagedList)
		{
			CheckArgument(pagedList);

			pagedList.PageIndex = 0;
		}

		/// <summary>
		///     Move the paged list to the last page.
		/// </summary>
		/// <typeparam name="T">The element type in the paged list.</typeparam>
		/// <param name="pagedList">The paged list to be moving.</param>
		/// <exception cref="ArgumentNullException"><paramref name="pagedList" /> is null.</exception>
		public static void GoToLastPage<T>(this IPagedList<T> pagedList)
		{
			CheckArgument(pagedList);

			pagedList.PageIndex = pagedList.TotalPage - 1;
		}

		/// <summary>
		///     Move the paged list to the previous page.
		/// </summary>
		/// <typeparam name="T">The element type in the paged list.</typeparam>
		/// <param name="pagedList">The paged list to be moving.</param>
		/// <returns>
		///     If the operation is successful, returns <c>true</c>; If the paged list is already in the first page, return
		///     <c>false</c>.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="pagedList" /> is null.</exception>
		public static bool GoToPreviousPage<T>(this IPagedList<T> pagedList)
		{
			CheckArgument(pagedList);

			if (pagedList.IsFirstPage())
			{
				return false;
			}

			pagedList.PageIndex--;
			return true;
		}

		/// <summary>
		///     Move the paged list to the next page.
		/// </summary>
		/// <typeparam name="T">The element type in the paged list.</typeparam>
		/// <param name="pagedList">The paged list to be moving.</param>
		/// <returns>
		///     If the operation is successful, returns <c>true</c>; If the paged list is already in the last page, return
		///     <c>false</c>.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="pagedList" /> is null.</exception>
		public static bool GoToNextPage<T>(this IPagedList<T> pagedList)
		{
			CheckArgument(pagedList);

			if (pagedList.IsLastPage())
			{
				return false;
			}

			pagedList.PageIndex++;
			return true;
		}
	}
}