using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Sakura.AspNetCore
{
	public class AsyncPagedList<T> : IAsyncEnumerable<T>
	{
		#region Constructors

		/// <summary>
		///     Initialize a new instance with specified information.
		/// </summary>
		/// <param name="source">The data source to be paging.</param>
		/// <param name="pageSize">The size of each page.</param>
		/// <param name="pageIndex">The index of the current page, start from 1.</param>
		/// <param name="cacheOptions">Additional cacheOptions for the paged list.</param>
		public AsyncPagedList(IAsyncEnumerable<T> source, int pageSize, int pageIndex = 1,
			DynamicPagedListCacheOptions cacheOptions = null)
		{
			// Exception handling
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			// Default parameters
			var defaultOptions = new DynamicPagedListCacheOptions
			{
				CurrentPageCacheMode = CacheMode.Manual,
				TotalCountCacheMode = CacheMode.Manual
			};

			// Merge cacheOptions
			cacheOptions = cacheOptions ?? defaultOptions;

			// Initialize the data source
			Source = source;


			// Set Paging information
			PageSize = pageSize;
			PageIndex = pageIndex;


			// Caching total count for first time
			TotalCountCacheInternal = new AsyncCacheable<Task<int>>(GetTotalCountAsync, cacheMode: cacheOptions.TotalCountCacheMode);
			// Caching current page for first time
			CurrentPageCacheInternal = new AsyncCacheable<IAsyncEnumerable<T>>(GetCurrentPageAsync, CacheDataAsync, cacheOptions.CurrentPageCacheMode);

			// Set initialized flag
			IsInitialized = true;
		}

		#endregion

		#region Data Source

		/// <summary>
		///     Get the original data source.
		/// </summary>
		public IAsyncEnumerable<T> Source { get; }

		#endregion

		#region Core Features must be implemented in Derived Classes

		/// <summary>
		///     When be derived, returns the total count of the data source.
		/// </summary>
		/// <returns>The total count of the data source.</returns>
		protected virtual Task<int> GetTotalCountAsync() => Source.Count();

		/// <summary>
		///     When be derived, get the elements in the current page.
		/// </summary>
		/// <returns>The collection of elements in the current page.</returns>
		protected virtual IAsyncEnumerable<T> GetCurrentPageAsync()
		{
			var skipValue = PageSize * (PageIndex - 1);
			var takeValue = PageSize;

			return Source.Skip(skipValue).Take(takeValue);
		}

		/// <summary>
		///     When be derived, Cache the data page.
		/// </summary>
		/// <param name="source">The data page to be cached.</param>
		/// <returns>The cached copy of <paramref name="source" />.</returns>
		protected virtual async Task<IAsyncEnumerable<T>> CacheDataAsync(IAsyncEnumerable<T> source)
		{
			return (await source.ToArray()).ToAsyncEnumerable();
		}

		#endregion

		#region Data Caching and Controllers

		/// <summary>
		///     Get the internal caching object for total count.
		/// </summary>
		private AsyncCacheable<int> TotalCountCacheInternal { get; }

		/// <summary>
		///     Get the internal caching object for current page.
		/// </summary>
		private AsyncCacheable<IAsyncEnumerable<T>> CurrentPageCacheInternal { get; }

		/// <summary>
		///     Get the cache controller for the total count cache.
		/// </summary>
		public ICacheControl TotalCountCache => TotalCountCacheInternal;

		/// <summary>
		///     Get the cache controller for the current page cache.
		/// </summary>
		public ICacheControl CurrentPageCache => CurrentPageCacheInternal;

		#endregion

		#region Cached Core Data Properties

		/// <summary>
		///     Get the data in current page.
		/// </summary>
		public Task<IAsyncEnumerable<T>> CurrentPage => CurrentPageCacheInternal.GetDataAsync();

		/// <summary>
		///     Get the total count of the data source.
		/// </summary>
		public Task<int> GetTotalCountAsync() => TotalCountCacheInternal.GetDataAsync();

		#endregion

		#region Paging Core Features

		/// <summary>
		///     The current page index.
		/// </summary>
		private int _PageIndex;

		/// <summary>
		///     The size of each page.
		/// </summary>
		private int _PageSize;

		/// <summary>
		///     Get or set the current page index. The page index is started from 1.
		/// </summary>
		public int PageIndex
		{
			get { return _PageIndex; }
			set
			{
				// If initialized, make detailed value check
				if (IsInitialized)
				{
					// Range check
					if (value < 1 || value > TotalPage)
					{
						throw new ArgumentOutOfRangeException(nameof(value), value, "The page index is out of valid range.");
					}

					if (_PageIndex != value)
					{
						_PageIndex = value;

						// Notify data dismiss
						CurrentPageCacheInternal.NotifyDismiss();
					}
				}
				// When initializing, skip the detailed check
				else
				{
					// Basic range check
					if (value < 1)
					{
						throw new ArgumentOutOfRangeException(nameof(value), value, "The page index is out of valid range.");
					}

					_PageIndex = value;
				}
			}
		}

		/// <summary>
		///     Get or set the size of each page.
		/// </summary>
		/// <remarks>
		///     Modify this property will reset the <see cref="PageIndex" /> to <c>1</c>. This may raise data refreshing. If you
		///     will change <see cref="PageIndex" /> later, please condiser call <see cref="ICacheControl.DisableAutoRefresh" /> on
		///     <see cref="CurrentPageCache" /> property in order to improve the performance.
		/// </remarks>
		public int PageSize
		{
			get { return _PageSize; }
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException(nameof(value), value, "Page size must be positive integer");
				}

				_PageSize = value;
				PageIndex = 1;
			}
		}

		#endregion

		#region Additional propreties

		/// <summary>
		///     Get a value that indicates if the object has been initialized.
		/// </summary>
		public bool IsInitialized { get; }

		/// <summary>
		///     Get the total page count of the data source.
		/// </summary>
		public int TotalPage => (TotalCount - 1) / PageSize + 1;

		/// <summary>
		///     Get the count of current page.
		/// </summary>
		public int Count
		{
			get
			{
				// Cache property value.
				var totalCount = TotalCount;

				// Total page
				// NOTE: Cannot use propreties since it may cause duplicated computation
				var totalPage = (totalCount - 1) / PageSize + 1;

				// Only the last page should be calculated.
				return PageIndex != totalPage ? PageSize : totalCount - PageSize * (totalPage - 1);
			}
		}

		#endregion

		#region Interface Implementations

		/// <summary>
		///     Get the enumerator for the current object.
		/// </summary>
		/// <returns>The enumerator for the current object.</returns>
		public IAsyncEnumerator<T> GetEnumerator()
		{
			return CurrentPage.GetEnumerator();
		}


		/// <summary>
		///     Get the data at the specified location in the current page.
		/// </summary>
		/// <param name="index">The location index in the current page, starting from zero.</param>
		/// <returns>The element at the specified location in the current page.</returns>
		public Task<T> GetAsync(int index) => CurrentPage.ElementAt(index);

		#endregion
	}
}
