using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sakura.AspNetCore
{
	/// <summary>
	///     Represent as a cachable data.
	/// </summary>
	/// <typeparam name="T">The type of the data to be caching。</typeparam>
	/// <seealso cref="ICacheControl" />
	public class AsyncCacheable<T> : ICacheControl
	{
		/// <summary>
		///     Auto refreshing counter used by refreshing control feature.
		/// </summary>
		private int _AutoRefreshControlCount;

		/// <summary>
		///     Initialize a new instance using specified information.
		/// </summary>
		/// <param name="getDataCallback">The callback delegate to obtain the data to be caching.</param>
		/// <param name="cacheCallback">
		///     A optional callback delegate to generate a cached copy for original data. If this parameter
		///     is <c>null</c>, <see cref="DefaultCacheCallback" /> will be used.
		/// </param>
		/// <param name="cacheMode">
		///     The default cache mode of the cache. The default value of this parameter is
		///     <see cref="CacheMode.Manual" />.
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="getDataCallback" /> is <c>null</c>.</exception>
		public AsyncCacheable(Func<Task<T>> getDataCallback, Func<T, T> cacheCallback = null, CacheMode cacheMode = CacheMode.Manual)
		{
			if (getDataCallback == null)
			{
				throw new ArgumentNullException(nameof(getDataCallback));
			}

			GetDataCallback = getDataCallback;

			// Set the caching callback.
			CacheCallback = cacheCallback ?? DefaultCacheCallback;

			// Set the caching mode.
			CacheMode = cacheMode;

			// Trigger the first dismiss notification.
			NotifyDismiss();
		}

		/// <summary>
		///     Get the callback delegate for generating a cached copy.
		/// </summary>
		private Func<T, T> CacheCallback { get; }

		/// <summary>
		///     Get the callback delegate for get the data to be caching.
		/// </summary>
		private Func<Task<T>> GetDataCallback { get; }

		/// <summary>
		///     Get the cached data. If no cache is available, the default value of <typeparamref name="T" /> will be returned.
		/// </summary>
		public T CachedData { get; private set; }

		/// <summary>
		///     Get a value that indicates whether the cache is available now.
		/// </summary>
		public bool IsCached { get; private set; }


		public async Task<T> GetDataAsync()
		{
			// Alaways return the cache if available.
			if (IsCached)
			{
				return CachedData;
			}

			// No caching if caching mode is set to "manual".
			if (CacheMode == CacheMode.Manual)
			{
				return await GetDataDirectlyAsync();
			}

			// Otherwise, reload the cache and return the result.
			await ReloadAsync();
			return CachedData;

		}

		/// <summary>
		///     Forcely remove the current cache and reload it.
		/// </summary>
		public async Task ReloadAsync()
		{
			CachedData = CacheCallback(await GetDataCallback());
			IsCached = true;
		}

		/// <summary>
		///     Generate a cache from the data source. If the data is already cached, this method will do nothing.
		/// </summary>
		public async Task CacheAsync()
		{
			if (!IsCached)
			{
				await ReloadAsync();
			}
		}

		/// <summary>
		///     Uncache the current value. This method will make cache dismissed, but will not automatically remove or refresh the
		///     cached data
		/// </summary>
		public Task UncacheAsync()
		{
			IsCached = false;
			return Task.CompletedTask;
		}

		/// <summary>
		///     Get or set the cache mode of the current object.
		/// </summary>
		public CacheMode CacheMode { get; set; }

		/// <summary>
		///     The default value for <see cref="CacheCallback" />
		/// </summary>
		/// <param name="item">The data to be cached.</param>
		/// <returns>The cached data.</returns>
		/// <remarks>This method will do nothing.</remarks>
		private static T DefaultCacheCallback(T item) => item;

		/// <summary>
		///     Get data directly from the source and ignore any cache.
		/// </summary>
		/// <returns>The data got directly from the source.</returns>
		public Task<T> GetDataDirectlyAsync() => GetDataCallback();

		/// <summary>
		///     Notify the cached data is dismissed, and reload it if necessary.
		/// </summary>
		public async Task NotifyDismissAsync()
		{
			// Make the cache unavailable.
			IsCached = false;

			// Automatically reload the cache if cache mode is set to "prefetch" and currently caching refresh is enabled.
			if (CacheMode == CacheMode.AutoWithPrefetch && _AutoRefreshControlCount == 0)
			{
				await ReloadAsync();
			}
		}

	}
}