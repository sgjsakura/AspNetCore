using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;

namespace Sakura.AspNetCore;

/// <summary>
///     Provide fast access for snapshotted one page data from a data source.
/// </summary>
/// <typeparam name="TSource">The type of the data source.</typeparam>
/// <typeparam name="TElement">The element type in the page.</typeparam>
public class PagedList<TSource, TElement> : IPagedList<TElement>
{
	/// <summary>
	///     Initializes a new instance of the <see cref="PagedList{TSource,TElement}" /> class.
	/// </summary>
	/// <param name="currentPage">
	///     The list of current page data.
	/// </param>
	/// <param name="source">
	///     The source of the page data.
	/// </param>
	/// <param name="pageSize">
	///     The page size.
	/// </param>
	/// <param name="pageIndex">
	///     The page index of current page.
	/// </param>
	/// <param name="totalCount">
	///     The total data count in the <paramref name="source" />.
	/// </param>
	/// <param name="totalPage">
	///     The total page of the <paramref name="source" />.
	/// </param>
	/// <exception cref="ArgumentOutOfRangeException">
	///     The <paramref name="pageSize" /> or <paramref name="pageIndex" /> is not positive, or the
	///     <paramref name="totalCount" /> or <paramref name="totalPage" /> is negative.
	/// </exception>
	/// <exception cref="ArgumentNullException">
	///     The <paramref name="currentPage" /> or <paramref name="source" /> is <c>null</c>.
	/// </exception>
	public PagedList([NotNull] IList<TElement> currentPage, [NotNull] TSource source, int pageSize, int pageIndex,
		int totalCount, int totalPage)
	{
		if (currentPage == null)
			throw new ArgumentNullException(nameof(currentPage));

		if (pageSize <= 0)
			throw new ArgumentOutOfRangeException(nameof(pageSize));

		if (pageIndex <= 0)
			throw new ArgumentOutOfRangeException(nameof(pageIndex));

		if (totalCount < 0)
			throw new ArgumentOutOfRangeException(nameof(totalCount));

		if (totalPage < 0)
			throw new ArgumentOutOfRangeException(nameof(totalPage));

		Source = source != null ? source : throw new ArgumentNullException(nameof(source));
		PageSize = pageSize;
		PageIndex = pageIndex;
		CurrentPage = new ReadOnlyCollection<TElement>(currentPage);
		TotalCount = totalCount;
		TotalPage = totalPage;
	}

	/// <summary>
	///     Get the actual data contained in the current page.
	/// </summary>
	[PublicAPI]
	protected ReadOnlyCollection<TElement> CurrentPage { get; }

	/// <summary>
	///     The source of the page data. This property is a reference of the constructor argument, any change of the source
	///     will not be watched.
	/// </summary>
	[PublicAPI]
	public TSource Source { get; }

	/// <summary>
	///     Get the size of each page.
	/// </summary>
	public int PageSize { get; }

	/// <summary>
	///     Get the index of the current page in the original data source.
	/// </summary>
	/// <remarks>
	///     The index is start from one (not zero).
	/// </remarks>
	public int PageIndex { get; }

	/// <summary>
	///     Get the total item count of data source.
	/// </summary>
	public int TotalCount { get; }

	/// <summary>
	///     Get the total page count.
	/// </summary>
	public int TotalPage { get; }


	/// <summary>
	///     Get the item count in the current page.
	/// </summary>
	public int Count => CurrentPage.Count;

	/// <summary>
	///     Get the element at the specified location in the current page.
	/// </summary>
	/// <param name="index">The index of the location starts at zero.</param>
	/// <returns>The element in the specified location.</returns>
	public TElement this[int index] => CurrentPage[index];

	#region Implementation of IEnumerable

	/// <inheritdoc />
	IEnumerator<TElement> IEnumerable<TElement>.GetEnumerator()
	{
		return CurrentPage.GetEnumerator();
	}

	/// <inheritdoc />
	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable) CurrentPage).GetEnumerator();
	}

	#endregion

	#region Implementation of ICollection

	/// <inheritdoc />
	void ICollection.CopyTo(Array array, int index)
	{
		((ICollection) CurrentPage).CopyTo(array, index);
	}

	/// <inheritdoc />
	bool ICollection.IsSynchronized => ((ICollection) CurrentPage).IsSynchronized;

	/// <inheritdoc />
	object ICollection.SyncRoot => ((ICollection) CurrentPage).SyncRoot;

	#endregion

	#region Implementation of IList

	/// <inheritdoc />
	int IList.Add(object value)
	{
		throw new NotSupportedException();
	}

	/// <inheritdoc />
	void IList.Clear()
	{
		throw new NotSupportedException();
	}


	/// <inheritdoc />
	bool IList.Contains(object value)
	{
		return ((IList) CurrentPage).Contains(value);
	}


	/// <inheritdoc />
	int IList.IndexOf(object value)
	{
		return ((IList) CurrentPage).IndexOf(value);
	}


	/// <inheritdoc />
	void IList.Insert(int index, object value)
	{
		throw new NotSupportedException();
	}

	/// <inheritdoc />
	void IList.Remove(object value)
	{
		throw new NotSupportedException();
	}

	/// <inheritdoc />
	void IList.RemoveAt(int index)
	{
		throw new NotSupportedException();
	}

	/// <inheritdoc />
	bool IList.IsFixedSize => true;

	/// <inheritdoc />
	bool IList.IsReadOnly => true;

	/// <inheritdoc />
	object IList.this[int index]
	{
		get => ((IList) CurrentPage)[index];
		set => throw new NotSupportedException();
	}

	#endregion
}