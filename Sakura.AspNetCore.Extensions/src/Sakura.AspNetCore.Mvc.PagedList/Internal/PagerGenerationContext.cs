using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sakura.AspNetCore.Mvc.Internal
{
	/// <summary>
	///     Provide necessary information for pager generation.
	/// </summary>
	public class PagerGenerationContext
	{
		/// <summary>
		///     Initialize a new instance with empty setting.
		/// </summary>
		protected PagerGenerationContext()
		{
		}

		/// <summary>
		///     Initialize a new instance.
		/// </summary>
		/// <param name="currentPage">The current page number in the pager.</param>
		/// <param name="totalPage">The total page count of the pager.</param>
		/// <param name="options">The options of the pager.</param>
		/// <param name="viewContext">The current view context.</param>
		/// <param name="generationMode">The generation mode.</param>
		public PagerGenerationContext(
			int currentPage,
			int totalPage,
			PagerOptions options,
			ViewContext viewContext,
			PagerGenerationMode generationMode)
		{
			Options = options;
			ViewContext = viewContext;
			GenerationMode = generationMode;
			CurrentPage = currentPage;
			TotalPage = totalPage;
			GenerationMode = generationMode;
		}

		/// <summary>
		///     Get the current pager number of the pager.
		/// </summary>
		public int CurrentPage { get; protected set; }

		/// <summary>
		///     Get the options for pager generation.
		/// </summary>
		public PagerOptions Options { get; protected set; }

		/// <summary>
		///     Get the total page count of the pager.
		/// </summary>
		public int TotalPage { get; protected set; }

		/// <summary>
		///     Get the pager generation mode.
		/// </summary>
		public PagerGenerationMode GenerationMode { get; protected set; }

		/// <summary>
		///     Get the current view context.
		/// </summary>
		public ViewContext ViewContext { get; protected set; }
	}
}