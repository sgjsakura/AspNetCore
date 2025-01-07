using System;
using System.ComponentModel;
using System.Globalization;

namespace Sakura.AspNetCore.Mvc.Internal;

/// <summary>
///     Convert from <see cref="string" /> to <see cref="IPagerItemContentGenerator" /> instances.
/// </summary>
public class PagerItemContentGeneratorConverter : TypeConverter
{
	/// <summary>
	///     返回该转换器是否可以使用指定的上下文将给定类型的对象转换为此转换器的类型。
	/// </summary>
	/// <returns>
	///     如果该转换器能够执行转换，则为 true；否则为 false。
	/// </returns>
	/// <param name="context">一个 <see cref="T:System.ComponentModel.ITypeDescriptorContext" />，提供格式上下文。</param>
	/// <param name="sourceType">一个 <see cref="T:System.Type" />，表示要转换的类型。</param>
	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
	{
		return sourceType == typeof(string);
	}

	/// <summary>
	///     使用指定的上下文和区域性信息将给定的对象转换为此转换器的类型。
	/// </summary>
	/// <returns>
	///     表示转换的 value 的 <see cref="T:System.Object" />。
	/// </returns>
	/// <param name="context">一个 <see cref="T:System.ComponentModel.ITypeDescriptorContext" />，提供格式上下文。</param>
	/// <param name="culture">用作当前区域性的 <see cref="T:System.Globalization.CultureInfo" />。</param>
	/// <param name="value">要转换的 <see cref="T:System.Object" />。</param>
	/// <exception cref="T:System.NotSupportedException">不能执行转换。</exception>
	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		var realValue = value as string;

		if (realValue == null)
			throw new NotSupportedException();

		return PagerItemContentGenerators.FromConfiguration(realValue);
	}
}