using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Sakura.AspNetCore
{
	/// <summary>
	///     Provide extension method for adding messages. This class is static.
	/// </summary>
	[PublicAPI]
	public static class OperationMessageExtensions
	{
		/// <summary>
		///     Add a new message into the message collection.
		/// </summary>
		/// <param name="collection">The collection of messages to be adding the new message.</param>
		/// <param name="level">The level of the new message.</param>
		/// <param name="title">The title of the new message.</param>
		/// <param name="description">The detailed description of the new message.</param>
		/// <returns>The newly added <see cref="OperationMessage" /> object.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="collection" /> is <c>null</c>.</exception>
		public static OperationMessage Add([NotNull] this ICollection<OperationMessage> collection,
			OperationMessageLevel level, [CanBeNull] [LocalizationRequired] string title,
			[CanBeNull] [LocalizationRequired] string description = null)
		{
			if (collection == null)
				throw new ArgumentNullException(nameof(collection));

			var item = new OperationMessage(level, title, description);
			collection.Add(item);

			return item;
		}
	}
}