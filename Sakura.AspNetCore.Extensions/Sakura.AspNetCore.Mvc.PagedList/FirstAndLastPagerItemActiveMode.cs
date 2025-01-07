namespace Sakura.AspNetCore.Mvc;

/// <summary>
///     Define how to active the fist and last item in a pager.
/// </summary>
public enum FirstAndLastPagerItemActiveMode
{
	/// <summary>
	///     The first or last button is always active regardless what the current page is.
	/// </summary>
	Always,

	/// <summary>
	///     The first or last button is inactive if the current page is just the first/last page.
	/// </summary>
	NotInCurrentPage,

	/// <summary>
	///     The first or last button is inactive if user can see the link item for the first/last page.
	/// </summary>
	NotInVisiblePageList
}