using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sakura.AspNetCore.Mvc.TagHelpers
{
	/// <summary>
	/// Provide Utility methods for tag helpers. This class is static.
	/// </summary>
	internal static class TagHelperUtility
	{
		/// <summary>
		/// Check the <see cref="TagHelperContext"/> to ensure the specified attribute is not set before.
		/// </summary>
		/// <param name="context">The <see cref="TagHelperContext"/> instance.</param>
		/// <param name="attributeName">The attribute name be checking.</param>
		public static void CheckAttributeConflicting(this TagHelperContext context, string attributeName)
		{
			if (context.AllAttributes.ContainsName(attributeName))
			{
				throw new InvalidOperationException(
					$"The '{attributeName}' attribute has already set from code explicitly or from another tag helper.");
			}
		}
	}
}