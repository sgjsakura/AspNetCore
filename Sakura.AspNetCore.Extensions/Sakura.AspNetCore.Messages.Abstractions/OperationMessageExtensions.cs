﻿using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Html;

namespace Sakura.AspNetCore;

/// <summary>
///     Provide extension method for adding messages. This class is static.
/// </summary>
[PublicAPI]
public static class OperationMessageExtensions
{
	/// <summary>
	///     Add a new message into the message collection.
	/// </summary>
	/// <param name="messageAccessor">The collection of messages to be adding the new message.</param>
	/// <param name="level">The level of the new message.</param>
	/// <param name="title">The title of the new message.</param>
	/// <param name="description">The detailed description of the new message.</param>
	/// <returns>The newly added <see cref="OperationMessage" /> object.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="messageAccessor" /> is <c>null</c>.</exception>
	public static OperationMessage Add([NotNull] this IOperationMessageAccessor messageAccessor,
		OperationMessageLevel level, [LocalizationRequired] IHtmlContent? title,
		[LocalizationRequired] IHtmlContent? description = null)
	{
		if (messageAccessor == null)
			throw new ArgumentNullException(nameof(messageAccessor));

		var item = new OperationMessage(level, title, description);
		messageAccessor.Messages.Add(item);

		return item;
	}

	/// <summary>
	///     Add a new message into the message collection.
	/// </summary>
	/// <param name="messageAccessor">The collection of messages to be adding the new message.</param>
	/// <param name="level">The level of the new message.</param>
	/// <param name="title">The title of the new message.</param>
	/// <param name="description">The detailed description of the new message.</param>
	/// <returns>The newly added <see cref="OperationMessage" /> object.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="messageAccessor" /> is <c>null</c>.</exception>
	public static OperationMessage Add([NotNull] this IOperationMessageAccessor messageAccessor,
		OperationMessageLevel level, [LocalizationRequired] string? title,
		[LocalizationRequired] string? description = null)
	{
		if (messageAccessor == null)
			throw new ArgumentNullException(nameof(messageAccessor));

		var item = new OperationMessage(level, EncodeToHtml(title), EncodeToHtml(description));
		messageAccessor.Messages.Add(item);

		return item;
	}

	private static IHtmlContent EncodeToHtml(string value)
	{
		return new HtmlContentBuilder().Append(value);
	}
}