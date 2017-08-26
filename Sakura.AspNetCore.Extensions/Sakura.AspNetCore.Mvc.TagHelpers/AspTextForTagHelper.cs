using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	/// <inheritdoc />
	[HtmlTargetElement(Attributes = TextForHtmlAttributeName)]
	public class AspTextForTagHelper : TagHelper
	{
		public AspTextForTagHelper(IStringLocalizerFactory stringLocalizerFactory)
		{
			StringLocalizerFactory = stringLocalizerFactory;
		}

		#region TagHelper Constants

		/// <summary>
		/// The HTML attribute related with the <see cref="TextFor"/> property. This field is constant.
		/// </summary>
		public const string TextForHtmlAttributeName = "asp-text-for";

		/// <summary>
		/// The HTML attribute related with the <see cref="TextSource"/> property. This field is constant.
		/// </summary>
		public const string TextSourceHtmlAttributeName = "asp-text-source";

		#endregion

		#region Services

		/// <summary>
		/// Get the service used to create <see cref="IStringLocalizer"/> services.
		/// </summary>
		private IStringLocalizerFactory StringLocalizerFactory { get; }

		#endregion

		/// <summary>
		/// Get or set the related model expression for the text element.
		/// </summary>
		[HtmlAttributeName(TextForHtmlAttributeName)]
		[UsedImplicitly(ImplicitUseKindFlags.Assign)]
		public ModelExpression TextFor { get; set; }

		/// <summary>
		/// Get or set the text source used to generated the inner text.
		/// </summary>
		[HtmlAttributeName(TextSourceHtmlAttributeName)]
		[UsedImplicitly(ImplicitUseKindFlags.Assign)]
		public TextSource TextSource { get; set; } = TextSource.MemberNameOnly;

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			if (TextFor.Metadata.MetadataKind != ModelMetadataKind.Property)
			{
				throw new InvalidOperationException($"The '{TextForHtmlAttributeName}' attribute is only valid to bind with a property.");
			}

			if (output.TagMode != TagMode.StartTagAndEndTag)
			{
				throw new InvalidOperationException($"The '{TextForHtmlAttributeName}' attribute can be only applied on elements that contains both start and end tags.");
			}

			if (!output.Content.IsEmptyOrWhiteSpace)
			{
				throw new InvalidOperationException($"You cannot used the '{TextForHtmlAttributeName}' attribute when the tag already has any real content in it.");
			}


			// get property definition
			var member = TextFor.Metadata.ContainerType.GetProperty(TextFor.Metadata.PropertyName, BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance);

			// get the localizer for the container type.
			var localizer = StringLocalizerFactory.Create(TextFor.Metadata.ContainerType);

			var memberText = member.GetTextForMember(TextSource);
			var realText = localizer.TryGetLocalizedText(memberText);

			output.Content.SetContent(realText);
		}
	}
}
