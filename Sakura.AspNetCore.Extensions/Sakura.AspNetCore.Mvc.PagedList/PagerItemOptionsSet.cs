using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sakura.AspNetCore.Mvc.Internal;

namespace Sakura.AspNetCore.Mvc;

/// <summary>
///     Include all <see cref="PagerItemOptions" /> for all types of pager items.
/// </summary>
public class PagerItemOptionsSet
{
	/// <summary>
	///     Initialize a new set with all empty settings.
	/// </summary>
	public PagerItemOptionsSet()
	{
		Default = new PagerItemOptions();
		Normal = new PagerItemOptions();
		Current = new PagerItemOptions();
		Omitted = new PagerItemOptions();
		FirstPageButton = new PagerItemOptions();
		LastPageButton = new PagerItemOptions();
		PreviousPageButton = new PagerItemOptions();
		NextPageButton = new PagerItemOptions();
	}

	/// <summary>
	///     Get or set the default setting for all items.
	/// </summary>
	[NotNull]
	public PagerItemOptions Default { get; set; }

	/// <summary>
	///     Get or set the setting for normal number items. This setting will be merged with <see cref="Default" />.
	/// </summary>
	public PagerItemOptions Normal { get; set; }

	/// <summary>
	///     Get or set the setting for current page. This setting will be merged with <see cref="Normal" />.
	/// </summary>
	public PagerItemOptions Current { get; set; }

	/// <summary>
	///     Get or set the setting for omitted page. This setting will be merged with <see cref="Normal" />.
	/// </summary>
	public PagerItemOptions Omitted { get; set; }

	/// <summary>
	///     Get or set the setting for "Go to first page" button. This setting will be merged with <see cref="Default" />.
	/// </summary>
	public PagerItemOptions FirstPageButton { get; set; }

	/// <summary>
	///     Get or set the setting for "Go to last page" button. This setting will be merged with <see cref="Default" />.
	/// </summary>
	public PagerItemOptions LastPageButton { get; set; }

	/// <summary>
	///     Get or set the setting for "Go to next page" button. This setting will be merged with <see cref="Default" />.
	/// </summary>
	public PagerItemOptions NextPageButton { get; set; }

	/// <summary>
	///     Get or set the setting for "Go to previous page" button. This setting will be merged with <see cref="Default" />.
	/// </summary>
	public PagerItemOptions PreviousPageButton { get; set; }

	/// <summary>
	///     Get the actual <see cref="PagerItemOptions" /> for a specified <see cref="PagerLayoutElement" />.
	/// </summary>
	/// <param name="layoutElement">The <see cref="PagerLayoutElement" /> which indicates the role of the pager item.</param>
	/// <returns>A <see cref="PagerItemOptions" /> used for the current <see cref="PagerLayoutElement" />.</returns>
	/// <remarks>
	///     The <see cref="PagerLayoutElement" /> enum type cannot tell the difference between normal number pages,
	///     current page, and omitted pages. In order to get more accurate result, please use
	///     <see cref="GetOptionsFor(PagerItemType)" /> or <see cref="GetOptionsFor(PagerItem)" /> overloads.
	/// </remarks>
	/// <exception cref="ArgumentException">The value of <paramref name="layoutElement" /> is not a valid enum item.</exception>
	public PagerItemOptions GetOptionsFor(PagerLayoutElement layoutElement)
	{
		switch (layoutElement)
		{
			case PagerLayoutElement.Items:
				return Normal;
			case PagerLayoutElement.GoToFirstPageButton:
				return FirstPageButton;
			case PagerLayoutElement.GoToLastPageButton:
				return LastPageButton;
			case PagerLayoutElement.GoToPreviousPageButton:
				return PreviousPageButton;
			case PagerLayoutElement.GoToNextPageButton:
				return NextPageButton;
			default:
				throw new ArgumentException("The value of the argument is not a valid enum item.", nameof(layoutElement));
		}
	}

	/// <summary>
	///     Get the actual <see cref="PagerItemOptions" /> for a specified <see cref="PagerItemType" />.
	/// </summary>
	/// <param name="itemType">The <see cref="PagerItemType" /> which indicates the role of the pager item.</param>
	/// <returns>A <see cref="PagerItemOptions" /> used for the current <see cref="PagerLayoutElement" />.</returns>
	/// <exception cref="ArgumentException">The value of <paramref name="itemType" /> is not a valid enum item.</exception>
	public PagerItemOptions GetOptionsFor(PagerItemType itemType)
	{
		switch (itemType)
		{
			case PagerItemType.Normal:
				return Normal;
			case PagerItemType.First:
				return FirstPageButton;
			case PagerItemType.Last:
				return LastPageButton;
			case PagerItemType.Previous:
				return PreviousPageButton;
			case PagerItemType.Next:
				return NextPageButton;
			case PagerItemType.Current:
				return Current;
			case PagerItemType.Omitted:
				return Omitted;
			default:
				throw new ArgumentException("The value of the argument is not a valid enum item.", nameof(itemType));
		}
	}

	/// <summary>
	///     Get the actual <see cref="PagerItemOptions" /> for a specified <see cref="PagerItemType" />.
	/// </summary>
	/// <param name="item">The actual <see cref="PagerItem" /> instance.</param>
	/// <returns>A <see cref="PagerItemOptions" /> used for the current <see cref="PagerLayoutElement" />.</returns>
	/// <exception cref="ArgumentNullException">The value of <paramref name="item" /> is <c>null</c>.</exception>
	public PagerItemOptions GetOptionsFor([NotNull] PagerItem item)
	{
		if (item == null)
			throw new ArgumentNullException(nameof(item));

		return GetOptionsFor(item.ItemType);
	}

	/// <summary>
	///     Get the base item type of a <see cref="PagerItemType" />. If no base itemType is appliable. This method will return
	///     <c>null</c>.
	/// </summary>
	/// <param name="itemType">A <see cref="PagerItemType" /> to be getting the base type.</param>
	/// <returns>The base type of <paramref name="itemType" />, or <c>null</c> if no base itemType is appliable.</returns>
	public static PagerItemType? GetBaseItemType(PagerItemType itemType)
	{
		switch (itemType)
		{
			case PagerItemType.Current:
			case PagerItemType.Omitted:
				return PagerItemType.Normal;
			default:
				return null;
		}
	}

	/// <summary>
	///     Get the final merged <see cref="PagerItemOptions" /> for a specified <see cref="PagerItemType" />.
	/// </summary>
	/// <param name="itemType">The <see cref="PagerItemType" /> which indicates the role of the pager item.</param>
	/// <returns>The final merged <see cref="PagerItemOptions" /> instance.</returns>
	[NotNull]
	public PagerItemOptions GetMergedOptionsFor(PagerItemType itemType)
	{
		// Stack used to LIFO operations
		var optionsStack = new Stack<PagerItemOptions>();

		// Recursively get base item
		PagerItemType? current = itemType;
		while (current != null)
		{
			optionsStack.Push(GetOptionsFor(itemType));
			current = GetBaseItemType(current.Value);
		}

		// Put default item
		optionsStack.Push(Default);

		// Merge all items
		return PagerItemOptions.MergeAll(optionsStack);
	}

	/// <summary>
	///     Get the final merged <see cref="PagerItemOptions" /> for a specified <see cref="PagerItem" />.
	/// </summary>
	/// <param name="item">The actual <see cref="PagerItem" /> instance.</param>
	/// <returns>The final merged <see cref="PagerItemOptions" /> instance.</returns>
	[NotNull]
	public PagerItemOptions GetMergedOptionsFor([NotNull] PagerItem item)
	{
		if (item == null)
			throw new ArgumentNullException(nameof(item));

		return GetMergedOptionsFor(item.ItemType);
	}

	/// <summary>
	///     Create a deep clone for this object.
	/// </summary>
	/// <returns>A clone of this object.</returns>
	public PagerItemOptionsSet Clone()
	{
		var result = (PagerItemOptionsSet) MemberwiseClone();

		result.Default = Default.Clone();
		result.Current = Current.Clone();
		result.Normal = Normal.Clone();
		result.Omitted = Omitted.Clone();
		result.FirstPageButton = FirstPageButton.Clone();
		result.LastPageButton = LastPageButton.Clone();
		result.PreviousPageButton = PreviousPageButton.Clone();
		result.NextPageButton = NextPageButton.Clone();

		return result;
	}
}