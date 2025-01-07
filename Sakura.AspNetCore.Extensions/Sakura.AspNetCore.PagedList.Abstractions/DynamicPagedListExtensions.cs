using System;
using JetBrains.Annotations;

namespace Sakura.AspNetCore;

/// <summary>
///     Provide additional extension method for <see cref="IDynamicPagedList" /> objects. This class is static.
/// </summary>
[PublicAPI]
public static class DynamicPagedListExtensions
{
	/// <summary>
	///     Check the argument.
	/// </summary>
	/// <param name="pagedList">The paged list to be checking.</param>
	/// <exception cref="ArgumentNullException">The <paramref name="pagedList" /> is null.</exception>
	private static void CheckArgument(IDynamicPagedList pagedList)
	{
		if (pagedList == null)
			throw new ArgumentNullException(nameof(pagedList));
	}


	/// <summary>
	///     Move the paged list to the first page.
	/// </summary>
	/// <param name="pagedList">The paged list to be moving.</param>
	/// <exception cref="ArgumentNullException"><paramref name="pagedList" /> is null.</exception>
	public static void GoToFirstPage(this IDynamicPagedList pagedList)
	{
		CheckArgument(pagedList);

		pagedList.PageIndex = 0;
	}

	/// <summary>
	///     Move the paged list to the last page.
	/// </summary>
	/// <param name="pagedList">The paged list to be moving.</param>
	/// <exception cref="ArgumentNullException"><paramref name="pagedList" /> is null.</exception>
	public static void GoToLastPage(this IDynamicPagedList pagedList)
	{
		CheckArgument(pagedList);

		pagedList.PageIndex = pagedList.TotalPage - 1;
	}

	/// <summary>
	///     Move the paged list to the previous page.
	/// </summary>
	/// <param name="pagedList">The paged list to be moving.</param>
	/// <returns>
	///     If the operation is successful, returns <c>true</c>; If the paged list is already in the first page, return
	///     <c>false</c>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="pagedList" /> is null.</exception>
	public static bool GoToPreviousPage(this IDynamicPagedList pagedList)
	{
		CheckArgument(pagedList);

		if (pagedList.IsFirstPage())
			return false;

		pagedList.PageIndex--;
		return true;
	}

	/// <summary>
	///     Move the paged list to the next page.
	/// </summary>
	/// <param name="pagedList">The paged list to be moving.</param>
	/// <returns>
	///     If the operation is successful, returns <c>true</c>; If the paged list is already in the last page, return
	///     <c>false</c>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="pagedList" /> is null.</exception>
	public static bool GoToNextPage(this IDynamicPagedList pagedList)
	{
		CheckArgument(pagedList);

		if (pagedList.IsLastPage())
			return false;

		pagedList.PageIndex++;
		return true;
	}
}