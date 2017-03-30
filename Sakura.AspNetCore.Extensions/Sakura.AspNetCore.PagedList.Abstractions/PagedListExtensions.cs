using System;
using JetBrains.Annotations;

namespace Sakura.AspNetCore
{
	/// <summary>
	///     Provide additional extension method for <see cref="IPagedList" /> objects. This class is static.
	/// </summary>
	[PublicAPI]
	public static class PagedListExtensions
	{
		/// <summary>
		///     Check the argument.
		/// </summary>
		/// <param name="pagedList">The paged list to be checking.</param>
		/// <exception cref="ArgumentNullException">The <paramref name="pagedList" /> is null.</exception>
		private static void CheckArgument(IPagedList pagedList)
		{
			if (pagedList == null)
				throw new ArgumentNullException(nameof(pagedList));
		}


		/// <summary>
		///     Get a value that indicates if the paged list is currently in the first page.
		/// </summary>
		/// <param name="pagedList">The paged list to be checking.</param>
		/// <returns>If the paged list is currently in the first page, returns <c>true</c>; otherwise,returns <c>false</c>.</returns>
		/// <exception cref="ArgumentNullException">The <paramref name="pagedList" /> is null.</exception>
		public static bool IsFirstPage(this IPagedList pagedList)
		{
			CheckArgument(pagedList);

			return pagedList.PageIndex == 1;
		}

		/// <summary>
		///     Get a value that indicates if the paged list is currently in the last page.
		/// </summary>
		/// <param name="pagedList">The paged list to be checking.</param>
		/// <returns>If the paged list is currently in the last page, returns <c>true</c>; otherwise,returns <c>false</c>.</returns>
		/// <exception cref="ArgumentNullException">The <paramref name="pagedList" /> is null.</exception>
		public static bool IsLastPage(this IPagedList pagedList)
		{
			CheckArgument(pagedList);

			return pagedList.PageIndex == pagedList.TotalPage;
		}
	}
}