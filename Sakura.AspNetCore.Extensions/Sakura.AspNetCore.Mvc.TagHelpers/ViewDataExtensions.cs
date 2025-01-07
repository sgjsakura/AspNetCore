using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Sakura.AspNetCore.Mvc.TagHelpers;

/// <summary>
/// Provide extension methods for <see cref="ViewDataDictionary"/> data extracting. This class is static.
/// </summary>
[PublicAPI]
public static class ViewDataExtensions
{
    /// <summary>
    /// Try getting the value of type <typeparamref name="T"/> stored in the <paramref name="viewData"/> with the specified <paramref name="key"/>. If Value is not found or cannot be converted to the target type, return the default value of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="viewData">The view data dictionary.</param>
    /// <param name="key">The key of the value.</param>
    /// <returns>If the item with the specified <paramref name="key"/> is found in the <paramref name="viewData"/> and its type is <typeparamref name="T"/>, return its value. Otherwise, return the default value of <typeparamref name="T"/></returns>.
    public static T? GetItemOrDefault<T>(this ViewDataDictionary viewData, string key)
    {
        return viewData.TryGetValue(key, out var value) && value is T result
            ? result 
            : default;
    }

    /// <summary>
    /// Try getting the value of type <typeparamref name="T"/> stored in the <paramref name="viewData"/> with the specified <paramref name="key"/>. If Value is not found or cannot be converted to the target type, return the <paramref name="defaultValue"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="viewData">The view data dictionary.</param>
    /// <param name="key">The key of the value.</param>
    /// <param name="defaultValue">The default value to be returned if the value cannot be found.</param>
    /// <returns>If the item with the specified <paramref name="key"/> is found in the <paramref name="viewData"/> and its type is <typeparamref name="T"/>, return its value. Otherwise, return the <paramref name="defaultValue"/>.</returns>
    public static T GetItemOrDefault<T>(this ViewDataDictionary viewData, string key, T defaultValue)
    {
        return viewData.TryGetValue(key, out var value) && value is T result
            ? result
            : defaultValue;
    }
}