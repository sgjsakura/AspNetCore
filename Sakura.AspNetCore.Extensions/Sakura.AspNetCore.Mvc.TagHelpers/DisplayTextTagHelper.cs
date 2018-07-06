using System;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	/// <summary>
	///     Provide general ability for extract data annotation text from property definition for a model expression.
	/// </summary>
	/// <inheritdoc />
	[HtmlTargetElement(Attributes = TargetElementName)]
	public class DisplayTextTagHelper : TagHelper
	{
		public DisplayTextTagHelper(IServiceProvider serviceProvider)
		{
			StringLocalizerFactory = serviceProvider.GetService<IStringLocalizerFactory>();
		}

		#region Services

		/// <summary>
		///     Get the service used to create <see cref="IStringLocalizer" /> services.
		/// </summary>
		private IStringLocalizerFactory StringLocalizerFactory { get; }

		#endregion

		/// <summary>
		///     Get or set the related model expression for the text element.
		/// </summary>
		[HtmlAttributeName(ForHtmlAttributeName)]
		[UsedImplicitly(ImplicitUseKindFlags.Assign)]
		public ModelExpression For { get; set; }

		/// <summary>
		///     Get or set the text source used to generated the inner text.
		/// </summary>
		[HtmlAttributeName(TextSourceHtmlAttributeName)]
		[UsedImplicitly(ImplicitUseKindFlags.Assign)]
		public TextSource TextSource { get; set; } = TextSource.MemberNameOnly;

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			if (output.TagMode != TagMode.SelfClosing)
				throw new InvalidOperationException(
					$"The '{TargetElementName}' element can only use the self closing mode.");


			// get property definition
			var member = For.Metadata.ContainerType.GetProperty(For.Metadata.PropertyName,
				BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance);

			// get the localizer for the container type.
			var localizer = StringLocalizerFactory?.Create(For.Metadata.ContainerType);

			var memberText = member.GetTextForMember(TextSource);
			var realText = localizer.TryGetLocalizedText(memberText);

			output.TagName = null;
			output.Content.SetContent(realText);
		}

		#region TagHelper Constants

		/// <summary>
		///     The HTML target element name for this tag helper. This field is constant.
		/// </summary>
		public const string TargetElementName = "display-text";

		/// <summary>
		///     The HTML attribute related with the <see cref="For" /> property. This field is constant.
		/// </summary>
		public const string ForHtmlAttributeName = "for";

		/// <summary>
		///     The HTML attribute related with the <see cref="TextSource" /> property. This field is constant.
		/// </summary>
		public const string TextSourceHtmlAttributeName = "text-source";

		#endregion
	}
}