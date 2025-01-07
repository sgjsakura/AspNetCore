namespace Sakura.AspNetCore.Mvc.TagHelpers;

/// <summary>
///     Define the update mode for AJAX operation results.
/// </summary>
public enum AjaxUpdateMode
{
    /// <summary>
    ///     The fetched content will be placed before the target element.
    /// </summary>
    Before,

    /// <summary>
    ///     The fetched content will be placed after the target element.
    /// </summary>
    After,

    /// <summary>
    ///     The fetched content with replace the target element.
    /// </summary>
    ReplaceWith
}