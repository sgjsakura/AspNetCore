namespace Sakura.AspNetCore.Mvc
{
	/// <summary>
	///     Provide methods to convert <see cref="OperationMessageLevel" /> into CSS class names.
	/// </summary>
	public interface IOperationMessageLevelClassMapper
	{
		/// <summary>
		///     Convert <see cref="OperationMessageLevel" /> to a CSS class names.
		/// </summary>
		/// <param name="value">The value to converting.</param>
		/// <param name="listStyle">The list style of the operation message list.</param>
		/// <returns>Converted class names. Using space to split mutilple class names.</returns>
		string MapLevel(OperationMessageLevel value, MessageListStyle listStyle);
	}
}