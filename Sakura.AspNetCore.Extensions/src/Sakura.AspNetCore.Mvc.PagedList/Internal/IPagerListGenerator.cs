namespace Sakura.AspNetCore.Mvc.Internal
{
	public interface IPagerListGenerator
	{
		/// <summary>
		///     Generate a <see cref="PagerList" />.
		/// </summary>
		/// <param name="context">The pager generation context.</param>
		/// <returns>A <see cref="PagerList" /> generated for the pager.</returns>
		PagerList GeneratePagerItems(PagerGenerationContext context);
	}
}