using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;

namespace Sakura.AspNetCore.Localization
{
	/// <summary>
	///     Provide abilities to access localizers as dynamic objects.
	/// </summary>
	public interface IDynamicLocalizerFactory
	{
		/// <summary>
		///     Get a dynamic object that can be used to access localizble strings similar as <see cref="IViewLocalizer" /> type.
		/// </summary>
		/// <typeparam name="T">The resource type.</typeparam>
		/// <returns>A dynamic object that can be used to access localizble strings.</returns>
		dynamic View { get; }

		/// <summary>
		///     Get a dynamic object that can be used to access localizble strings similar as
		///     <see cref="IHtmlLocalizer{TResource}" /> type.
		/// </summary>
		/// <typeparam name="T">The resource type.</typeparam>
		/// <returns>A dynamic object that can be used to access localizble strings.</returns>
		dynamic Html<T>();

		/// <summary>
		///     Get a dynamic object that can be used to access localizble strings similar as <see cref="IStringLocalizer{T}" />
		///     type.
		/// </summary>
		/// <typeparam name="T">The resource type.</typeparam>
		/// <returns>A dynamic object that can be used to access localizble strings.</returns>
		dynamic Text<T>();
	}
}