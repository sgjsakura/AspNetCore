using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Sakura.AspNetCore.Mvc.Internal
{
	/// <summary>
	///     Provide utility methods. This class is static.
	/// </summary>
	internal static class Utility
	{
		/// <summary>
		///     Try get the value associated with the speicified key in a dictionary. If the key is not presented, return a custom
		///     default value.
		/// </summary>
		/// <typeparam name="TKey">The key type of the <paramref name="dictionary" />.</typeparam>
		/// <typeparam name="TValue">The value type of the <paramref name="dictionary" />.</typeparam>
		/// <param name="dictionary">The dictionary which contains all key and values.</param>
		/// <param name="key">The key associated with the target value in the <paramref name="dictionary" />.</param>
		/// <param name="defaultValue">
		///     The default value if <paramref name="key" /> is not presented in the
		///     <paramref name="dictionary" />.
		/// </param>
		/// <returns>
		///     If the <paramref name="key" /> is presented in the <paramref name="dictionary" />, returns its associated
		///     value; otherwise, returns the <paramref name="defaultValue" />.
		/// </returns>
		public static TValue GetValueOfDefault<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dictionary, TKey key,
			TValue defaultValue = default(TValue))

		{
			if (dictionary == null)
				throw new ArgumentNullException(nameof(dictionary));

			return dictionary.TryGetValue(key, out TValue result) ? result : defaultValue;
		}
	}
}