using Microsoft.Extensions.Localization;

namespace Sakura.AspNetCore.Localization
{
	/// <summary>
	///     Provide strong typed resource class for dynamic style text resource accessing.
	/// </summary>
	/// <typeparam name="TResource">The resource type.</typeparam>
	public interface IDynamicStringLocalizer<TResource> : IDynamicStringLocalizer
	{
		/// <summary>
		///     Get the internal <see cref="IStringLocalizer{T}" /> service instance.
		/// </summary>
		new IStringLocalizer<TResource> Localizer { get; }
	}

	/// <summary>
	///     Define the necessary feature for dynamic style text resource accessing.
	/// </summary>
	public interface IDynamicStringLocalizer
	{
		/// <summary>
		///     Get the dynamic object used to access resource strings as text format.
		/// </summary>
		dynamic Text { get; }

		/// <summary>
		///     Get the internal <see cref="IStringLocalizer" /> service instance.
		/// </summary>
		IStringLocalizer Localizer { get; }
	}
}