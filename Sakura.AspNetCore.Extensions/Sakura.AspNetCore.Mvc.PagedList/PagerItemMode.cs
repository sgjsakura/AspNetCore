namespace Sakura.AspNetCore.Mvc;

/// <summary>
///     Define the working mode for a pager item.
/// </summary>
public enum PagerItemMode
{
	/// <summary>
	///     The pager item is used to navigate the browser to a new location. The generated item is usually an "a" element.
	/// </summary>
	Navigation = 0,

	/// <summary>
	///     The pager item is used to submit data to a specified location. The generated item is usually an "button" element
	///     with the type "submit" specified.
	/// </summary>
	Submition,

	/// <summary>
	///     The pager item is used to raise click events. The generated item is usually an "button" element with the type
	///     "button" specified.
	/// </summary>
	Clicking
}