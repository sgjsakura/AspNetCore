using JetBrains.Annotations;

namespace Sakura.AspNetCore
{
	/// <summary>
	///     Define as a <see cref="IPagedList" /> with dynamically page changing support.
	/// </summary>
	public interface IDynamicPagedList : IPagedList
	{
		/// <summary>
		///     The current data paged.
		/// </summary>
		[PublicAPI]
		new int PageIndex { get; set; }

		/// <summary>
		///     The size of each page.
		/// </summary>
		[PublicAPI]
		new int PageSize { get; set; }
	}

	/// <summary>
	///     Extend <see cref="IPagedList" /> in order to provide strong-typed data access.
	/// </summary>
	/// <typeparam name="T">The element type in the data page.</typeparam>
	// ReSharper disable once PossibleInterfaceMemberAmbiguity
	public interface IDynamicPagedList<out T> : IDynamicPagedList, IPagedList<T>
	{
	}
}