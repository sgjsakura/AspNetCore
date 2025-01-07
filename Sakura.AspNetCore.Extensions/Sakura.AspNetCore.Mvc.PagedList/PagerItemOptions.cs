using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Sakura.AspNetCore.Mvc;

/// <summary>
///     Control the generation process for a single pager item.
/// </summary>
public class PagerItemOptions
{
	/// <summary>
	///     Get or set a generator that used to generate the content of the pager item.
	/// </summary>
	public IPagerItemContentGenerator Content { get; set; }

	/// <summary>
	///     Get or set a generator that used to generate the link of the pager item.
	/// </summary>
	public IPagerItemLinkGenerator Link { get; set; }

	/// <summary>
	///     Get or set the behavior for special pager items (non-standard number items) when it is in active. This property is
	///     not used for number items.
	/// </summary>
	public SpecialPagerItemInactiveBehavior? InactiveBehavior { get; set; }

	/// <summary>
	///     Get or set the active mode for the first and last button. This property is only used for first and last items.
	/// </summary>
	public FirstAndLastPagerItemActiveMode? ActiveMode { get; set; }

	/// <summary>
	///     Get or set additional information for the pager item.
	/// </summary>
	public IDictionary<string, string> AdditionalSettings { get; private set; } = new Dictionary<string, string>();

	/// <summary>
	///     Create a clone for the current object.
	/// </summary>
	/// <returns>A clone for current object.</returns>
	public PagerItemOptions Clone()
	{
		// Base clone
		var result = (PagerItemOptions) MemberwiseClone();
		// Deep clone
		result.AdditionalSettings = new Dictionary<string, string>(AdditionalSettings);

		return result;
	}

	/// <summary>
	///     Merge a <see cref="PagerItemOptions" /> into another base instace.
	/// </summary>
	/// <param name="baseOptions">The base instance to be merged.</param>
	/// <param name="additionalOptions">
	///     The additional instance, in which the non-null values will merge into the base
	///     instance.
	/// </param>
	/// <returns>A new <see cref="PagerItemOptions" /> to represent as the merging result.</returns>
	/// <remarks>This method will not change either <paramref name="baseOptions" /> or <paramref name="additionalOptions" />.</remarks>
	[Pure]
	[NotNull]
	public static PagerItemOptions Merge(PagerItemOptions? baseOptions,
		PagerItemOptions? additionalOptions)
	{
		// If base options is not null, use its clone as base; otherwise, use a new empty item as base.
		var result = baseOptions?.Clone() ?? new PagerItemOptions();

		// Return base item if no additional options are 
		if (additionalOptions == null)
			return result;

		// Merge attributes
		result.Content = additionalOptions.Content ?? result.Content;
		result.Link = additionalOptions.Link ?? result.Link;
		result.ActiveMode = additionalOptions.ActiveMode ?? result.ActiveMode;
		result.InactiveBehavior = additionalOptions.InactiveBehavior ?? result.InactiveBehavior;

		// Merge settings
		foreach (var i in additionalOptions.AdditionalSettings)
			result.AdditionalSettings[i.Key] = i.Value;

		return result;
	}

	/// <summary>
	///     Merge all <see cref="PagerItemOptions" /> and get a final result.
	/// </summary>
	/// <param name="options">
	///     A collection of all pager item options to be merged.The items at the leader will be merged
	///     earlier.
	/// </param>
	/// <returns>The final merged result.</returns>
	[NotNull]
	public static PagerItemOptions MergeAll(params PagerItemOptions?[] options)
	{
		return MergeAll((IEnumerable<PagerItemOptions>) options);
	}

	/// <summary>
	///     Merge all <see cref="PagerItemOptions" /> and get a final result.
	/// </summary>
	/// <param name="options">
	///     A collection of all pager item options to be merged.The items at the leader will be merged
	///     earlier.
	/// </param>
	/// <returns>The final merged result.</returns>
	[NotNull]
	public static PagerItemOptions MergeAll(IEnumerable<PagerItemOptions?> options)
	{
		return options.Aggregate(new PagerItemOptions(), Merge);
	}
}