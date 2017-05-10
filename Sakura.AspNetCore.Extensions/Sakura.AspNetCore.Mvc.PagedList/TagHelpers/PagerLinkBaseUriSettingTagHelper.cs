using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	public abstract class PagerLinkBaseUriSettingTagHelper : PagerSettingTagHelper
	{
		public const string BaseUriSettingName = "base-uri";

		protected abstract string GetBaseUri();

		/// <inheritdoc />
		public override void ApplySettings(IDictionary<string, string> settings)
		{
			if (settings.ContainsKey(BaseUriSettingName))
			{
				throw new InvalidOperationException($"The '{nameof(BaseUriSettingName)}' setting has already be set by another tag helper.");
			}

			settings[BaseUriSettingName] = GetBaseUri();
		}
	}
}
