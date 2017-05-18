using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Microsoft.Extensions.Localization;

namespace Sakura.AspNetCore.Localization.Internal
{
	/// <summary>
	///     Provide the dynamic style implementation for <see cref="IStringLocalizer" /> object.
	/// </summary>
	public class DynamicStringLocalizerWrapper : DynamicObject
	{
		/// <summary>
		///     Initialize a new instance of <see cref="DynamicStringLocalizerWrapper" /> object.
		/// </summary>
		/// <param name="innerLocalizer">The internal <see cref="IStringLocalizer" /> object.</param>
		public DynamicStringLocalizerWrapper(IStringLocalizer innerLocalizer)
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
		public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
		{
			if (indexes.Length == 0)
				throw new ArgumentException(nameof(indexes), "The length of index array cannot be zero.");

			if (indexes[0] is string name)
				result = InnerLocalizer[name, indexes.Skip(1).ToArray()];
			else
				throw new ArgumentException(nameof(indexes), "The first index value must be a string.");

			return true;
		}

		/// <inheritdoc />
		public override IEnumerable<string> GetDynamicMemberNames()
		{
			return InnerLocalizer.GetAllStrings().Select(i => i.Name);
		}
	}
}