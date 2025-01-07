namespace Sakura.AspNetCore.Mvc;

/// <summary>
///     Define the layout element for a paged list pager.
/// </summary>
public enum PagerLayoutElement
{
	/// <summary>
	///     Main navigation area with page numbers.
	/// </summary>
	Items,

	/// <summary>
	///     "Go to first page" button.
	/// </summary>
	GoToFirstPageButton,

	/// <summary>
	///     "Go to last page" button.
	/// </summary>
	GoToLastPageButton,

	/// <summary>
	///     "Go to previous page" button.
	/// </summary>
	GoToPreviousPageButton,

	/// <summary>
	///     "Go to next page" button.
	/// </summary>
	GoToNextPageButton
}