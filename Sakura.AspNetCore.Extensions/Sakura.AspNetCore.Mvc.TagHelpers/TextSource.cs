using System.ComponentModel.DataAnnotations;

namespace Sakura.AspNetCore.Mvc.TagHelpers;

/// <summary>
///     Define the text source for text generators for a relection items.
/// </summary>
public enum TextSource
{
	/// <summary>
	///     Use the member name as the text.
	/// </summary>
	MemberNameOnly = 0,

	/// <summary>
	///     If <see cref="DisplayAttribute" /> is defined and <see cref="DisplayAttribute.Name" /> is provided, use its value;
	///     Otherwise, fallback to the <see cref="MemberNameOnly" /> option.
	/// </summary>
	Name,

	/// <summary>
	///     If <see cref="DisplayAttribute" /> is defined and <see cref="DisplayAttribute.ShortName" /> is provided, use its
	///     value; Otherwise, fallback to the <see cref="MemberNameOnly" /> option.
	/// </summary>
	ShortName,

	/// <summary>
	///     If <see cref="DisplayAttribute" /> is defined and <see cref="DisplayAttribute.Description" /> is provided, use its
	///     value; Otherwise, fallback to the <see cref="MemberNameOnly" /> option.
	/// </summary>
	Description
}