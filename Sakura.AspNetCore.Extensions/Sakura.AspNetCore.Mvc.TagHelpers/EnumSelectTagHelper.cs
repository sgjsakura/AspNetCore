using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	/// <summary>
	///     Generate the item list for a "select" element with all items defined in an enum type.
	/// </summary>
	public abstract class EnumSelectTagHelper : TagHelper
	{
		#region Constructor

		/// <summary>
		/// Create a new instance of <see cref="EnumSelectTagHelper"/>.
		/// </summary>
		/// <param name="stringLocalizerFactory">The service instance of <see cref="IStringLocalizerFactory"/>.</param>
		protected EnumSelectTagHelper(IStringLocalizerFactory stringLocalizerFactory)
		{
			StringLocalizerFactory = stringLocalizerFactory;
		}

		#endregion

		private IStringLocalizerFactory StringLocalizerFactory { get; }

		#region Services

		/// <summary>
		/// Get the <see cref="IStringLocalizer"/> object used for localization.
		/// </summary>
		protected IStringLocalizer EnumTypeStringLocalizer => StringLocalizerFactory?.Create(GetEnumType());

		#endregion


		#region Abstract Methods

		/// <summary>
		///     When derived, return the actual enum type for generating the list.
		/// </summary>
		/// <returns></returns>
		protected abstract Type GetEnumType();

		#endregion

		/// <summary>
		///     Generate a list of <see cref="SelectListItem" /> for a specified enum type.
		/// </summary>
		/// <returns>The generated list.</returns>
		protected virtual IEnumerable<SelectListItem> GenerateListForEnumType()
		{
			return GetEnumType()
				.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static)
				.Select(GetItemForMember);
		}

		#region Constants for attributes

		/// <summary>
		///     Get the HTML name associated with the <see cref="TextSource" /> property. This field is constant.
		/// </summary>
		[PublicAPI] public const string TextSourceAttributeName = "asp-text-source";

		/// <summary>
		///     Get the HTML name associated with the <see cref="ValueSource" /> property. This field is constant.
		/// </summary>
		[PublicAPI] public const string ValueSourceAttributeName = "asp-value-source";

		/// <summary>
		///     Get or set the text source for the generated options. The default value of this property is
		///     <see cref="EnumOptionTextSource.EnumNameOnly" />.
		/// </summary>
		[HtmlAttributeName(TextSourceAttributeName)]
		public EnumOptionTextSource TextSource { get; set; } = EnumOptionTextSource.EnumNameOnly;

		/// <summary>
		///     Get or set the value source for the generated options. The default value of this property is
		///     <see cref="EnumOptionValueSource.Name" />.
		/// </summary>
		[HtmlAttributeName(ValueSourceAttributeName)]
		public EnumOptionValueSource ValueSource { get; set; } = EnumOptionValueSource.Name;

		#endregion

		#region Helper Method

		/// <summary>
		///     Get the text of the option associated with the specified enum item.
		/// </summary>
		/// <param name="memberInfo">The <see cref="MemberInfo" /> object represents as the enum item.</param>
		/// <returns><paramref name="memberInfo" />The option text associated with <paramref name="memberInfo" />.</returns>
		protected virtual string GetTextForMember(MemberInfo memberInfo)
		{
			var memberText = memberInfo.GetTextForMember(TextSource);
			return EnumTypeStringLocalizer != null ? EnumTypeStringLocalizer[memberText] : memberText;
		}

		/// <summary>
		///     Get the value of the option associated with the specified enum item.
		/// </summary>
		/// <param name="memberInfo">The <see cref="MemberInfo" /> object represents as the enum item.</param>
		/// <returns><paramref name="memberInfo" />The option value associated with <paramref name="memberInfo" />.</returns>
		protected virtual string GetValueForMember(MemberInfo memberInfo)
		{
			return memberInfo.GetValueForMember(ValueSource);
		}

		/// <summary>
		///     Generate a <see cref="SelectListItem" /> for a specified <see cref="MemberInfo" />.
		/// </summary>
		/// <param name="memberInfo">The <see cref="MemberInfo" /> object represents as the enum item.</param>
		/// <returns></returns>
		protected virtual SelectListItem GetItemForMember(MemberInfo memberInfo)
		{
			return new SelectListItem
			{
				Text = GetTextForMember(memberInfo),
				Value = GetValueForMember(memberInfo)
			};
		}

		#endregion
	}
}