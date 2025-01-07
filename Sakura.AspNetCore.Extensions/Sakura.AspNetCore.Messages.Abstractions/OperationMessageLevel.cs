namespace Sakura.AspNetCore;

/// <summary>
///     Define the level of messages.
/// </summary>
/// <seealso cref="OperationMessage.Level" />
public enum OperationMessageLevel
{
	/// <summary>
	///     The level of the message is not classified.
	/// </summary>
	None = 0,

	/// <summary>
	///     The message is in debug level.
	/// </summary>
	Debug,

	/// <summary>
	///     The message is in verbose level.
	/// </summary>
	Verbose,

	/// <summary>
	///     The message is in information level, which means it can be ignored safely.
	/// </summary>
	Info,

	/// <summary>
	///     The message is in success level, it represent as a successfully operation result.
	/// </summary>
	Success,

	/// <summary>
	///     The message is in warning level, it represents as an non-critical incorrect runtime behavior.
	/// </summary>
	Warning,

	/// <summary>
	///     The message is in error level, it means there is some problem during the system executing.
	/// </summary>
	Error,

	/// <summary>
	///     The message is in critical level, the system may be badly broken and cannot continue for execution.
	/// </summary>
	Critical
}