namespace Sakura.AspNetCore.Mvc
{
	/// <summary>
	///     Define the HTML generation mode of a pager related tag.
	/// </summary>
	public enum PagerGenerationMode
	{
		/// <summary>
		///     Generate full pager, including all pager items and the container.
		/// </summary>
		Full,

		/// <summary>
		///     Generate pager items only, containers will not be generated.
		/// </summary>
		ListOnly
	}
}