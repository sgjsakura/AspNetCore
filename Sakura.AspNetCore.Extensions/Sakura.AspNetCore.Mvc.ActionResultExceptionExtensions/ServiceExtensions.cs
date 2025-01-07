using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Sakura.AspNetCore.Mvc;

/// <summary>
///     Provide extension methods for register <see cref="EnableActionResultExceptionAttribute" /> globally. This class is
///     static.
/// </summary>
public static class ServiceExtensions
{
	/// <summary>
	///     Add <see cref="EnableActionResultExceptionAttribute" /> as a exception filter in the
	///     <paramref name="filterCollection" />.
	/// </summary>
	/// <param name="filterCollection">The <see cref="FilterCollection" /> object.</param>
	/// <exception cref="ArgumentNullException">The <paramref name="filterCollection" /> is <c>null</c>.</exception>
	public static void EnableActionResultExceptionFilter(this FilterCollection filterCollection)
	{
		if (filterCollection == null)
			throw new ArgumentNullException(nameof(filterCollection));

		filterCollection.Add(typeof(EnableActionResultExceptionAttribute));
	}

	/// <summary>
	///     Add <see cref="EnableActionResultExceptionAttribute" /> as a exception filter in the <paramref name="mvcOptions" />
	///     .
	/// </summary>
	/// <param name="mvcOptions">The <see cref="MvcOptions" /> object.</param>
	/// <exception cref="ArgumentNullException">The <paramref name="mvcOptions" /> is <c>null</c>.</exception>
	public static void EnableActionResultExceptionFilter(this MvcOptions mvcOptions)
	{
		if (mvcOptions == null)
			throw new ArgumentNullException(nameof(mvcOptions));

		mvcOptions.Filters.EnableActionResultExceptionFilter();
	}
}