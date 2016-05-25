using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;

namespace Sakura.AspNetCore.Mvc
{
	/// <summary>
	///     Provide common layouts for paged list pager. This class is static.
	/// </summary>
	[PublicAPI]
	public static class PagerLayouts
	{
		/// <summary>
		///     Generate a <see cref="PagerLayout" /> for specified items.
		/// </summary>
		/// <param name="items">Items list.</param>
		/// <returns>A <see cref="PagerLayout" /> with specified layout.</returns>
		private static PagerLayout GenerateLayout(IEnumerable<PagerLayoutElement> items)
			=> new PagerLayout(new ReadOnlyCollection<PagerLayoutElement>(items.ToArray()));

		#region Shoutcuts

		/// <summary>
		///     The default layout. The navigation bar is in the center, surrounded by the next and previous button. The first and
		///     last button is in the most lateral.
		/// </summary>
		public static PagerLayout Default { get; } =
			GenerateLayout(new[]
			{
				PagerLayoutElement.GoToFirstPageButton,
				PagerLayoutElement.GoToPreviousPageButton,
				PagerLayoutElement.Items,
				PagerLayoutElement.GoToNextPageButton,
				PagerLayoutElement.GoToLastPageButton
			});

		/// <summary>
		///     The navigation bar is in the center, surrounded by the next and previous button. There is no first and last button,
		///     you may use <see cref="PagerOptions.PagerItemsForEndings" /> to show the first and last paged directly.
		/// </summary>
		public static PagerLayout NoFirstAndLastButton { get; } =
			GenerateLayout(new[]
			{
				PagerLayoutElement.GoToPreviousPageButton,
				PagerLayoutElement.Items,
				PagerLayoutElement.GoToNextPageButton
			});

		/// <summary>
		///     Only navigation pager items are in the pager. No additional buttons are included.
		/// </summary>
		public static PagerLayout ItemsOnly { get; } = GenerateLayout(new[] {PagerLayoutElement.Items});

		/// <summary>
		///     Generate a custom layout using specified layout element sequence.
		/// </summary>
		/// <param name="items">The sequence of the layout elements.</param>
		/// <returns>The <see cref="PagerLayout" /> object.</returns>
		public static PagerLayout Custom(params PagerLayoutElement[] items) => GenerateLayout(items);

		/// <summary>
		///     Generate a custom layout using specified layout element sequence.
		/// </summary>
		/// <param name="items">The sequence of the layout elements.</param>
		/// <returns>The <see cref="PagerLayout" /> object.</returns>
		public static PagerLayout Custom(IEnumerable<PagerLayoutElement> items) => GenerateLayout(items);

		#endregion
	}
}