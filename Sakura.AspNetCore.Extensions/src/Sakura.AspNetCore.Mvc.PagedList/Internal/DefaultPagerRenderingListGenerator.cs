using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sakura.AspNetCore.Mvc.Internal
{
	/// <summary>
	///     Provide default implementation for <see cref="IPagerRenderingListGenerator" />.
	/// </summary>
	public class DefaultPagerRenderingListGenerator : IPagerRenderingListGenerator
	{
		/// <summary>
		///     Build a <see cref="PagerRenderingList" /> according to the <see cref="PagerGenerationContext" /> information.
		/// </summary>
		/// <param name="list">The <see cref="PagerList" /> which contains all pager items.</param>
		/// <param name="context">The <see cref="PagerGenerationContext" /> object which includes all the informations needed.</param>
		/// <returns>A <see cref="PagerRenderingList" /> object which represent as a list to ge displayed in the page.</returns>
		public PagerRenderingList GenerateRenderingList(PagerList list, PagerGenerationContext context)
		{
			return GenerateRenderingListCore(list.Items, context);
		}

		/// <summary>
		///     Determine if the page is in visible page number list.
		/// </summary>
		/// <param name="context">
		///     The <see cref="PagerItemGenerationContext" /> which contains all informations for the pager and
		///     current page.
		/// </param>
		/// <returns>If the current pager item is in the visible list, returns <c>true</c>; otherwise, returns <c>false</c>.</returns>
		private static bool IsPageVisible(PagerItemGenerationContext context)
		{
			var number = context.PagerItem.PageNumber;
			return number <= context.Options.PagerItemsForEndings
			       || number >= context.TotalPage - context.Options.PagerItemsForEndings + 1
			       || number >= context.CurrentPage - context.Options.ExpandPageItemsForCurrentPage
			       || number <= context.CurrentPage + context.Options.ExpandPageItemsForCurrentPage;
		}


		/// <summary>
		///     Get a value that indicates whether the current pager item should be disabled.
		/// </summary>
		/// <param name="context">The pager item generation context.</param>
		/// <returns>If the current pager item should be disabled, returns <c>true</c>; otherwise returns <c>false</c>.</returns>
		private bool ItemShouldBeDisabled(PagerItemGenerationContext context)
		{
			switch (context.PagerItem.ItemType)
			{
				case PagerItemType.First:
				case PagerItemType.Last:
					switch (context.PagerItemOptions.ActiveMode)
					{
						case FirstAndLastPagerItemActiveMode.Always:
							return false;
						case FirstAndLastPagerItemActiveMode.NotInCurrentPage:
							return context.CurrentPage == context.PagerItem.PageNumber;
						case FirstAndLastPagerItemActiveMode.NotInVisiblePageList:
							return IsPageVisible(context);
						default:
							throw new InvalidOperationException(
								$"The value of '{nameof(context.PagerItemOptions.ActiveMode)}' property cannot be a valid enum item for a pager item of type '{context.PagerItem.ItemType}'.");
					}
				case PagerItemType.Next:
					return context.PagerItem.PageNumber == context.TotalPage;
				case PagerItemType.Previous:
					return context.PagerItem.PageNumber == 1;
				default:
					return false;
			}
		}

		/// <summary>
		///     Get the rendering state of the current pager item.
		/// </summary>
		/// <param name="context">The pager item generation context.</param>
		/// <returns>The rendering item of the current pager item.</returns>
		private PagerRenderingItemState GetRenderingItemState(PagerItemGenerationContext context)
		{
			switch (context.PagerItem.ItemType)
			{
				case PagerItemType.First:
				case PagerItemType.Last:
				case PagerItemType.Previous:
				case PagerItemType.Next:
					var disabled = ItemShouldBeDisabled(context);
					if (!disabled)
					{
						return PagerRenderingItemState.Active;
					}
					switch (context.PagerItemOptions.InactiveBehavior)
					{
						case SpecialPagerItemInactiveBehavior.Disable:
							return PagerRenderingItemState.Disabled;
						case SpecialPagerItemInactiveBehavior.Hide:
							return PagerRenderingItemState.Hidden;
						default:
							throw new InvalidOperationException(
								$"The value of '{nameof(context.PagerItemOptions.InactiveBehavior)}' property cannot be a valid enum item for a pager item of type '{context.PagerItem.ItemType}'.");
					}
				case PagerItemType.Current:
					return PagerRenderingItemState.Active;
				default:
					return PagerRenderingItemState.Normal;
			}
		}

		/// <summary>
		///     Generate a <see cref="PagerRenderingItem" /> for the current pager item.
		/// </summary>
		/// <param name="list">The ownner list of the new item.</param>
		/// <param name="context">The generation context.</param>
		/// <returns>The generated <see cref="PagerRenderingItem" /> instance.</returns>
		private PagerRenderingItem GenerateRenderingItem(PagerRenderingList list, PagerItemGenerationContext context)
		{
			return new PagerRenderingItem(list)
			{
				Content = context.PagerItemOptions.Content?.GenerateContent(context),
				Link = context.PagerItemOptions.Link?.GenerateLink(context),
				Settings = new Dictionary<string, string>(context.PagerItemOptions.AdditionalSettings),
				State = GetRenderingItemState(context)
			};
		}

		/// <summary>
		///     Generate a <see cref="PagerRenderingList" /> for a series of <see cref="PagerItem" />.
		/// </summary>
		/// <param name="items">The collection of <see cref="PagerItem" /> which are included in the list.</param>
		/// <param name="context">The generation context.</param>
		/// <returns>The generated <see cref="PagerRenderingList" /> instance.</returns>
		private PagerRenderingList GenerateRenderingListCore(IEnumerable<PagerItem> items, PagerGenerationContext context)
		{
			var optionsCache = new Dictionary<PagerItemType, PagerItemOptions>();

			var renderingItemList = new List<PagerRenderingItem>();

			// Owner
			var result = new PagerRenderingList
			{
				Settings = new Dictionary<string, string>(context.Options.AdditionalSettings)
			};

			foreach (var item in items)
			{
				PagerItemOptions itemOptions;

				// Try to get cached options, or create a new option instance
				if (!optionsCache.TryGetValue(item.ItemType, out itemOptions))
				{
					itemOptions = context.Options.ItemOptions.GetMergedOptionsFor(item);
					optionsCache[item.ItemType] = itemOptions;
				}

				// Generate item context
				var itemContext = new PagerItemGenerationContext(context, item, itemOptions);

				// Generate item and add to list
				var renderingItem = GenerateRenderingItem(result, itemContext);
				renderingItemList.Add(renderingItem);
			}

			// Result
			result.Items = new ReadOnlyCollection<PagerRenderingItem>(renderingItemList);
			return result;
		}
	}
}