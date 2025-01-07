using System.Collections.Generic;

namespace Sakura.AspNetCore;

/// <summary>
///     Provide the service for getting the messages from the current execution context.
/// </summary>
public interface IOperationMessageAccessor
{
	/// <summary>
	///     Get all the messages in the current context. If no messages are currently set, this property will return an empty
	///     collection.
	/// </summary>
	ICollection<OperationMessage> Messages { get; }
}