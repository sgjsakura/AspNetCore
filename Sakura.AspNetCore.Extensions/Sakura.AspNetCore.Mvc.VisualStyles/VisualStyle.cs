using System;

namespace Sakura.AspNetCore.Mvc;

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