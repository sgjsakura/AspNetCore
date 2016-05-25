namespace Sakura.AspNetCore.Mvc.Internal
{
	/// <summary>
	///     Define necessary feature used to genearte a <see cref="PagerRenderingList" /> from a
	///     <see cref="PagerGenerationContext" />.
	/// </summary>
	public interface IPagerRenderingListGenerator
	{
		/// <summary>
		///     Build a <see cref="PagerRenderingList" /> according to the <see cref="PagerGenerationContext" /> information.
		/// </summary>
		/// <param name="list">The <see cref="PagerList" /> which contains all pager items.</param>
		/// <param name="context">The <see cref="PagerGenerationContext" /> object which includes all the informations needed.</param>
		/// <returns>A <see cref="PagerRenderingList" /> object which represent as a list to ge displayed in the page.</returns>
		PagerRenderingList GenerateRenderingList(PagerList list, PagerGenerationContext context);
	}
}