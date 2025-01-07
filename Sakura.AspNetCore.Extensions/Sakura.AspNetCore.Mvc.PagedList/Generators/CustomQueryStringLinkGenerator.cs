using System;
using JetBrains.Annotations;
using Sakura.AspNetCore.Mvc.Internal;

namespace Sakura.AspNetCore.Mvc.Generators;

/// <summary>
///     Represent as a pager item link generator that append a query parameter to the current url with custom parameter
///     name and value generators.
/// </summary>
public class CustomQueryStringLinkGenerator : QueryStringLinkGenerator
{
	/// <summary>
	///     Create a new instance with specified information.
	/// </summary>
	/// <param name="queryNameGenerator">The query parameter name generating method delegate.</param>
	/// <param name="queryValueGenerator">The query parameter value generating method delegate.</param>
	/// <exception cref="ArgumentNullException">
	///     The <paramref name="queryNameGenerator" /> or
	///     <paramref name="queryValueGenerator" /> is <c>null</c>.
	/// </exception>
	public CustomQueryStringLinkGenerator([NotNull] Func<PagerItemGenerationContext, string> queryNameGenerator,
		[NotNull] Func<PagerItemGenerationContext, string> queryValueGenerator)
	{
		QueryNameGenerator = queryNameGenerator ?? throw new ArgumentNullException(nameof(queryNameGenerator));
		QueryValueGenerator = queryValueGenerator ?? throw new ArgumentNullException(nameof(queryValueGenerator));
	}

	/// <summary>
	///     Get the query parameter name generating method delegate.
	/// </summary>
	[PublicAPI]
	public Func<PagerItemGenerationContext, string> QueryNameGenerator { get; }

	/// <summary>
	///     Get the query parameter value generating method delegate.
	/// </summary>
	[PublicAPI]
	public Func<PagerItemGenerationContext, string> QueryValueGenerator { get; }

	/// <summary>
	///     Generate the query parameter name for the specified <see cref="PagerItem" />.
	/// </summary>
	/// <param name="context">The generation context.</param>
	/// <returns>The query parameter name for current pager item.</returns>
	public override string GenerateQueryParameterName(PagerItemGenerationContext context)
	{
		return QueryNameGenerator(context);
	}

	/// <summary>
	///     Generate the query parameter value for the specified <see cref="PagerItem" />.
	/// </summary>
	/// <param name="context">The generation context.</param>
	/// <returns>The query parameter name for current pager item.</returns>
	public override string GenerateQueryParameterValue(PagerItemGenerationContext context)
	{
		return QueryValueGenerator(context);
	}
}