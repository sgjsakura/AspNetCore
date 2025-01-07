namespace Sakura.AspNetCore.Mvc;

/// <summary>
///     Define the style of message list.
/// </summary>
public enum MessageListStyle
{
	/// <summary>
	///     Each message will display an individual alert dialog.
	/// </summary>
	AlertDialog = 0,

	/// <summary>
	///     Each message will display an individual alert dialog. The dialog is closable.
	/// </summary>
	AlertDialogClosable,

	/// <summary>
	///     Use a list to display all messages.
	/// </summary>
	List
}