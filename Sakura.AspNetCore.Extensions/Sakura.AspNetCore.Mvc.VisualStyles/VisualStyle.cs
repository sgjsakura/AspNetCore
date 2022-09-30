﻿using Sakura.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sakura.AspNetCore.Mvc
{
	/// <summary>
	/// Define the common visual style names. This class is static.
	/// </summary>
	public class VisualStyle : IEquatable<VisualStyle>
	{
		/// <summary>
		/// The name of the UI framework. 
		/// </summary>
		public string Framework { get; }

		/// <summary>
		/// The version fo the UI Framework.
		/// </summary>
		public string Version { get; }

		/// <summary>
		/// Initialize a new instance of <see cref="VisualStyle"/>.
		/// </summary>
		/// <param name="framework">The name of the framework UI.</param>
		/// <param name="version">The version of the framework UI.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public VisualStyle(string framework, string version)
		{

			Framework = framework ?? throw new ArgumentNullException(nameof(framework));
			Version = version ?? throw new ArgumentNullException(nameof(version));
		}

		/// <summary>
		/// Provide command UI framework names. This class is static.
		/// </summary>
		public static class CommonFrameworks
		{
			/// <summary>
			/// The Bootstrap framework UI.
			/// </summary>
			public const string Bootstrap = "Bootstrap";
		}

		/// <summary>
		/// Bootstrap 4.
		/// </summary>
		public static VisualStyle Bootstrap4 { get; } = new VisualStyle(CommonFrameworks.Bootstrap, "4");

		/// <summary>
		/// Bootstrap 5.
		/// </summary>
		public static VisualStyle Bootstrap5 { get; } = new VisualStyle(CommonFrameworks.Bootstrap, "5");

		/// <summary>
		/// Try match 2 visual styles.
		/// </summary>
		/// <param name="first">The first visual style to be matching.</param>
		/// <param name="second">The second visual style to be matching.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static VisualStyleMatchType TryMatch(VisualStyle first, VisualStyle second)
		{
			if (first == null) throw new ArgumentNullException(nameof(first));
			if (second == null) throw new ArgumentNullException(nameof(second));

			if (string.Equals(first.Framework, second.Framework))
			{
				return string.Equals(first.Version, second.Version)
					? VisualStyleMatchType.Exact
					: VisualStyleMatchType.Framework;
			}

			return VisualStyleMatchType.None;
		}

		#region Equality

		/// <inheritdoc />
		public bool Equals(VisualStyle other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return string.Equals(Framework, other.Framework, StringComparison.OrdinalIgnoreCase) && string.Equals(Version, other.Version, StringComparison.OrdinalIgnoreCase);
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((VisualStyle)obj);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			unchecked
			{
				return (StringComparer.OrdinalIgnoreCase.GetHashCode(Framework) * 397) ^ StringComparer.OrdinalIgnoreCase.GetHashCode(Version);
			}
		}

		public static bool operator ==(VisualStyle left, VisualStyle right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(VisualStyle left, VisualStyle right)
		{
			return !Equals(left, right);
		}

		#endregion
	}
}

/// <summary>
/// Provide options for visual style settings.
/// </summary>
public class VisualStyleOptions
{
	/// <summary>
	/// The preferred visual styles. Item orders indicate the preference priority.
	/// </summary>
	public IList<VisualStyle> PreferredStyles { get; } = new Collection<VisualStyle>();


	/// <summary>
	/// The minimal match level for the style selection.
	/// </summary>
	public VisualStyleMatchType MinimalMatchLevel { get; set; } = VisualStyleMatchType.None;
}

/// <summary>
/// Provide service for visual style detection and selection.
/// </summary>
public class VisualStyleService
{
	public VisualStyleService(IOptions<VisualStyleOptions> options)
	{
		Options = options;
	}

	private VisualStyleOptions Options { get; }

	public VisualStyleMatchResult Match(IEnumerable<VisualStyle> allowedStyles)
	{
		var result =
			from i in Options.PreferredStyles
			from j in allowedStyles
			let matchType = VisualStyle.TryMatch(i, j)
			where matchType >= Options.MinimalMatchLevel
			orderby matchType descending
			select new VisualStyleMatchResult(i, j, matchType);

		return result.FirstOrDefault();
	}
}

public class VisualStyleMatchResult
{
	public VisualStyle RequiredStyle { get; }

	public VisualStyle SelectedStyle { get; }

	public VisualStyleMatchType MatchType { get; }

	public VisualStyleMatchResult(VisualStyle requiredStyle, VisualStyle selectedStyle, VisualStyleMatchType matchType)
	{
		RequiredStyle = requiredStyle;
		SelectedStyle = selectedStyle;
		MatchType = matchType;
	}

}

public enum VisualStyleMatchType
{
	None,
	Framework,
	Exact,
}
