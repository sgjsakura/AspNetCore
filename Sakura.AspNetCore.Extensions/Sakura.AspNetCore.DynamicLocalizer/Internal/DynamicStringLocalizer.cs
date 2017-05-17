using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Microsoft.Extensions.Localization;

namespace Sakura.AspNetCore.Localization.Internal
{
	/// <summary>
	///     Provide the dynamic style implementation for <see cref="IStringLocalizer" /> object.
	/// </summary>
	public class DynamicStringLocalizer : DynamicObject
	{
		/// <summary>
		///     Initialize a new instance of <see cref="DynamicStringLocalizer" /> object.
		/// </summary>
		/// <param name="innerLocalizer">The internal <see cref="IStringLocalizer" /> object.</param>
		public DynamicStringLocalizer(IStringLocalizer innerLocalizer)
		{
			InnerLocalizer = innerLocalizer;
		}

		/// <summary>
		///     Get the internal <see cref="IStringLocalizer" /> service.
		/// </summary>
		private IStringLocalizer InnerLocalizer { get; }

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
		public override IEnumerable<string> GetDynamicMemberNames()
		{
			return InnerLocalizer.GetAllStrings().Select(i => i.Name);
		}
	}
}