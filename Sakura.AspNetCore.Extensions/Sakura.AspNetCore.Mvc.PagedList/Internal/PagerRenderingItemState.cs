namespace Sakura.AspNetCore.Mvc.Internal;

/// <summary>
///     Represent as the state for a paged list pager item.
/// </summary>
public enum PagerRenderingItemState
{
	/// <summary>
	///     The item is in a normal state, i.e. not activated nor disabled.
	/// </summary>
	Normal = 0,

	/// <summary>
	///     The item is activated.
	/// </summary>
	Active,

	/// <summary>
	///     The item is disabled.
	/// </summary>
	Disabled,

	/// <summary>
	///     The item should not be displayed.
	/// </summary>
	Hidden
}