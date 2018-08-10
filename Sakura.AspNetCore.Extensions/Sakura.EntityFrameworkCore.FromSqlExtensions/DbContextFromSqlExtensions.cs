using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Sakura.EntityFrameworkCore.FromSqlExtensions
{
	/// <summary>
	///     Provide extension methods which can be used to execute parameterized SQL statements and retrieve the result data
	///     set directly. This class is static.
	/// </summary>
	public static class DbContextFromSqlExtensions
	{
		/// <summary>
		///     The runtime reference of the
		///     <see cref="RelationalQueryableExtensions.FromSql{T}(IQueryable{T}, RawSqlString, object[])" /> method.
		/// </summary>
		private static MethodInfo FromSqlMethod { get; } = typeof(RelationalQueryableExtensions).GetTypeInfo()
			.GetDeclaredMethods(nameof(RelationalQueryableExtensions.FromSql))
			.Single(i => i.GetParameters().Length == 3);

		/// <summary>
		///     Executing a SQL statement and retrieve the result set directly frm the specified <see cref="DbContext" />.
		/// </summary>
		/// <typeparam name="T">The type of the item in the result set.</typeparam>
		/// <param name="dbContext">The <see cref="DbContext" /> instance.</param>
		/// <param name="sql">The SQL statement to be executing.</param>
		/// <param name="parameters">Arguments provided along with the parameterized <paramref name="sql" />.</param>
		/// <returns>A <see cref="IQueryable{T}" /> instance which can be used for retrieving the result or making a further query.</returns>
		[PublicAPI]
		public static IQueryable<T> FromSql<T>(this DbContext dbContext, RawSqlString sql, params object[] parameters)
		{
			// Argument check
			if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));

			// Add new query type if not exists
			dbContext.TryAddQueryType<T>();

			// Get the query provider service instance
			var queryProvider = dbContext.GetDependencies().QueryProvider;

			// Build query source. Actually query source is just used to determine the final result item type.
			var querySource = new EntityQueryable<T>(queryProvider);

			// Build LINQ expression for Entity Framework Core.
			var expression = Expression.Call(null, FromSqlMethod.MakeGenericMethod(typeof(T)), querySource.Expression,
				Expression.Constant(sql), Expression.Constant(parameters));

			// Return result
			return queryProvider.CreateQuery<T>(expression);
		}

		/// <summary>
		///     Executing a SQL statement and retrieve the result set directly frm the specified <see cref="DbContext" />.
		/// </summary>
		/// <typeparam name="T">The type of the item in the result set.</typeparam>
		/// <param name="dbContext">The <see cref="DbContext" /> instance.</param>
		/// <param name="sql">The parameterized SQL statement represented as a <see cref="FormattableString" />.</param>
		/// <returns>A <see cref="IQueryable{T}" /> instance which can be used for retrieving the result or making a further query.</returns>
		[PublicAPI]
		public static IQueryable<T> FromSql<T>(this DbContext dbContext, FormattableString sql)
		{
			return dbContext.FromSql<T>(sql.Format, sql.GetArguments());
		}

		/// <summary>
		///     Try to add a new query type into the <see cref="DbContext" /> if it not exists yet.
		/// </summary>
		/// <typeparam name="T">The query type should be adding.</typeparam>
		/// <param name="dbContext">The <see cref="DbContext" /> instance.</param>
		private static void TryAddQueryType<T>(this DbContext dbContext)
		{
			var model = dbContext.Model.AsModel();
			var type = typeof(T);
			if (model.FindEntityType(type) == null) model.AddQueryType(type);
		}
	}
}