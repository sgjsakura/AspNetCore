using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	/// <summary>
	///     Add CSS class name to the element conditionally.
	/// </summary>
	[HtmlTargetElement(Attributes = ConditionalClassPrefix + "*")]
	[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
	public class ConditionalClassTagHelper : TagHelper
	{
		/// <summary>
		///     The prefix for all attributes will be storeed in <see cref="ConditionalClasses" /> dictionary. This field is
		///     constant.
		/// </summary>
		[PublicAPI] public const string ConditionalClassPrefix = "asp-conditional-class-";

		/// <summary>
		///     Get or set the dictionary that stores all the conditional class definitions.
		/// </summary>
		[UsedImplicitly(ImplicitUseKindFlags.Assign | ImplicitUseKindFlags.Access)]
		[HtmlAttributeName(DictionaryAttributePrefix = ConditionalClassPrefix)]
		public IDictionary<string, bool> ConditionalClasses { get; set; } = new Dictionary<string, bool>();


		/// <summary>
		///     Synchronously executes the <see cref="TagHelper" /> with the given <paramref name="context" /> and
		///     <paramref name="output" />.
		/// </summary>
		/// <param name="context">Contains information associated with the current HTML tag.</param>
		/// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			// Get all items
			var allItems = (from i in ConditionalClasses
				where i.Value
				select i.Key).ToList();

			// No actions if no items
			if (allItems.Count == 0)
				return;

			// The original class attribute
			var classAttr = output.Attributes["class"];

			// If class attribute exists, merge it
			// Original value of the class attribute
			var originalClass = classAttr?.Value?.ToString();

			// append the original class value if not null
			if (!string.IsNullOrWhiteSpace(originalClass))
				allItems.Add(originalClass);

			// merge to the final class value
			var finalClass = string.Join(" ", allItems);


			// Replace original value
			output.Attributes.SetAttribute("class", finalClass);
		}
	}
}