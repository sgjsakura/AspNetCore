using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sakura.AspNetCore.Mvc;

/// <summary>
///     Provide default implementation for <see cref="IOperationMessageHtmlGenerator" />.
/// </summary>
public class DefaultOperationMessageHtmlGenerator : IOperationMessageHtmlGenerator
{
	/// <summary>
	///     Initialize a new instance of <see cref="DefaultOperationMessageHtmlGenerator" />.
	/// </summary>
	/// <param name="levelClassMapper">The message-level to CSS class mapper.</param>
	public DefaultOperationMessageHtmlGenerator(IOperationMessageLevelClassMapper levelClassMapper)
	{
		LevelClassMapper = levelClassMapper;
	}

	/// <summary>
	///     Get the message-level to CSS class mapper.
	/// </summary>
	protected IOperationMessageLevelClassMapper LevelClassMapper { get; }

	/// <summary>
	///     Generate the HTML message list for a collection of <see cref="OperationMessage" /> items.
	/// </summary>
	/// <param name="messages">The collection of all <see cref="OperationMessage" /> items.</param>
	/// <param name="listStyle">The list style of the <see cref="OperationMessage" />.</param>
	/// <param name="useTwoLineMode">If the two line mode should be used.</param>
	/// <returns>The generated HTML message list for all messages.</returns>
	public IHtmlContent GenerateList(IEnumerable<OperationMessage> messages, MessageListStyle listStyle,
		bool useTwoLineMode)
	{
		if (messages == null)
			throw new ArgumentNullException(nameof(messages));

		switch (listStyle)
		{
			case MessageListStyle.AlertDialog:
			case MessageListStyle.AlertDialogClosable:
				return GenerateAlertList(messages, listStyle, useTwoLineMode);
			case MessageListStyle.List:
				return GenerateNormalList(messages, useTwoLineMode);
			default:
				throw new ArgumentException("The value of the argument is not a valid enum item.", nameof(listStyle));
		}
	}

	/// <summary>
	///     Generate HTML content for the title.
	/// </summary>
	/// <param name="title">The content of the title.</param>
	/// <returns>The generated HTML content.</returns>
	private static IHtmlContent GenerateTitle(IHtmlContent title)
	{
		var tag = new TagBuilder("strong");
		tag.InnerHtml.AppendHtml(title);
		return tag;
	}

	/// <summary>
	///     Generate HTML content for the description.
	/// </summary>
	/// <param name="description">The description of the title.</param>
	/// <returns>The generated HTML content.</returns>
	private static IHtmlContent GenerateDescription(IHtmlContent description)
	{
		var tag = new TagBuilder("span");
		tag.InnerHtml.AppendHtml(description);
		return tag;
	}

	/// <summary>
	///     Generate the HTML content for a <see cref="OperationMessage" />.
	/// </summary>
	/// <param name="message">The <see cref="OperationMessage" /> instance.</param>
	/// <param name="useTwoLineMode">If the two line mode should be used.</param>
	/// <returns>The generated HTML content.</returns>
	private static IHtmlContent GenerateMessageContent(OperationMessage message, bool useTwoLineMode)
	{
		var result = new DefaultTagHelperContent();

		// Title
		result.AppendHtml(GenerateTitle(message.Title));

		// If description exists, add it
		if (message.Description != null)
		{
			// Add a newline for two line mode.
			if (useTwoLineMode)
				result.AppendHtml("<br />");
			else
				result.AppendHtml("&nbsp;&nbsp;");

			result.AppendHtml(GenerateDescription(message.Description));
		}

		return result;
	}

	/// <summary>
	///     Generate the a list item for a <see cref="OperationMessage" />.
	/// </summary>
	/// <param name="message">The <see cref="OperationMessage" /> instance.</param>
	/// <param name="useTwoLineMode">If the two line mode should be used.</param>
	/// <returns>The generated HTML content which represent as a HTML list item.</returns>
	private IHtmlContent GenerateNormalItem(OperationMessage message, bool useTwoLineMode)
	{
		var tag = new TagBuilder("li");

		tag.AddCssClass("list-group-item");
		tag.AddCssClass(LevelClassMapper.MapLevel(message.Level, MessageListStyle.List));
		tag.InnerHtml.AppendHtml(GenerateMessageContent(message, useTwoLineMode));

		return tag;
	}

	/// <summary>
	///     Generate the a alert dialog for a <see cref="OperationMessage" />.
	/// </summary>
	/// <param name="message">The <see cref="OperationMessage" /> instance.</param>
	/// <param name="isClosable">If the alert dialog is closable.</param>
	/// <param name="useTwoLineMode">If the two line mode should be used.</param>
	/// <returns>The generated HTML content which represent as a HTML alert dialog.</returns>
	private IHtmlContent GenerateAlertItem(OperationMessage message, bool isClosable, bool useTwoLineMode)
	{
		// Real style
		var listStyle = isClosable ? MessageListStyle.AlertDialogClosable : MessageListStyle.AlertDialog;

		var tag = new TagBuilder("div");

		tag.AddCssClass("alert");
		tag.AddCssClass(LevelClassMapper.MapLevel(message.Level, listStyle));

		// Closable handling
		if (isClosable)
			tag.AddCssClass("alert-dismissible");

		tag.MergeAttribute("role", "alert");

		var content = new DefaultTagHelperContent();

		if (isClosable)
			content.AppendHtml(
				"<button type =\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>");

		content.AppendHtml(GenerateMessageContent(message, useTwoLineMode));

		// Internal content
		tag.InnerHtml.AppendHtml(content);

		return tag;
	}

	/// <summary>
	///     Generate the a list for a series of <see cref="OperationMessage" /> items.
	/// </summary>
	/// <param name="messages">The collection of all <see cref="OperationMessage" /> items.</param>
	/// <param name="useTwoLineMode">If the two line mode should be used.</param>
	/// <returns>The generated HTML content which represent as a HTML list.</returns>
	private IHtmlContent GenerateNormalList(IEnumerable<OperationMessage> messages, bool useTwoLineMode)
	{
		var tag = new TagBuilder("ul");

		tag.AddCssClass("list-group");

		var content = new DefaultTagHelperContent();

		foreach (var message in messages)
			content.AppendHtml(GenerateNormalItem(message, useTwoLineMode));

		tag.InnerHtml.AppendHtml(content);

		return tag;
	}

	/// <summary>
	///     Generate the a series of alert dialogs for a collection of <see cref="OperationMessage" /> items.
	/// </summary>
	/// <param name="messages">The collection of all <see cref="OperationMessage" /> items.</param>
	/// <param name="listStyle">The list style of the <see cref="OperationMessage" />.</param>
	/// <param name="useTwoLineMode">If the two line mode should be used.</param>
	/// <returns>The generated HTML content which represent as a series of alert dialogs.</returns>
	private IHtmlContent GenerateAlertList(IEnumerable<OperationMessage> messages, MessageListStyle listStyle,
		bool useTwoLineMode)
	{
		var tag = new TagBuilder("div");

		var content = new DefaultTagHelperContent();

		foreach (var message in messages)
			content.AppendHtml(GenerateAlertItem(message, listStyle == MessageListStyle.AlertDialogClosable,
				useTwoLineMode));

		tag.InnerHtml.AppendHtml(content);

		return tag;
	}
}