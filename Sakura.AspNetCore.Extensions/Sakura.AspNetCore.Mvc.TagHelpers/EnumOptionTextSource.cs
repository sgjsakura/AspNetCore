using System.ComponentModel.DataAnnotations;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	/// <summary>
	///     Define the text source for <see cref="EnumSelectTagHelper" />.
	/// </summary>
	/// <seealso cref="EnumSelectTagHelper" />
	public enum EnumOptionTextSource
	{
		/// <summary>
		///     Use the enum name as the text.
		/// </summary>
		EnumNameOnly = 0,

		/// <summary>
		///     If <see cref="DisplayAttribute" /> is defined and <see cref="DisplayAttribute.Name" /> is provided, use its value;
		///     Otherwise, fallback to the <see cref="EnumNameOnly" /> option.
		/// </summary>
		Name,

		/// <summary>
		///     If <see cref="DisplayAttribute" /> is defined and <see cref="DisplayAttribute.ShortName" /> is provided, use its
		///     value; Otherwise, fallback to the <see cref="EnumNameOnly" /> option.
		/// </summary>
		ShortName,

		/// <summary>
		///     If <see cref="DisplayAttribute" /> is defined and <see cref="DisplayAttribute.Description" /> is provided, use its
		///     value; Otherwise, fallback to the <see cref="EnumNameOnly" /> option.
		/// </summary>
		Description
	}
}