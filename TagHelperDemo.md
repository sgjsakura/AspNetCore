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


### `EnumSelectForTagHelper`

Generation a list of all enum items and allow user to select one of them is a common task in MVC projects. Currently, you may use `HtmlHelper.GenerateEnumList` to generated a `SelectItemList` object and then set it to `asp-items` attributes on an `<select>` tags. These steps works, however it is a bit complex as well as you can not control the generated item's name and value, this will be UI unfriendly since the enum names are always short terms without spaces between words, and it also cannot be localizable.

Use the `EnumSelectForTagHelper` You can now use `asp-enum-for` attribute to generate a HTML select list with options for an enum type, the type is specified by the given model expression. What's more, You can apply `DisplayAttribute` on enum items and specify `asp-text-source` to support custom display texts. The `asp-value-source` can be used to specify the format of option values.

The following code shows the basic way for this tag helper:
```C#
// backend file 
public enum Gender
{
  Male,
  Female,
}

public class Person
{
  public Gender Gender { get; set; }
}

```

```HTML
<!-- In MVC view page -->
@model Person
<select asp-enum-for="Gender"></select>
```

The actual page will be generated as:
```HTML
<select name="Gender">
  <option value="Male">Male</option>
  <option value="Female">Female</option>
</select>
```

* `EnumSelectTypeTagHelper` You can use `asp-enum-type` to specify the enum type manually if you use an select without an model data. Adding the `asp-enum-value` attribute to specify the selected item; otherwise, no item is selected by default.  `asp-text-source` and `asp-value-source` can also be used as the same as `EnumSelectForTagHelper`.

* `SelectValueTagHelper`: You can now use `asp-value` attribute on `select` element to automatically make the matched option selected when rendering its content.

* `SelectOptionLabelTagHelper`: You can now use `asp-option-label` attribute on `select` element to generate an option label with custom text (the value of the option label is set to empty). The `asp-option-label-position` attribute can be used to controller the position of the option label.

* `ConditionalClassTagHelper`: You can now use `asp-conditional-class-<className>="<ConditionExpression>"` to add conditional actived class name to any HTML elements. When the condition expression is evaluated with value `true`, the specified class will be added to this element; otherwise, this class will be ignored. You can apply multiple conditional classes on one element.

* `FlagsEnumInputTagHelper`: You can now use `asp-flag-enum-for` and `asp-flag-enum-value` attributes on checkbox (`<input type="checkbox" />`) element. The `asp-flag-enum-for` attribute controls the model expression of enum flag value (the enum type must be marked with `FlagsAtttibute`), and the `asp-flag-enum-value` indicates the enum flag item for this checkbox. If the actual model expression value contains the flag value (mainly equivalent as `Enum.HasFlag` method), the checkbox will be checked, otherwise, it will be unchecked.

*NOTE: To correctly analysis the generated model data when page posts them to server, you may also need `FlagsEnumModelBinder` class.*

* `FlagsEnumModelBinder`: A helper model binder class often used together with `FlagsEnumInputTagHelper` tag helper, but also can be used indivindually. This model binder can collect all model values associated with one model (or its property) of enum types with `FlagsAttribute` applid, convert each value seperately, and used bitwise or to generate the merged final value.
