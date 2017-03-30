using System;
using JetBrains.Annotations;

namespace Sakura.AspNetCore
{
	/// <summary>
	///     Define caching management related features.
	/// </summary>
	[PublicAPI]
	public interface ICacheControl
	{
		/// <summary>
		///     Get or set the cache mode of the current object.
		/// </summary>
		CacheMode CacheMode { get; set; }

		/// <summary>
		///     Cache the current data.
		/// </summary>
		void Cache();

		/// <summary>
		///     Uncache the current data.
		/// </summary>
		void Uncache();

		/// <summary>
		///     Forcedly reload data even caching is available.
		/// </summary>
		void Reload();

		/// <summary>
		///     Temporally disable auto refreshing.
		/// </summary>
		/// <returns>An <see cref="IDisposable" /> object that will re-enable auto refreshing when it is disposed.</returns>
		IDisposable DisableAutoRefresh();
	}
}