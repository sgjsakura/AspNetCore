# ASP.NET Core Tag Helper Extension Library Usage Guideline

ASP.NET Core uses a new coding writing design mode named `Tag Helpers` replacing the original `Html Helpers` utlity in order to simplify server-side HTML generation. This library is an suppliment for the official `Microsoft.AspNetCore.Mvc.TagHelpers` package and added a series of new tag helpers. This page will demostrate the usages all tag helpers provided in the `Sakura.AspNetCore.Mvc.TagHelpers` library. 

## Installation

To usage new tag helpers in this library, you must first install and configure it. To install this package to your project, please open `project.json` file and add a new depenedency named `Sakura.AspNetCore.Mvc.TagHelpers` in the `dependencies` section.

And then, you should enable new tag helpers in your MVC view. The most simple way to enable all tag helpers for all pages is add a new line in `_ViewImports.cs` files as follow:
```HTML
@addTagHelper *, Sakura.AspNetCore.Mvc.TagHelpers
```

If you need to enable/disable a custom tag helper in a specified page, please see the documentation for `@addTagHelper` directive.

## TagHelper List

This section will describle the usage of all tag helpers.

Add various addtional TagHelper classes to simplify strong type model based ASP.NET Core MVC web application development, including:

* `EnumSelectForTagHelper`: You can now use `asp-enum-for` attribute to generate a HTML select list with options for an enum  type, the type is specified by the given model expression. You can apply `DisplayAttribute` on enum items and specify `asp-text-source` to support custom display texts. The `asp-value-source` can be used to specify the format of option values.

* `EnumSelectTypeTagHelper` You can use `asp-enum-type` to specify the enum type manually if you use an select without an model data. Adding the `asp-enum-value` attribute to specify the selected item; otherwise, no item is selected by default.  `asp-text-source` and `asp-value-source` can also be used as the same as `EnumSelectForTagHelper`.

* `SelectValueTagHelper`: You can now use `asp-value` attribute on `select` element to automatically make the matched option selected when rendering its content.

* `SelectOptionLabelTagHelper`: You can now use `asp-option-label` attribute on `select` element to generate an option label with custom text (the value of the option label is set to empty). The `asp-option-label-position` attribute can be used to controller the position of the option label.

* `ConditionalClassTagHelper`: You can now use `asp-conditional-class-<className>="<ConditionExpression>"` to add conditional actived class name to any HTML elements. When the condition expression is evaluated with value `true`, the specified class will be added to this element; otherwise, this class will be ignored. You can apply multiple conditional classes on one element.

* `FlagsEnumInputTagHelper`: You can now use `asp-flag-enum-for` and `asp-flag-enum-value` attributes on checkbox (`<input type="checkbox" />`) element. The `asp-flag-enum-for` attribute controls the model expression of enum flag value (the enum type must be marked with `FlagsAtttibute`), and the `asp-flag-enum-value` indicates the enum flag item for this checkbox. If the actual model expression value contains the flag value (mainly equivalent as `Enum.HasFlag` method), the checkbox will be checked, otherwise, it will be unchecked.

*NOTE: To correctly analysis the generated model data when page posts them to server, you may also need `FlagsEnumModelBinder` class.*

* `FlagsEnumModelBinder`: A helper model binder class often used together with `FlagsEnumInputTagHelper` tag helper, but also can be used indivindually. This model binder can collect all model values associated with one model (or its property) of enum types with `FlagsAttribute` applid, convert each value seperately, and used bitwise or to generate the merged final value.
