using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Localization;

namespace Sakura.AspNetCore.Localization.Internal
{
	/// <summary>
	/// Provide the dynamic style implementation for <see cref="IHtmlLocalizer"/> object.
	/// </summary>
	public class DynamicHtmlLocalizer : DynamicObject
	{
		/// <summary>
		/// Initialize a new instance of <see cref="DynamicHtmlLocalizer"/> object.
		/// </summary>
		/// <param name="innerLocalizer">The internal <see cref="IHtmlLocalizer"/> object.</param>
		public DynamicHtmlLocalizer([NotNull] IHtmlLocalizer innerLocalizer)
		{
			InnerLocalizer = innerLocalizer ?? throw new ArgumentNullException(nameof(innerLocalizer));
		}

		/// <summary>
		/// Get the internal <see cref="IHtmlLocalizer"/> service.
		/// </summary>
		private IHtmlLocalizer InnerLocalizer { get; }

		/// <inheritdoc />
		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			result = InnerLocalizer[binder.Name];
			return true;
		}

		/// <inheritdoc />
		public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
		{
			result = InnerLocalizer[binder.Name, args];
			return true;
		}

		/// <inheritdoc />
		public override IEnumerable<string> GetDynamicMemberNames() => InnerLocalizer.GetAllStrings().Select(i => i.Name);
	}
}