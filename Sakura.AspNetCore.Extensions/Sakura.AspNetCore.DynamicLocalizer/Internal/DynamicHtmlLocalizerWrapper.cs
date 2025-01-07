using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Localization;

namespace Sakura.AspNetCore.Localization.Internal;

/// <summary>
///     Provide the dynamic style implementation for <see cref="IHtmlLocalizer" /> object.
/// </summary>
public class DynamicHtmlLocalizerWrapper : DynamicObject, IDynamicLocalizerWrapper
{
	/// <summary>
	///     Initialize a new instance of <see cref="DynamicHtmlLocalizerWrapper" /> object.
	/// </summary>
	/// <param name="innerLocalizer">The internal <see cref="IHtmlLocalizer" /> object.</param>
	public DynamicHtmlLocalizerWrapper([NotNull] IHtmlLocalizer innerLocalizer)
	{
		InnerLocalizer = innerLocalizer ?? throw new ArgumentNullException(nameof(innerLocalizer));
	}

	/// <summary>
	///     Get the internal <see cref="IHtmlLocalizer" /> service.
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