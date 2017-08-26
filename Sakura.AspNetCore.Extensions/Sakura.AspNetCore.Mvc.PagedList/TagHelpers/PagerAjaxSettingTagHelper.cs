using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	public class PagerAjaxSettingTagHelper : PagerSettingTagHelper
	{
		public const string IsAjaxEnabledAttributeName = "ajax";

		[HtmlAttributeName(IsAjaxEnabledAttributeName)]
		public bool IsAjaxEnabled { get; set; }

		/// <inheritdoc />
		public override void ApplySettings(IDictionary<string, string> settings)
		{
			throw new NotImplementedException();
		}
	}
}