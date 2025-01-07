using JetBrains.Annotations;

namespace Sakura.AspNetCore;

/// <summary>
///     Define as a <see cref="IPagedList" /> with dynamically page changing support.
/// </summary>
public interface IDynamicPagedList : IPagedList
{
	/// <summary>
	///     The current data paged.
	/// </summary>
	[PublicAPI]
	new int PageIndex { get; set; }

	/// <summary>
	///     The size of each page.
	/// </summary>
	[PublicAPI]
	new int PageSize { get; set; }

#if NETCOREAPP3_0

		/// <summary>
		///     Move the paged list to the first page.
		/// </summary>
		/// <param name="pagedList">The paged list to be moving.</param>
		public void GoToFirstPage() => PageIndex = 1;

		/// <summary>
		///     Move the paged list to the last page.
		/// </summary>
		public void GoToLastPage() => PageIndex = TotalPage;

		/// <summary>
		///     Move the paged list to the previous page.
		/// </summary>
		/// <returns>
		///     If the operation is successful, returns <c>true</c>; If the paged list is already in the first page, return
		///     <c>false</c>.
		/// </returns>
		public bool GoToPreviousPage()
		{
			if (IsFirstPage)
				return false;

			PageIndex--;
			return true;
		}

		/// <summary>
		///     Move the paged list to the next page.
		/// </summary>
		/// <returns>
		///     If the operation is successful, returns <c>true</c>; If the paged list is already in the last page, return
		///     <c>false</c>.
		/// </returns>
		public bool GoToNextPage()
		{
			if (IsLastPage)
				return false;

			PageIndex++;
			return true;
		}
#endif

}

/// <summary>
///     Extend <see cref="IPagedList" /> in order to provide strong-typed data access.
/// </summary>
/// <typeparam name="T">The element type in the data page.</typeparam>
// ReSharper disable once PossibleInterfaceMemberAmbiguity
public interface IDynamicPagedList<out T> : IDynamicPagedList, IPagedList<T>
{
}