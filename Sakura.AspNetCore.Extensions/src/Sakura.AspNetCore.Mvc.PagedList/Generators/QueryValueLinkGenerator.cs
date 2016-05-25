using System;
using JetBrains.Annotations;
using Sakura.AspNetCore.Mvc.Internal;

namespace Sakura.AspNetCore.Mvc.Generators
{
	/// <summary>
	///     Represent as a base class for all <see cref="QueryStringLinkGenerator" /> that use fixed query parameter name.
	/// </summary>
	public abstract class QueryValueLinkGenerator : QueryStringLinkGenerator
	{
		/// <summary>
		///     Initialize a new instance of the generator.
		/// </summary>
		/// <param name="queryParameterName">The query parameter name when generating the link URL.</param>
		/// <exception cref="ArgumentNullException">The <paramref name="queryParameterName" /> is <c>null</c>.</exception>
		protected QueryValueLinkGenerator([NotNull] string queryParameterName)
		{
			if (queryParameterName == null)
			{
				throw new ArgumentNullException(nameof(queryParameterName));
			}

			QueryParameterName = queryParameterName;
		}

		/// <summary>
		///     Get or set the query parameter name for the generated URL.
		/// </summary>
		[PublicAPI]
		[NotNull]
		public string QueryParameterName { get; }

		/// <summary>
		///     When derived, generate the query parameter name for the specified <see cref="PagerItem" />.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <returns>The query parameter name for current pager item.</returns>
		public override string GenerateQueryParameterName(PagerItemGenerationContext context) => QueryParameterName;
	}
}