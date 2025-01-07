using System;
using JetBrains.Annotations;
using Sakura.AspNetCore.Mvc.Internal;

namespace Sakura.AspNetCore.Mvc.Generators;

/// <summary>
///     Generate the pager link text with simple query parameter name and values.
/// </summary>
public class SimpleQueryValueLinkGenerator : QueryValueLinkGenerator
{
	/// <summary>
	///     Initialize a new instance of the generator.
	/// </summary>
	/// <param name="queryParameterName">The query parameter name when generating the link URL.</param>
	/// <param name="queryParameterValue">The query parameter value when generating the link URL.</param>
	/// <exception cref="ArgumentNullException">
	///     The <paramref name="queryParameterName" /> or
	///     <paramref name="queryParameterValue" /> is <c>null</c>.
	/// </exception>
	public SimpleQueryValueLinkGenerator([NotNull] string queryParameterName, [NotNull] string queryParameterValue)
		: base(queryParameterName)
	{
		QueryParameterValue = queryParameterValue ?? throw new ArgumentNullException(nameof(queryParameterValue));
	}

	/// <summary>
	///     Get the generated query parameter value.
	/// </summary>
	[PublicAPI]
	[NotNull]
	public string QueryParameterValue { get; }

	/// <summary>
	///     Generate the query parameter value for the specified <see cref="PagerItem" />.
	/// </summary>
	/// <param name="context">The generation context.</param>
	/// <returns>The query parameter name for current pager item.</returns>
	public override string GenerateQueryParameterValue(PagerItemGenerationContext context)
	{
		return QueryParameterValue;
	}
}