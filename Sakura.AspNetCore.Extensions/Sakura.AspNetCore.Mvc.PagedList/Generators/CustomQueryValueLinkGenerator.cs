using System;
using JetBrains.Annotations;
using Sakura.AspNetCore.Mvc.Internal;

namespace Sakura.AspNetCore.Mvc.Generators
{
	/// <summary>
	///     Generate the query parameter value using a custom method.
	/// </summary>
	public class CustomQueryValueLinkGenerator : QueryValueLinkGenerator
	{
		/// <summary>
		///     Initialize a new instance of the generator.
		/// </summary>
		/// <param name="queryParameterName">The query parameter name when generating the link URL.</param>
		/// <param name="queryValueGenerator">The query parameter value generating method delegate.</param>
		/// <exception cref="ArgumentNullException">
		///     The <paramref name="queryParameterName" /> or
		///     <paramref name="queryValueGenerator" /> is <c>null</c>.
		/// </exception>
		public CustomQueryValueLinkGenerator([NotNull] string queryParameterName,
			[NotNull] Func<PagerItemGenerationContext, string> queryValueGenerator) : base(queryParameterName)
		{
			QueryValueGenerator = queryValueGenerator ?? throw new ArgumentNullException(nameof(queryValueGenerator));
		}

		/// <summary>
		///     Get the query parameter value generating method delegate.
		/// </summary>
		[NotNull]
		public Func<PagerItemGenerationContext, string> QueryValueGenerator { get; }

		/// <summary>
		///     When derived, generate the query parameter value for the specified <see cref="PagerItem" />.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <returns>The query parameter name for current pager item.</returns>
		public override string GenerateQueryParameterValue(PagerItemGenerationContext context)
		{
			return QueryValueGenerator(context);
		}
	}
}