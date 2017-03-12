using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

// ReSharper disable MustUseReturnValue

namespace Sakura.AspNetCore.Mvc.Internal
{
	/// <summary>
	///     An HTML code generator using bootstrap theme for paged list pagers.
	/// </summary>
	[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
	public class BootstrapPagerHtmlGenerator : IPagerHtmlGenerator
	{
		/// <summary>
		///     Generate the entire HTML content for a pager renderling list.
		/// </summary>
		/// <param name="list">The rendering list to be generating.</param>
		/// <param name="context">The pager generation context.</param>
		/// <returns>The entire HTML content for the generated pager.</returns>
		public IHtmlContent GeneratePager(PagerRenderingList list, PagerGenerationContext context)
		{
			return GeneratePagerCore(list, context.GenerationMode == PagerGenerationMode.Full);
		}

		#region Core Methods for Pager Generation

		/// <summary>
		///     Determine if a <see cref="PagerRenderingItem" /> has a valid link.
		/// </summary>
		/// <param name="item">The <see cref="PagerRenderingItem" /> object.</param>
		/// <returns>
		///     If the <paramref name="item" /> has a valid link and is not disabled, returns <c>true</c>; otherwise, return
		///     <c>false</c>.
		/// </returns>
		private static bool CanLink(PagerRenderingItem item)
		{
			return item.State != PagerRenderingItemState.Disabled && !string.IsNullOrEmpty(item.Link);
		}

		/// <summary>
		///     Append additional HTML attributes according to the setting.
		/// </summary>
		/// <param name="tag">The tag to appending attributes.</param>
		/// <param name="settingDictionary">The dictionary which store all the settings.</param>
		/// <param name="attributePrefix">
		///     The perfix for attributes used to search valid attributes in
		///     <paramref name="settingDictionary" />.
		/// </param>
		private static void AppendAdditionalAttributes(TagBuilder tag, IDictionary<string, string> settingDictionary,
			string attributePrefix)
		{
			foreach (var i in settingDictionary)
				if (i.Key.StartsWith(attributePrefix))
					tag.Attributes[i.Key.Substring(attributePrefix.Length)] = i.Value;
		}

		/// <summary>
		///     The core method to generate the HTML content.
		/// </summary>
		/// <param name="list">The pager list to generating the content.</param>
		/// <param name="generateContainer">Whether a container element should be generated.</param>
		/// <returns>The final <see cref="IHtmlContent" />.</returns>
		protected static IHtmlContent GeneratePagerCore(PagerRenderingList list, bool generateContainer)
		{
			if (list == null)
				throw new ArgumentNullException(nameof(list));

			var content = GeneratePagerItems(list.Items);

			// No container
			if (!generateContainer)
				return content;

			// With container
			var container = GenerateContainer(list);
			container.InnerHtml.SetHtmlContent(content);

			return container;
		}

		/// <summary>
		///     Generate the container tag.
		/// </summary>
		/// <returns>The container tag.</returns>
		protected static TagBuilder GenerateContainer(PagerRenderingList list)
		{
			var tag = new TagBuilder("ul");
			tag.AddCssClass("pagination"); // core CSS class for pagination

			AppendAdditionalAttributes(tag, list.Settings, ListAttributeSettingKeyPrefix);

			return tag;
		}

		/// <summary>
		///     Generate the html content for a series of pager items.
		/// </summary>
		/// <param name="items">The collection of <see cref="PagerRenderingItem" /> to generate the content.</param>
		/// <returns>The final <see cref="IHtmlContent" />.</returns>
		protected static IHtmlContent GeneratePagerItems(IEnumerable<PagerRenderingItem> items)
		{
			if (items == null)
				throw new ArgumentNullException(nameof(items));

			var content = new DefaultTagHelperContent();

			foreach (var i in items)
				content.AppendHtml(GeneratePagerItem(i));

			return content;
		}

		/// <summary>
		///     Generate the html content for a pager item.
		/// </summary>
		/// <param name="item">The pager item for rendering.</param>
		/// <returns>The generated <see cref="IHtmlContent" /> for the <paramref name="item" />.</returns>
		protected static IHtmlContent GeneratePagerItem(PagerRenderingItem item)
		{
			if (item == null)
				throw new ArgumentNullException(nameof(item));

			// Container
			var itemTag = new TagBuilder("li");

			TagBuilder linkTag;

			// Generate <a> or <span> according to its state
			if (item.State == PagerRenderingItemState.Active)
			{
				itemTag.AddCssClass("active"); // active CSS class
				linkTag = new TagBuilder("span");
			}
			else if (CanLink(item))
			{
				linkTag = new TagBuilder("a");
				linkTag.Attributes["href"] = item.Link;
			}
			else
			{
				itemTag.AddCssClass("disabled"); // Disabled core CSS class
				linkTag = new TagBuilder("span");
			}

			// Add Bootstrap v4 classes, this is not confict with bootstrap v3. However user may choose to diable it manually.
			if (!string.Equals(item.Settings.GetValueOfDefault("disble-bootstrap-v4-class"), "true",
				StringComparison.OrdinalIgnoreCase))
			{
				itemTag.AddCssClass("page-item");
				linkTag.AddCssClass("page-link");
			}

			// Append father settings
			AppendAdditionalAttributes(itemTag, item.List.Settings, ItemAttributeSettingKeyPrefix);
			AppendAdditionalAttributes(linkTag, item.List.Settings, LinkAttributeSettingKeyPrefix);

			// Append all additional attributes
			AppendAdditionalAttributes(itemTag, item.Settings, ItemAttributeSettingKeyPrefix);
			AppendAdditionalAttributes(linkTag, item.Settings, LinkAttributeSettingKeyPrefix);

			// Set real content and merge elements
			linkTag.InnerHtml.SetHtmlContent(item.Content);
			itemTag.InnerHtml.SetHtmlContent(linkTag);
			return itemTag;
		}

		#endregion

		#region Constants

		/// <summary>
		///     The setting prefix used to define additional container (&lt;li&gt;) HTML attributes. This field is constant.
		/// </summary>
		[PublicAPI] public const string ItemAttributeSettingKeyPrefix = "item-attr-";

		/// <summary>
		///     The setting prefix used to define additional link (&lt;a&gt; or &lt;span&gt;) HTML attributes. This field is
		///     constant.
		/// </summary>
		[PublicAPI] public const string LinkAttributeSettingKeyPrefix = "link-attr-";

		/// <summary>
		///     The setting prefix used to define additional list (&lt;ul&gt;) HTML attributes. This field is constant.
		/// </summary>
		[PublicAPI] public const string ListAttributeSettingKeyPrefix = "list-attr-";

		#endregion
	}
}