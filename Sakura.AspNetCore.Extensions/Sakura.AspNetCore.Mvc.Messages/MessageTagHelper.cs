using JetBrains.Annotations;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sakura.AspNetCore.Mvc
{
	/// <summary>
	///     Provide tag helper service for messages.
	/// </summary>
	[HtmlTargetElement("div", Attributes = MessageListAttributeName)]
	public class MessageTagHelper : TagHelper
	{
		/// <summary>
		///     Initialize a instance with required services.
		/// </summary>
		/// <param name="messageAccessor">The HTML generator service.</param>
		/// <param name="generator">The message accessor service.</param>
		[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
		public MessageTagHelper(IOperationMessageAccessor messageAccessor, IOperationMessageHtmlGenerator generator)
		{
			MessageAccessor = messageAccessor;
			Generator = generator;
		}

		/// <summary>
		///     Get the message accessor service.
		/// </summary>
		[PublicAPI]
		protected IOperationMessageAccessor MessageAccessor { get; }

		/// <summary>
		///     Get the HTML generator service.
		/// </summary>
		[PublicAPI]
		protected IOperationMessageHtmlGenerator Generator { get; }

		/// <summary>
		///     Synchronously executes the <see cref="TagHelper" /> with the given <paramref name="context" /> and
		///     <paramref name="output" />.
		/// </summary>
		/// <param name="context">Contains information associated with the current HTML tag.</param>
		/// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			// Get the message list
			var messages = MessageAccessor.Messages;

			// Generate the output
			var tag = Generator.GenerateList(messages, ListStyle, UseTwoLine);

			// Merge result
			output.PostContent.AppendHtml(tag);
		}

		#region HTML bound fields and properties

		/// <summary>
		///     Get the attribute name for <see cref="ListStyle" /> property. This field is constant.
		/// </summary>
		[PublicAPI] public const string MessageListAttributeName = "asp-message-list";

		/// <summary>
		///     Get the attribute name for <see cref="UseTwoLine" /> property. This field is constant.
		/// </summary>
		[PublicAPI] public const string MessageListUseTwoLineAttributeName = "asp-message-list-use-two-line";

		/// <summary>
		///     Get or set the message list style.
		/// </summary>
		[HtmlAttributeName(MessageListAttributeName)]
		public MessageListStyle ListStyle { get; set; } = MessageListStyle.AlertDialog;

		/// <summary>
		///     Get or set a value that indicate if each message should use two line mode.
		/// </summary>
		[HtmlAttributeName(MessageListUseTwoLineAttributeName)]
		public bool UseTwoLine { get; set; } = false;

		#endregion
	}
}