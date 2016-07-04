using System;
using System.Collections.Generic;
using System.Linq;

namespace Sakura.AspNetCore.Mvc.Internal
{
	/// <summary>
	///     Provide default implementation for <see cref="IPagerListGenerator" />.
	/// </summary>
	public class DefaultPagerListGenerator : IPagerListGenerator
	{
		/// <summary>
		///     Generate a <see cref="PagerList" />.
		/// </summary>
		/// <param name="context">The pager generation context.</param>
		/// <returns>A <see cref="PagerList" /> generated for the pager.</returns>
		public PagerList GeneratePagerItems(PagerGenerationContext context)
		{
			var items = GeneratePagerItemsCore(context.CurrentPage, context.TotalPage, context.Options);

			return new PagerList(items);
		}

		/// <summary>
		///     Core method for generating normal pager items according to the page information.
		/// </summary>
		/// <param name="currentPage">The current page number.</param>
		/// <param name="totalPage">The count of total pages.</param>
		/// <param name="expandForCurrentPage">How many pages should be expanded for current page.</param>
		/// <param name="expandForEnding">How many pages should be expanded for ending.</param>
		/// <returns></returns>
		private static IEnumerable<PagerItem> GeneratePagerNormalItems(int currentPage, int totalPage, int expandForCurrentPage,
			int expandForEnding)
		{
			var pageNumberList = new SortedSet<int> {currentPage};

			// Expand for current page
			for (var i = 1; i <= expandForCurrentPage; i++)
			{
				pageNumberList.Add(currentPage + i);
				pageNumberList.Add(currentPage - i);
			}

			// Expand for ending
			for (var i = 1; i <= expandForEnding; i++)
			{
				pageNumberList.Add(i);
				pageNumberList.Add(totalPage + 1 - i);
			}

			// Remove invalid items
			pageNumberList.RemoveWhere(i => i < 1 || i > totalPage);

			var lastPageNumber = 0;

			foreach (var i in pageNumberList)
			{
				// Skipped some item
				if (i - lastPageNumber > 1)
				{
					yield return new PagerItem
					{
						ItemType = PagerItemType.Omitted,
						PageNumber = -1
					};
				}

				yield return new PagerItem
				{
					// is current page
					ItemType = i == currentPage ? PagerItemType.Current : PagerItemType.Normal,
					PageNumber = i
				};

				// Set last page
				lastPageNumber = i;
			}

			// last page omit handling
			if (lastPageNumber < totalPage)
			{
				yield return new PagerItem
				{
					ItemType = PagerItemType.Omitted,
					PageNumber = -1
				};
			}
		}

		/// <summary>
		///     Generate a pager item for a special layout element.
		/// </summary>
		/// <param name="currentPage">The current page number.</param>
		/// <param name="totalPage">The count of total pages.</param>
		/// <param name="element">The <see cref="PagerLayoutElement" /> to generate the <see cref="PagerItem" />.</param>
		/// <returns>The generated <see cref="PagerItem" /> for the <paramref name="element" />.</returns>
		/// <exception cref="ArgumentException">
		///     The <paramref name="element" /> is <see cref="PagerLayoutElement.Items" />, or it is not
		///     a valid enum item.
		/// </exception>
		private PagerItem GenerateSpecialItems(int currentPage, int totalPage, PagerLayoutElement element)
		{
			PagerItemType type;
			int pageNumber;

			switch (element)
			{
				case PagerLayoutElement.GoToFirstPageButton:
					type = PagerItemType.First;
					pageNumber = 1;
					break;
				case PagerLayoutElement.GoToLastPageButton:
					type = PagerItemType.Last;
					pageNumber = totalPage;
					break;
				case PagerLayoutElement.GoToPreviousPageButton:
					type = PagerItemType.Previous;
					pageNumber = currentPage - 1;
					break;
				case PagerLayoutElement.GoToNextPageButton:
					type = PagerItemType.Next;
					pageNumber = currentPage + 1;
					break;
				case PagerLayoutElement.Items:
					throw new ArgumentException("This method is invalid for normal item layout.", nameof(element));
				default:
					throw new ArgumentException("The argument is not a valid enum item.", nameof(element));
			}

			return new PagerItem
			{
				ItemType = type,
				PageNumber = pageNumber
			};
		}

		/// <summary>
		///     Generate pager items for the specified layout element.
		/// </summary>
		/// <param name="element">The layout element to generating the pager items.</param>
		/// <param name="currentPage">The current page number.</param>
		/// <param name="totalPage">The count of total pages.</param>
		/// <param name="options">The options of the pager.</param>
		/// <returns>The generated all pager items collection for the specified <paramref name="element" />.</returns>
		/// <exception cref="ArgumentException">The <paramref name="element" /> is not a valid enum item.</exception>
		private IEnumerable<PagerItem> GenerateItemsForLayoutElement(PagerLayoutElement element, int currentPage,
			int totalPage, PagerOptions options)
		{
			switch (element)
			{
				case PagerLayoutElement.Items:
					return GeneratePagerNormalItems(currentPage, totalPage, options.ExpandPageItemsForCurrentPage,
						options.PagerItemsForEndings);
				case PagerLayoutElement.GoToFirstPageButton:
				case PagerLayoutElement.GoToLastPageButton:
				case PagerLayoutElement.GoToNextPageButton:
				case PagerLayoutElement.GoToPreviousPageButton:
					return new[] {GenerateSpecialItems(currentPage, totalPage, element)};
				default:
					throw new ArgumentException("The value of the argument is not a valid enum item.", nameof(element));
			}
		}

		/// <summary>
		///     Generate all pager items for a layout sequence.
		/// </summary>
		/// <param name="currentPage">The current page number.</param>
		/// <param name="totalPage">The count of total pages.</param>
		/// <param name="layout">A sequence that indicates the layout of the pager.</param>
		/// <param name="options">The options of the pager.</param>
		/// <returns>The collection for all pager items.</returns>
		private IEnumerable<PagerItem> GenerateItemsCore(int currentPage, int totalPage,
			IEnumerable<PagerLayoutElement> layout, PagerOptions options)
		{
			// Dictionary for each layout, used to cache generation results
			var layoutResult = new Dictionary<PagerLayoutElement, PagerItem[]>();

			var result = new List<PagerItem>();

			foreach (var element in layout)
			{
				PagerItem[] elementResult;

				// If the dictionary not contains the layout result, generate once and store it to the dictionary
				if (!layoutResult.TryGetValue(element, out elementResult))
				{
					elementResult = GenerateItemsForLayoutElement(element, currentPage, totalPage, options).ToArray();
					layoutResult[element] = elementResult;
				}

				result.AddRange(elementResult);
			}

			return result;
		}

		/// <summary>
		///     Generate all pager items.
		/// </summary>
		/// <param name="currentPage">The current page number in the pager.</param>
		/// <param name="totalPage">The total page count of the pager.</param>
		/// <param name="options">The options of the pager.</param>
		/// <returns>A collection of all <see cref="PagerItem" /> generated for the pager.</returns>
		public IEnumerable<PagerItem> GeneratePagerItemsCore(int currentPage, int totalPage, PagerOptions options)
		{
			return GenerateItemsCore(currentPage, totalPage, options.Layout.Elements, options);
		}
	}
}