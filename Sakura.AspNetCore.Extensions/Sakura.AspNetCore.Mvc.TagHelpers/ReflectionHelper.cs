using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.Localization;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	/// <summary>
	///     Provide supporting method to reflection operations. This class is static.
	/// </summary>
	[PublicAPI]
	public static class ReflectionHelper
	{
		/// <summary>
		///     Get the <see cref="FieldInfo" /> definition for an enum value object.
		/// </summary>
		/// <param name="obj">The enum value object.</param>
		/// <returns>The corresponding <see cref="FieldInfo" /> definition.</returns>
		public static FieldInfo GetMember(this Enum obj)
		{
			var enumType = obj.GetType();
			var name = Enum.GetName(enumType, obj);
			return enumType.GetField(name);
		}

		/// <summary>
		///     Get the display text of an <see cref="MemberInfo" /> from the specified text source.
		/// </summary>
		/// <param name="memberInfo">The <see cref="MemberInfo" /> object.</param>
		/// <param name="textSource">The text source for the enum item.</param>
		/// <returns>
		///     The text retrieved from the <paramref name="memberInfo" /> definition. If there is no text in the location
		///     <paramref name="textSource" /> specified, this method will return <see cref="MemberInfo.Name" />.
		/// </returns>
		/// <exception cref="ArgumentException">The value of <paramref name="textSource" /> is not a valid enum item.</exception>
		public static string GetTextForMember(this MemberInfo memberInfo, TextSource textSource)
		{
			// Get DisplayAttribute instance
			var attr = memberInfo.GetCustomAttribute<DisplayAttribute>();

			// No attribute is defined, return name immediately
			if (attr == null)
				return memberInfo.Name;

			// Variable to store the result
			string result;

			// Get data according to the source
			switch (textSource)
			{
				case TextSource.MemberNameOnly:
					result = memberInfo.Name;
					break;
				case TextSource.Name:
					result = attr.GetName();
					break;
				case TextSource.ShortName:
					result = attr.GetShortName();
					break;
				case TextSource.Description:
					result = attr.GetDescription();
					break;
				default:
					throw new ArgumentException("The argument value is not a valid enum item.", nameof(textSource));
			}

			// No result it found from the source, fallback to name
			if (string.IsNullOrEmpty(result))
				result = memberInfo.Name;

			// Return
			return result;
		}

		/// <summary>
		///     Get the value text of an enum item from the specified value source.
		/// </summary>
		/// <param name="memberInfo">The <see cref="MemberInfo" /> object represented as an enum item.</param>
		/// <param name="valueSource">The value source for the enum item.</param>
		/// <returns>The text retrieved from the <paramref name="memberInfo" /> definition.</returns>
		/// <exception cref="ArgumentException">The value of <paramref name="valueSource" /> is not a valid enum item.</exception>
		public static string GetValueForEnumMember(this MemberInfo memberInfo, EnumOptionValueSource valueSource)
		{
			switch (valueSource)
			{
				case EnumOptionValueSource.Value:
					return ((int) Enum.Parse(
						memberInfo.DeclaringType ?? throw new InvalidOperationException("The enum member has no declaring type."),
						memberInfo.Name)).ToString("D", CultureInfo.InvariantCulture);
				case EnumOptionValueSource.Name:
					return memberInfo.Name;
				default:
					throw new ArgumentException("The argument value is not a valid enum item.", nameof(valueSource));
			}
		}
	}
}