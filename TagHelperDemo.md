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

Generation a list of all enum items and allow user to select one of them is a common task in MVC projects. Currently, you may use `HtmlHelper.GenerateEnumList` to generated a `SelectItemList` object and then set it to `asp-items` attributes on an `<select>` tags. These steps can work, however it is a bit complex as well as you can not control the generated item's name and value, thus it will be UI unfriendly since the enum names are always short terms without spaces between words, and it also cannot be localizable.

Use the `EnumSelectForTagHelper` You can now use `asp-enum-for` attribute to generate a HTML select list with options for an enum type, the type is specified by the given model expression. The following code shows the basic way for this tag helper:
```C#
// backend file 
public enum ProjectAccessType
{
  [Display(Description = "Everyone can access this project")]
  Public,
  [Display(Description = "Only specified users can access this project")]
  Private,
  [Display(Description = "Everyone can access and update this project")]
  Community
}

public class Project
{
  public ProjectAccessType AccessType { get; set; }
}

```

```HTML
<!-- In MVC view page -->
@model Project
<select asp-enum-for="AccessType"></select>
```

The actual page will be generated as:
```HTML
<select name="Gender">
  <option value="Public">Public</option>
  <option value="Private">Private</option>
  <option value="Community">Community</option>
</select>
```

An important enhancement provied by this tag helper is that you can use `asp-text-source` and `asp-value-source` to control the generated text and value of options. The tag helper will detect any `DisplayAttribute` applied on each enum item, and get the property value specified by `asp-text-source` attribute as the display text of options. e.g. The following code:
```HTML
<!-- In MVC view page -->
@model Project
<select asp-enum-for="AccessType" asp-text-source="Description"></select>
```
will generate the following HTML (using the same backend type definition as first sample):
```HTML
<select name="Gender">
  <option value="Public">Everyone can access this project</option>
  <option value="Private">Only specified users can access this project</option>
  <option value="Community">Everyone can access and update this project</option>
</select>
```
If none of `asp-text-source` is specified, the default value is set to `EnumNameOnly`, which will generate the result as the same as the first sample. If some enum items is lack of specified inforation (e.g. `Description` is null), the tag helper will also fallback to using the enum name as the text.
The `asp-value-source` is used to control the value of options. The default value of this attribute is set to `Name`, which means the name of the enum item is used as the option value. If needed, you can change this attribute to `Value`, in such case, the following code:
```HTML
<!-- In MVC view page -->
@model Project
<select asp-enum-for="AccessType" asp-value-source="Value"></select>
```
Will be generated as:
```HTML
<select name="Gender">
  <option value="0">Public</option>
  <option value="1">Private</option>
  <option value="2">Community</option>
</select>
```
*Note: The default MVC model binders can handle enum names correctly, thus you usually do not need to set this attribute.*

### `EnumSelectTypeTagHelper`

This tag helper is similar ot `EnumSelectForTagHelper`, however you use `asp-enum-type` to specify the enum type explicitly instead of inferring it from a model expression. In such case you may also need to add the `name` attribute for the `<select>` tag manually in order to send its data to server correctly.

### `SelectValueTagHelper`

Another common task related to `<select>` tag is to initially set the default selected value according to the current state of data when a user tries to edit a existing data item. In ASP.NET Core MVC projects, you can use `asp-for` for a `<select>` element to automatically set the initial state according to the model value. However, if you are trying to display a select list without directly model binding, you must using one of the following manner:
1. build-up a `SelectList` instance and provide `selectedValue` argument during construction, and then apply `asp-items` attribute on the `<select>` element.
2. Define each `<option>` element in HTML and using a Rozar condition expression to the `selected` attribute.

This first manner is more simple for value calculation, however you will write a lot of C# code for generating a static list. The second manner is gracer in HTML generation, however you must repeat the `selected` condition on each option.

Now with the new tag helper, you can simplely apply an `asp-value` attribute on any `<select>` tag, and tag helper will help you to automatically make the correct option selected during the page generation. e.g. The following code:
```HTML
<select asp-value="@myValue">
  <option value="1">1</option>
  <option value="2">2</option>
</select>
```
will generate the following HTML is the value of `myValue` is equal to `1`:
```HTML
<select>
  <option value="1" selected="selected">1</option>
  <option value="2">2</option>
</select>
```
*Note: The value of `asp-value` attribute is considered as a string since HTML only accept string as element content. You may need to convert value of other types into string manually.*

Additionally, you can specify the `asp-value-compare-mode` on the `<select>` tag to control how to determine the specified value is equal to the option value. The default setting is `OrdinalIgnoreCase`, which can be used in mose cases, but you can change it to any enum item in `System.StringComparison` type to change this behavior.

* `SelectValueTagHelper`: You can now use `asp-value` attribute on `select` element to automatically make the matched option selected when rendering its content.

* `SelectOptionLabelTagHelper`: You can now use `asp-option-label` attribute on `select` element to generate an option label with custom text (the value of the option label is set to empty). The `asp-option-label-position` attribute can be used to controller the position of the option label.

* `ConditionalClassTagHelper`: You can now use `asp-conditional-class-<className>="<ConditionExpression>"` to add conditional actived class name to any HTML elements. When the condition expression is evaluated with value `true`, the specified class will be added to this element; otherwise, this class will be ignored. You can apply multiple conditional classes on one element.

* `FlagsEnumInputTagHelper`: You can now use `asp-flag-enum-for` and `asp-flag-enum-value` attributes on checkbox (`<input type="checkbox" />`) element. The `asp-flag-enum-for` attribute controls the model expression of enum flag value (the enum type must be marked with `FlagsAtttibute`), and the `asp-flag-enum-value` indicates the enum flag item for this checkbox. If the actual model expression value contains the flag value (mainly equivalent as `Enum.HasFlag` method), the checkbox will be checked, otherwise, it will be unchecked.

*NOTE: To correctly analysis the generated model data when page posts them to server, you may also need `FlagsEnumModelBinder` class.*

* `FlagsEnumModelBinder`: A helper model binder class often used together with `FlagsEnumInputTagHelper` tag helper, but also can be used indivindually. This model binder can collect all model values associated with one model (or its property) of enum types with `FlagsAttribute` applid, convert each value seperately, and used bitwise or to generate the merged final value.
