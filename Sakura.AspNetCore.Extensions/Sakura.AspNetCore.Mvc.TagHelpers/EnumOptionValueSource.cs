namespace Sakura.AspNetCore.Mvc.TagHelpers;

/// <summary>
///     Define the value source for <see cref="EnumSelectTagHelper" />.
/// </summary>
/// <seealso cref="EnumSelectTagHelper" />
public enum EnumOptionValueSource
{
	/// <summary>
	///     Use the name of enum item as the value source.
	/// </summary>
	Name = 0,

	/// <summary>
	///     Use the integer value of the enum item as the value source.
	/// </summary>
	Value
}