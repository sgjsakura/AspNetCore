using System;
using JetBrains.Annotations;

namespace Sakura.AspNetCore.Mvc
{
	/// <summary>
	///     Provide the default implementation of <see cref="IOperationMessageLevelClassMapper" />.
	/// </summary>
	[PublicAPI]
	public class DefaultOperationMessageLevelClassMapper : IOperationMessageLevelClassMapper
	{
		/// <summary>
		///     Convert <see cref="OperationMessageLevel" /> to a CSS class names.
		/// </summary>
		/// <param name="value">The value to converting.</param>
		/// <param name="listStyle">The list style of the operation message list.</param>
		/// <returns>Converted class names. Using space to split mutilple class names.</returns>
		public string MapLevel(OperationMessageLevel value, MessageListStyle listStyle)
		{
			switch (listStyle)
			{
				case MessageListStyle.AlertDialog:
				case MessageListStyle.AlertDialogClosable:
					return MapLevelForAlert(value);
				case MessageListStyle.List:
					return MapLevelForListItem(value);
				default:
					throw new ArgumentException("The argument value is not a valid enum item.", nameof(listStyle));
			}
		}

		/// <summary>
		///     Convert <see cref="OperationMessageLevel" /> to class names used for alert dialog.
		/// </summary>
		/// <param name="value">The value to converting.</param>
		/// <returns>Converted class name.</returns>
		private static string MapLevelForAlert(OperationMessageLevel value)
		{
			switch (value)
			{
				case OperationMessageLevel.Critical:
				case OperationMessageLevel.Error:
					return "alert-danger";
				case OperationMessageLevel.Info:
					return "alert-info";
				case OperationMessageLevel.Success:
					return "alert-success";
				case OperationMessageLevel.Verbose:
					return "alert-muted";
				case OperationMessageLevel.Warning:
					return "alert-warning";
				default:
					return "";
			}
		}

		/// <summary>
		///     Convert <see cref="OperationMessageLevel" /> to class names used for list group.
		/// </summary>
		/// <param name="value">The value to converting.</param>
		/// <returns>Converted class name.</returns>
		private static string MapLevelForListItem(OperationMessageLevel value)
		{
			switch (value)
			{
				case OperationMessageLevel.Critical:
				case OperationMessageLevel.Error:
					return "list-group-item-danger";
				case OperationMessageLevel.Info:
					return "list-group-item-info";
				case OperationMessageLevel.Success:
					return "list-group-item-success";
				case OperationMessageLevel.Verbose:
					return "list-group-item-muted";
				case OperationMessageLevel.Warning:
					return "list-group-item-warning";
				default:
					return "";
			}
		}
	}
}