using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Rendering;

using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sakura.AspNetCore.Mvc.TagHelpers;

[HtmlTargetElement("partial", Attributes = $"{ViewDataItemAttributePrefix}*")]
[HtmlTargetElement("partial", Attributes = ViewDataAttributeName)]
[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
public class PartialViewItemTagHelper : TagHelper
{
    /// <summary>
    ///  The HTML attribute prefix for single view data entry. This field is constant.
    /// </summary>
    [PublicAPI]
    public const string ViewDataItemAttributePrefix = "view-item-";


    /// <summary>
    /// The HTML attribute name for the entire view data dictionary. This field is constant.
    /// </summary>
    [PublicAPI]
    public const string ViewDataAttributeName = "all-view-items";

    /// <inheritdoc />
    public override int Order => base.Order - 1;

    /// <summary>
    ///  The <see cref="ViewContext"/> instance.
    /// </summary>
    [ViewContext]
    [HtmlAttributeNotBound]
    [UsedImplicitly(ImplicitUseKindFlags.Assign)]
    public ViewContext ViewContext { get; set; } = null!;

    /// <summary>
    /// The dictionary containing all the view data items.
    /// </summary>
    [HtmlAttributeName(ViewDataAttributeName, DictionaryAttributePrefix = ViewDataItemAttributePrefix)]
    [UsedImplicitly(ImplicitUseKindFlags.Assign)]
    public Dictionary<string, object?> ViewDataItems { get; set; } = [];


    /// <inheritdoc />
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        foreach (var item in ViewDataItems)
        {
            ViewContext.ViewData[item.Key] = item.Value;
        }
    }
}