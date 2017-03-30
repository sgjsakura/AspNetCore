using System;
using System.Collections.Generic;
using System.Text;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	/// <summary>
	/// Provide the AJAX options.
	/// </summary>
    public class AjaxOptions
    {
		public bool Enabled { get; set; }

		public AjaxUpdateMode UpdateMode { get; set; }
    }

	public enum AjaxUpdateMode
	{
		Before,
		After,
		ReplaceWith
	}
}
