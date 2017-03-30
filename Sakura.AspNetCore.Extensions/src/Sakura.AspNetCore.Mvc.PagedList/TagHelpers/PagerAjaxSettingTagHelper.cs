using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	public class PagerAjaxSettingTagHelper : PagerSettingTagHelper
	{
		/// <inheritdoc />
		public override IEnumerable<KeyValuePair<string, string>> GetSettings()
		{
			var dic = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		}

		[HtmlAttributeName(IsAjaxEnabledAttributeName)]
		public bool IsAjaxEnabled { get; set; }

		public const string IsAjaxEnabledAttributeName = "ajax";
	}
}
