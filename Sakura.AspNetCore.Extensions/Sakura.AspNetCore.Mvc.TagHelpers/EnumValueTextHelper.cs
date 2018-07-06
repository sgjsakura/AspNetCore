using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	/// <summary>
	///     Provide general ability for extra data annotation text from its enum item definition for a enum value.
	/// </summary>
	/// <inheritdoc />
	[HtmlTargetElement(TargetElementName)]
	public class EnumValueTextHelper : TagHelper
	{
		#region Constructor

		public EnumValueTextHelper(IServiceProvider serviceProvider)
		{
			StringLocalizerFactory = serviceProvider.GetService<IStringLocalizerFactory>();
		}

		#endregion

		#region Services

		/// <summary>
		///     Get the service used to create <see cref="IStringLocalizer" /> services.
		/// </summary>
		private IStringLocalizerFactory StringLocalizerFactory { get; }

		#endregion

		/// <summary>
		///     Get or set the related model expression for the text element.
		/// </summary>
		[HtmlAttributeName(ValueHtmlAttributeName)]
		[UsedImplicitly(ImplicitUseKindFlags.Assign)]
		public Enum Value { get; set; }

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

			if (Value == null && For == null)
			{
				throw new InvalidOperationException(
					$"Either the '{ValueHtmlAttributeName}' attribute or the '{ForHtmlAttributeName}' attribute should be specified.");
			}

			if (Value != null && For != null)
			{
				throw new InvalidOperationException(
					$"Only one of the '{ValueHtmlAttributeName}' attribute and the '{ForHtmlAttributeName}' attribute can be specified.");
			}

			var realValue = Value ?? For.Model;

			if (!(realValue is Enum enumValue))
			{
				throw new InvalidOperationException(
					$"Your specified value from the '{ValueHtmlAttributeName} attribute or the '{ForHtmlAttributeName}' attribute is not a valid enum value.");
			}

			// get property definition
			var member = enumValue.GetMember();

			// get the localizer for the container type.
			var localizer = StringLocalizerFactory?.Create(member.DeclaringType);

			var memberText = member.GetTextForMember(TextSource);
			var realText = localizer.TryGetLocalizedText(memberText);

			output.TagName = null;
			output.Content.SetContent(realText);
		}

		#region TagHelper Constants

		/// <summary>
		///     The Target HTML element name. This field is constant.
		/// </summary>
		public const string TargetElementName = "enum-item-display-text";

		/// <summary>
		///     The HTML attribute related with the <see cref="Value" /> property. This field is constant.
		/// </summary>
		public const string ValueHtmlAttributeName = "value";

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