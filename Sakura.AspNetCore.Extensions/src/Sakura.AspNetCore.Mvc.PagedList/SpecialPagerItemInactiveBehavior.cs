namespace Sakura.AspNetCore.Mvc
{
	/// <summary>
	///     Control how to handle inactive special pager items.
	/// </summary>
	public enum SpecialPagerItemInactiveBehavior
	{
		/// <summary>
		///     When the item is inactive, disable it as shown in grey.
		/// </summary>
		Disable,

		/// <summary>
		///     When then item is inactive, remove it from the pager.
		/// </summary>
		Hide
	}
}