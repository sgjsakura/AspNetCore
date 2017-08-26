using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using JetBrains.Annotations;

namespace Sakura.AspNetCore
{
	/// <summary>
	///     Provide base common features for <see cref="DynamicPagedList{T}" /> and <see cref="DynamicQueryablePagedList{T}" />
	///     .
	/// </summary>
	/// <typeparam name="TCollection">The collection type of the list.</typeparam>
	/// <typeparam name="TElement">The element type of the list.</typeparam>
	public abstract class DynamicPagedListBase<TCollection, TElement> : IPagedList<TElement>
		where TCollection : IEnumerable<TElement>
	{
		#region Constructors

		/// <summary>
		///     Initialize a new instance with specified information.
		/// </summary>
		/// <param name="source">The data source to be paging.</param>
		/// <param name="pageSize">The size of each page.</param>
		/// <param name="pageIndex">The index of the current page, start from 1.</param>
		/// <param name="cacheOptions">Additional cacheOptions for the paged list.</param>
		protected DynamicPagedListBase(TCollection source, int pageSize, int pageIndex = 1,
			DynamicPagedListCacheOptions cacheOptions = null)
		{
			// Default parameters
			var defaultOptions = new DynamicPagedListCacheOptions
			{
				CurrentPageCacheMode = CacheMode.Manual,
				TotalCountCacheMode = CacheMode.Manual
			};

			// Merge cacheOptions
			cacheOptions = cacheOptions ?? defaultOptions;

			// Initialize the data source
			Source = source != null ? source : throw new ArgumentNullException(nameof(source));


			// Set Paging information
			PageSize = pageSize;
			PageIndex = pageIndex;


			// Caching total count for first time
			TotalCountCacheInternal = new Cacheable<int>(GetTotalCount, cacheMode: cacheOptions.TotalCountCacheMode);
			// Caching current page for first time
			CurrentPageCacheInternal = new Cacheable<TCollection>(GetCurrentPage, CacheData, cacheOptions.CurrentPageCacheMode);

			// Set initialized flag
			IsInitialized = true;
		}

		#endregion

		#region Data Source

		/// <summary>
		///     Get the original data source.
		/// </summary>
		public TCollection Source { get; }

		#endregion

		#region Enumerator

		/// <summary>
		///     Implemetation enumeration for <see cref="DynamicPagedListBase{TCollection,TElement}" /> object.
		/// </summary>
		public struct Enumerator : IEnumerator<TElement>
		{
			/// <summary>
			///     Get the source list of the enumerator.
			/// </summary>
			[NotNull]
			private DynamicPagedListBase<TCollection, TElement> List { get; }

			/// <summary>
			///     Get the version of the enumerator.
			/// </summary>
			private int Version { get; }

			/// <summary>
			///     Get the inner enumerator used for this enumerator.
			/// </summary>
			[NotNull]
			private IEnumerator<TElement> InnerEnumerator { get; }

			internal Enumerator([NotNull] DynamicPagedListBase<TCollection, TElement> source)
			{
				List = source;
				InnerEnumerator = source.CurrentPage.GetEnumerator();
				Version = source._Version;
			}

			/// <summary>
			///     Check the version of the enumrator. If the version not matches, throw an exception.
			/// </summary>
			private void CheckVersion()
			{
				if (List._Version != Version)
					throw new InvalidOperationException(
						"You cannot change the paging information or reload page while enumeration is proceed on this paged list.");
			}

			/// <inheritdoc />
			public bool MoveNext()
			{
				CheckVersion();
				return InnerEnumerator.MoveNext();
			}

			/// <inheritdoc />
			public void Reset()
			{
				CheckVersion();
				InnerEnumerator.Reset();
			}

			/// <inheritdoc />
			public TElement Current
			{
				get
				{
					CheckVersion();
					return InnerEnumerator.Current;
				}
			}

			/// <inheritdoc />
			object IEnumerator.Current => ((IEnumerator) InnerEnumerator).Current;

			/// <inheritdoc />
			public void Dispose()
			{
				InnerEnumerator.Dispose();
			}
		}

		#endregion

		#region Core Features must be implemented in Derived Classes

		/// <summary>
		///     When be derived, returns the total count of the data source.
		/// </summary>
		/// <returns>The total count of the data source.</returns>
		protected abstract int GetTotalCount();

		/// <summary>
		///     When be derived, get the elements in the current page.
		/// </summary>
		/// <returns>The collection of elements in the current page.</returns>
		protected abstract TCollection GetCurrentPage();

		/// <summary>
		///     When be derived, Cache the data page.
		/// </summary>
		/// <param name="source">The data page to be cached.</param>
		/// <returns>The cached copy of <paramref name="source" />.</returns>
		protected abstract TCollection CacheData(TCollection source);

		#endregion

		#region Data Caching and Controllers

		/// <summary>
		///     Get the internal caching object for total count.
		/// </summary>
		private Cacheable<int> TotalCountCacheInternal { get; }

		/// <summary>
		///     Get the internal caching object for current page.
		/// </summary>
		private Cacheable<TCollection> CurrentPageCacheInternal { get; }

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
		public TCollection CurrentPage => CurrentPageCacheInternal.Data;

		/// <summary>
		///     Get the total count of the data source.
		/// </summary>
		public int TotalCount => TotalCountCacheInternal.Data;

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
			get => _PageIndex;
			set
			{
				// If initialized, make detailed value check
				if (IsInitialized)
				{
					// Range check
					if (value < 1 || value > TotalPage)
						throw new ArgumentOutOfRangeException(nameof(value), value, "The page index is out of valid range.");

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
						throw new ArgumentOutOfRangeException(nameof(value), value, "The page index is out of valid range.");

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
			get => _PageSize;
			set
			{
				if (value <= 0)
					throw new ArgumentOutOfRangeException(nameof(value), value, "Page size must be positive integer");

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

		#region Version Control and Enumeration

		/// <summary>
		///     Version field used to check enumeration.
		/// </summary>
		private int _Version;

		/// <summary>
		///     Increase the data version.
		/// </summary>
		protected void IncreaseVersion()
		{
			Interlocked.Increment(ref _Version);
		}

		#endregion

		#region Interface Implementations

		/// <inheritdoc />
		public IEnumerator<TElement> GetEnumerator()
		{
			return new Enumerator(this);
		}


		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <inheritdoc />
		public TElement this[int index] => CurrentPage.ElementAt(index);

		#endregion

		#region Not Supported Features

		int IList.Add(object value)
		{
			throw new NotSupportedException();
		}

		void IList.Clear()
		{
			throw new NotSupportedException();
		}

		bool IList.Contains(object value)
		{
			throw new NotImplementedException();
		}

		int IList.IndexOf(object value)
		{
			throw new NotImplementedException();
		}

		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException();
		}

		void IList.Remove(object value)
		{
			throw new NotSupportedException();
		}

		void IList.RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		#endregion

		#region Supported But Not Recommended Featrures

		void ICollection.CopyTo(Array array, int index)
		{
			Array.Copy(CurrentPage.ToArray(), 0, array, index, Count);
		}


		bool IList.IsFixedSize => true;

		bool IList.IsReadOnly => true;

		object IList.this[int index]
		{
			get => this[index];
			set => throw new NotSupportedException();
		}


		bool ICollection.IsSynchronized => false;

		object ICollection.SyncRoot { get; } = new object();

		#endregion
	}
}