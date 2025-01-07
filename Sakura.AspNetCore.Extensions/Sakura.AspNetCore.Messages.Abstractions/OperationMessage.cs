using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;

namespace Sakura.AspNetCore;

/// <summary>
///     Represent as an operation message.
/// </summary>
[PublicAPI]
public class OperationMessage
{
	/// <summary>
	///     Initialize a new instance of operation message.
	/// </summary>
	public OperationMessage()
	{
	}

	/// <summary>
	///     Initialize a new instance of operation message with the specified information.
	/// </summary>
	/// <param name="level">The level of the message.</param>
	/// <param name="title">The title of the message.</param>
	/// <param name="description">The detailed description of the message. The default value of this parameter is <c>null</c>.</param>
	public OperationMessage(
		OperationMessageLevel level,
		[LocalizationRequired] IHtmlContent? title,
		[LocalizationRequired] IHtmlContent? description = null)
	{
		Level = level;
		Title = title;
		Description = description;
	}

	/// <summary>
	///     Get or set the detailed description of the message.
	/// </summary>
	[LocalizationRequired]
	public IHtmlContent? Description { get; set; }

	/// <summary>
	///     Get or set the level of the message.
	/// </summary>
	public OperationMessageLevel Level { get; set; }

	/// <summary>
	///     Get or set the title of the message.
	/// </summary>
	[LocalizationRequired]
	public IHtmlContent? Title { get; set; }
}