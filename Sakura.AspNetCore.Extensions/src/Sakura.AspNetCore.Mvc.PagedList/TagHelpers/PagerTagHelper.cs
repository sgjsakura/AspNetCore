using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sakura.AspNetCore.Mvc.Internal;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	/// <summary>
	///     Generate a pager in HTML page.
	/// </summary>
	[HtmlTargetElement(HtmlTagName)]
	[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
	public class PagerTagHelper : TagHelper
	{
		#region Constuctors

		/// <summary>
		///     Initialize a new instance of <see cref="PagerTagHelper" />.
		/// </summary>
		/// <param name="defaultOptions">Registered default options value.</param>
		public PagerTagHelper(IOptions<PagerOptions> defaultOptions)
		{
			DefaultOptions = defaultOptions.Value;
		}

		#endregion

		#region Registered Service

		/// <summary>
		///     Get the default options registered in the application.
		/// </summary>
		private PagerOptions DefaultOptions { get; }

		#endregion

		/// <summary>
		///     Get or set the current view context.
		/// </summary>
		[HtmlAttributeNotBound]
		[ViewContext]
		public ViewContext ViewContext { get; set; }

		/// <summary>
		///     Try to get the page information from the current context.
		/// </summary>
		/// <param name="context">The <see cref="TagHelperContext" /> object.</param>
		/// <param name="currentPage">Return the current page number.</param>
		/// <param name="totalPage">Return the total page count.</param>
		private void GetPagingInfo(TagHelperContext context, out int currentPage, out int totalPage)
		{
			var hasSource = context.AllAttributes.ContainsName(SourceAttributeName);
			var hasCurrentPage = context.AllAttributes.ContainsName(CurrentPageAttributeName);
			var hasTotalPage = context.AllAttributes.ContainsName(TotalPageAttributeName);

			if (hasTotalPage || hasCurrentPage)
			{
				// Not both set
				if (!hasTotalPage || !hasCurrentPage)
				{
					throw new InvalidOperationException(
						$"The '{TotalPageAttributeName}' and '{CurrentPageAttributeName}' attribute must both be set on the element.");
				}

				// Cannot combined with source
				if (hasSource)
				{
					throw new InvalidOperationException(
						$"The '{SourceAttributeName}' attribute cannot be used together with either '{TotalPageAttributeName}' or '{CurrentPageAttributeName}' attribute.");
				}

				// Range check
				if (TotalPage <= 0)
				{
					throw new InvalidOperationException($"The value of '{TotalPageAttributeName}' attribute must be positive integer.");
				}

				if (CurrentPage <= 0 || CurrentPage > TotalPage)
				{
					throw new InvalidOperationException(
						$"The value of '{CurrentPageAttributeName}' attribute must between 1 and the value of '{TotalPageAttributeName}' attribute.");
				}


				// Result
				currentPage = CurrentPage;
				totalPage = TotalPage;
			}
			else
			{
				IPagedList realSource;

				if (hasSource)
				{
					if (Source == null)
					{
						throw new InvalidOperationException(
							$"The pager source specified with '{SourceAttributeName}' attribute cannot be null.");
					}

					realSource = Source;
				}
				else
				{
					realSource = ViewContext.ViewData.Model as IPagedList;

					if (realSource == null)
					{
						throw new InvalidOperationException(
							$"The model of current view is either null or an object that cannot be converted to '{typeof(IPagedList).AssemblyQualifiedName}' type.");
					}
				}

				currentPage = realSource.PageIndex;
				totalPage = realSource.TotalPage;
			}
		}

		/// <summary>
		///     Get the real <see cref="IPagerGenerator" /> used in this tag helper/
		/// </summary>
		/// <param name="context">The tag helper context.</param>
		/// <returns>The used <see cref="IPagerGenerator" /> instance.</returns>
		/// <exception cref="InvalidOperationException">No valid <see cref="IPagerGenerator" /> is specified.</exception>
		private IPagerGenerator GetRealGenerator(TagHelperContext context)
		{
			if (context.AllAttributes.ContainsName(GeneratorAttributeName))
			{
				if (Generator == null)
				{
					throw new InvalidOperationException(
						$"The HTML generator specified using '{GeneratorAttributeName}' attribute cannot be null.");
				}

				return Generator;
			}

			// Get System registered service.
			var registeredGenerator = ViewContext.HttpContext.RequestServices.GetService<IPagerGenerator>();

			if (registeredGenerator == null)
			{
				throw new InvalidOperationException(
					$"You must provide a pager HTML generator service object to generate the pager either through the '{GenerationModeAttributeName}' attribute or register it at application startup time.");
			}

			return registeredGenerator;
		}

		/// <summary>
		///     Merge options set by the tag into the original options.
		/// </summary>
		/// <param name="original">The original options.</param>
		/// <param name="context">The tag helper context</param>
		/// <returns>
		///     A new <see cref="PagerOptions" /> instance with copies the original settings and merge any shortcut properties
		///     on the tag.
		/// </returns>
		private PagerOptions MergeShortcutProperties(PagerOptions original, TagHelperContext context)
		{
			var result = original.Clone();

			// Merge generators
			if (context.AllAttributes.ContainsName(ItemDefaultContentGeneratorAttributeName))
			{
				result.ItemOptions.Default.Content = ItemDefaultContentGenerator;
			}

			if (context.AllAttributes.ContainsName(ItemDefaultLinkGeneratorAttributeName))
			{
				result.ItemOptions.Default.Link = ItemDefaultLinkGenerator;
			}

			// Merge settings
			foreach (var i in Settings)
			{
				result.AdditionalSettings[i.Key] = i.Value;
			}

			return result;
		}

		/// <summary>
		///     Get the real <see cref="PagerOptions" /> used in this tag helper/
		/// </summary>
		/// <param name="context">The tag helper context.</param>
		/// <returns>The used <see cref="PagerOptions" /> instance.</returns>
		/// <exception cref="InvalidOperationException">No valid <see cref="PagerOptions" /> is specified.</exception>
		private PagerOptions GetRealOptions(TagHelperContext context)
		{
			PagerOptions result;

			if (context.AllAttributes.ContainsName(OptionsAttributeName))
			{
				if (Options == null)
				{
					throw new InvalidOperationException(
						$"The pager options specified using '{OptionsAttributeName}' attribute cannot be null.");
				}

				result = Options;
			}
			else if (DefaultOptions == null)
			{
				throw new InvalidOperationException(
					$"You must provide a pager options either through the '{OptionsAttributeName}' attribute or register it at application startup time.");
			}
			else
			{
				result = DefaultOptions;
			}

			// Merge result and return
			return MergeShortcutProperties(result, context);
		}


		/// <summary>
		///     Handle the options to avoid it is in an invalid state.
		/// </summary>
		/// <param name="options">The options to be checked.</param>
		/// <exception cref="ArgumentNullException">The <paramref name="options" /> is <c>null</c>.</exception>
		/// <exception cref="InvalidOperationException">The <paramref name="options" />is in an invalid state.</exception>
		private void CheckOptions([NotNull] PagerOptions options)
		{
			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			if (options.Layout == null)
			{
				throw new InvalidOperationException(
					$"The value of the '{nameof(PagerOptions.Layout)}' property in the pager options cannot be null.");
			}

			if (options.PagerItemsForEndings < 0)
			{
				throw new InvalidOperationException(
					$"The value of the '{nameof(PagerOptions.PagerItemsForEndings)}' property in the pager options cannot be negative.");
			}

			if (options.ExpandPageItemsForCurrentPage < 0)
			{
				throw new InvalidOperationException(
					$"The value of the '{nameof(PagerOptions.ExpandPageItemsForCurrentPage)}' property in the pager options cannot be negative.");
			}

			if (options.Layout == null)
			{
				throw new InvalidOperationException(
					$"The value of the '{nameof(PagerOptions.Layout)}' property cannot be null.");
			}
		}

		/// <summary>
		///     Synchronously executes the <see cref="TagHelper" /> with the given <paramref name="context" /> and
		///     <paramref name="output" />.
		/// </summary>
		/// <param name="context">Contains information associated with the current HTML tag.</param>
		/// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			// State check
			if (output.TagMode != TagMode.SelfClosing)
			{
				throw new InvalidOperationException($"The '{HtmlTagName}' tag must use self closing mode.");
			}

			// Get information and build up context
			var generator = GetRealGenerator(context);
			var options = GetRealOptions(context);
			CheckOptions(options);

			int currentPage, totalPage;
			GetPagingInfo(context, out currentPage, out totalPage);

			var pagerContext = new PagerGenerationContext(currentPage, totalPage, options, ViewContext, GenerationMode);

			// Generate result
			var result = generator.GeneratePager(pagerContext);

			// Disable default element output
			output.SuppressOutput();

			// Append pager content
			output.PostElement.AppendHtml(result);
		}

		#region Html Constants

		/// <summary>
		///     Get the HTML tag name associated with this tag helper. This field is constant.
		/// </summary>
		[PublicAPI] public const string HtmlTagName = "pager";

		/// <summary>
		///     Get the HTML attribute name for <see cref="Options" /> property. This field is constant.
		/// </summary>
		[PublicAPI] public const string OptionsAttributeName = "options";

		/// <summary>
		///     Get the HTML attribute name for <see cref="Source" /> property. This field is constant.
		/// </summary>
		[PublicAPI] public const string SourceAttributeName = "source";

		/// <summary>
		///     Get the HTML attribute name for <see cref="CurrentPage" /> property. This field is constant.
		/// </summary>
		[PublicAPI] public const string CurrentPageAttributeName = "current-page";

		/// <summary>
		///     Get the HTML attribute name for <see cref="TotalPage" /> property. This field is constant.
		/// </summary>
		[PublicAPI] public const string TotalPageAttributeName = "total-page";

		/// <summary>
		///     Get the HTML attribute name for <see cref="Generator" /> property. This field is constant.
		/// </summary>
		[PublicAPI] public const string GeneratorAttributeName = "generator";

		/// <summary>
		///     Get the HTML attribute name for <see cref="GenerationMode" /> property. This field is constant.
		/// </summary>
		[PublicAPI] public const string GenerationModeAttributeName = "generation-mode";

		/// <summary>
		///     Get the HTML attribute perfix for <see cref="Settings" /> property. This field is constant.
		/// </summary>
		[PublicAPI] public const string SettingsAttributePerfix = "setting-";

		/// <summary>
		///     Get the HTML attribute name for <see cref="Settings" /> property. This field is constant.
		/// </summary>
		[PublicAPI] public const string SettingsAttributeName = "settings";

		/// <summary>
		///     Get the HTML attribute name for <see cref="ItemDefaultContentGenerator" /> property. This field is constant.
		/// </summary>
		[PublicAPI] public const string ItemDefaultContentGeneratorAttributeName = "item-default-content";

		/// <summary>
		///     Get the HTML attribute name for <see cref="ItemDefaultLinkGenerator" /> property. This field is constant.
		/// </summary>
		[PublicAPI] public const string ItemDefaultLinkGeneratorAttributeName = "item-default-link";

		#endregion

		#region Html Bounded Properties

		#region Core Properties

		/// <summary>
		///     Get or set the options for the pager.
		/// </summary>
		[HtmlAttributeName(OptionsAttributeName)]
		public PagerOptions Options { get; set; }

		/// <summary>
		///     Get or set the source of the pager. This property cannot be used together with <see cref="CurrentPage" /> or
		///     <see cref="TotalPage" />.
		/// </summary>
		[HtmlAttributeName(SourceAttributeName)]
		public IPagedList Source { get; set; }

		/// <summary>
		///     Get or set the current page number or the pager. This property must use together with <see cref="TotalPage" />, and
		///     <see cref="Source" /> property must keep unset.
		/// </summary>
		[HtmlAttributeName(CurrentPageAttributeName)]
		public int CurrentPage { get; set; }

		/// <summary>
		///     Get or set the total page count or the pager. This property must use together with <see cref="CurrentPage" />, and
		///     <see cref="Source" /> property must keep unset.
		/// </summary>
		[HtmlAttributeName(TotalPageAttributeName)]
		public int TotalPage { get; set; }

		/// <summary>
		///     Get or set the <see cref="IPagerGenerator" /> used to generate the pager.
		/// </summary>
		[HtmlAttributeName(GeneratorAttributeName)]
		public IPagerGenerator Generator { get; set; }


		/// <summary>
		///     Get or set the generation mode for the pager.
		/// </summary>
		[HtmlAttributeName(GenerationModeAttributeName)]
		public PagerGenerationMode GenerationMode { get; set; }

		#endregion

		#region Shortcut Properties

		/// <summary>
		///     Get or set the additional settings for the current pager. This property is a shortcut property for setting the
		///     <see cref="PagerOptions.AdditionalSettings" /> for the <see cref="Options" />.
		/// </summary>
		[HtmlAttributeName(SettingsAttributeName, DictionaryAttributePrefix = SettingsAttributePerfix)]
		public Dictionary<string, string> Settings { get; set; } = new Dictionary<string, string>();

		/// <summary>
		///     Get or set the default item content generator for the current pager. This property is a shortcut property for the
		///     <see cref="Options" />.
		/// </summary>
		[HtmlAttributeName(ItemDefaultContentGeneratorAttributeName)]
		public IPagerItemContentGenerator ItemDefaultContentGenerator { get; set; }

		/// <summary>
		///     Get or set the item link generator for the current pager. This property is a shortcut property for the
		///     <see cref="Options" />.
		/// </summary>
		[HtmlAttributeName(ItemDefaultLinkGeneratorAttributeName)]
		public IPagerItemLinkGenerator ItemDefaultLinkGenerator { get; set; }

		#endregion

		#endregion
	}
}