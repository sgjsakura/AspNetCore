using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	public class PagerAjaxSettingTagHelper : PagerSettingTagHelper
	{
		/// <inheritdoc />
		public override void ApplySettings(IDictionary<string, string> settings)
		{
			throw new NotImplementedException();
		}

		[HtmlAttributeName(IsAjaxEnabledAttributeName)]
		public bool IsAjaxEnabled { get; set; }

		public const string IsAjaxEnabledAttributeName = "ajax";
	}
}
