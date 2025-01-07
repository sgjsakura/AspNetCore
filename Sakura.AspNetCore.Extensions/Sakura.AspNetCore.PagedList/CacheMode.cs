using System;

namespace Sakura.AspNetCore;

/// <summary>
///     Define the execution policy for caching features.
/// </summary>
[Flags]
public enum CacheMode
{
	/// <summary>
	///     Always using real-time calculated data until user caches data manually.
	/// </summary>
	Manual = 0,

	/// <summary>
	///     Automatically cache and fresh data when access it in the first time.
	/// </summary>
	Auto,

	/// <summary>
	///     Immediately cache and fresh data whenever caching is built up and data is dismissed.
	/// </summary>
	AutoWithPrefetch
}