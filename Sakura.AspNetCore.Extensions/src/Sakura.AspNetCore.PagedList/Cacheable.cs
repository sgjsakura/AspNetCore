using System;
using System.Threading;

namespace Sakura.AspNetCore
{
	/// <summary>
	///     Represent as a cachable data.
	/// </summary>
	/// <typeparam name="T">The type of the data to be caching。</typeparam>
	/// <seealso cref="ICacheControl" />
	public class Cacheable<T> : ICacheControl
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
		public Cacheable(Func<T> getDataCallback, Func<T, T> cacheCallback = null, CacheMode cacheMode = CacheMode.Manual)
		{
			GetDataCallback = getDataCallback ?? throw new ArgumentNullException(nameof(getDataCallback));

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
		private Func<T> GetDataCallback { get; }

		/// <summary>
		///     Get the cached data. If no cache is available, the default value of <typeparamref name="T" /> will be returned.
		/// </summary>
		public T CachedData { get; private set; }

		/// <summary>
		///     Get a value that indicates whether the cache is available now.
		/// </summary>
		public bool IsCached { get; private set; }

		/// <summary>
		///     Get the cached data. If cache is not available, the load result from source will be returned, and caching may or
		///     may not be executed according to the behavior indicated by <see cref="CacheMode" /> property.
		/// </summary>
		public T Data
		{
			get
			{
				// Alaways return the cache if available.
				if (IsCached)
					return CachedData;

				// No caching if caching mode is set to "manual".
				if (CacheMode == CacheMode.Manual)
					return GetDataDirectly();

				// Otherwise, reload the cache and return the result.
				Reload();
				return CachedData;
			}
		}

		/// <summary>
		///     Forcely remove the current cache and reload it.
		/// </summary>
		public void Reload()
		{
			CachedData = CacheCallback(GetDataCallback());
			IsCached = true;
		}

		/// <summary>
		///     Generate a cache from the data source. If the data is already cached, this method will do nothing.
		/// </summary>
		public void Cache()
		{
			if (!IsCached)
				Reload();
		}

		/// <summary>
		///     Uncache the current value. This method will make cache dismissed, but will not automatically remove or refresh the
		///     cached data
		/// </summary>
		public void Uncache()
		{
			IsCached = false;
		}

		/// <summary>
		///     Get or set the cache mode of the current object.
		/// </summary>
		public CacheMode CacheMode { get; set; }


		/// <summary>
		///     Temporary disable auto refreshing.
		/// </summary>
		/// <returns>An <see cref="IDisposable" /> object which will re-enabled the auto refreshing when it is disposed.</returns>
		public IDisposable DisableAutoRefresh()
		{
			return new DisableAutoRefreshController<T>(this);
		}

		/// <summary>
		///     The default value for <see cref="CacheCallback" />
		/// </summary>
		/// <param name="item">The data to be cached.</param>
		/// <returns>The cached data.</returns>
		/// <remarks>This method will do nothing.</remarks>
		private static T DefaultCacheCallback(T item)
		{
			return item;
		}

		/// <summary>
		///     Get data directly from the source and ignore any cache.
		/// </summary>
		/// <returns>The data got directly from the source.</returns>
		public T GetDataDirectly()
		{
			return GetDataCallback();
		}

		/// <summary>
		///     Notify the cached data is dismissed, and reload it if necessary.
		/// </summary>
		public void NotifyDismiss()
		{
			// Make the cache unavailable.
			IsCached = false;

			// Automatically reload the cache if cache mode is set to "prefetch" and currently caching refresh is enabled.
			if (CacheMode == CacheMode.AutoWithPrefetch && _AutoRefreshControlCount == 0)
				Reload();
		}


		/// <summary>
		///     Increase the auto refreshing control value to disable auto refreshing temporary.
		/// </summary>
		internal void AddAutoRefreshControl()
		{
			Interlocked.Increment(ref _AutoRefreshControlCount);
		}

		/// <summary>
		///     Decrease the auto refreshing control value, and reload data immediately if auto refreshing is re-enabled.
		/// </summary>
		internal void RemoveAutoRefreshControl()
		{
			// If the counter is zero then reload data.
			if (Interlocked.Decrement(ref _AutoRefreshControlCount) == 0)
				Reload();
		}
	}
}