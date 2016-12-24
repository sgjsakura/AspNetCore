# ASP.NET Core Tag Helper Extension Library Usage Guideline

ASP.NET Core uses a new coding writing design mode named `Tag Helpers` replacing the original `Html Helpers` utlity in order to simplify server-side HTML generation. This library is an supplement for the official `Microsoft.AspNetCore.Mvc.TagHelpers` package and added a series of new tag helpers. This page will demostrate the usages all tag helpers provided in the `Sakura.AspNetCore.Mvc.TagHelpers` library. 

## Installation

To use new tag helpers provided in this library, you must first install and configure the package. In order to install it to your project, please open `project.json` file and add a new depenedency named `Sakura.AspNetCore.Mvc.TagHelpers` in the `dependencies` section.

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

This first manner is more simple for value calculation, however you will write a lot of C# code for generating a static list. The second manner is more graceful in HTML generation, however you must repeat the `selected` condition on each option.

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

### `SelectOptionLabelTagHelper`

This is a shortcut tag helper used to add a option label (option with empty value) into a `<select>` tag. You may use `asp-option-label` to set the label text and `asp-option-label-position` to set the location of the label. A sample can be:
```HTML
<select asp-option-label="Select a option" asp-option-label-position="First">
  <option value="1">1</option>
  <option value="1">1</option>
</select>
```
The above code will generate the folloing HTML:
```HTML
<select>
  <option value="">Select a option</option>
  <option value="1">1</option>
  <option value="1">1</option>
</select>
```

### `FlagsEnumInputTagHelper`

Flags enum is a common design pattern used to store multiple flags into a composed value, and filter values using arbitray flag combination. For end users, each flag can be considered as a individual option, and is usually represented as a checkbox. However, the default MVC model binding system does not allow to bind a single flag with a checkbox's check state. 

With the new tag helper, you can now use `asp-flag-enum-for` and `asp-flag-enum-value` attributes on checkbox to correctly bind a certern flag of a model value with the input element. e.g. the following code:
```C#
// backend code
[Flags]
public enum Features
{
  None = 0x0,
  CanRead = 0x1,
  CanWrite = 0x2,
  CanSeek = 0x4
}

public class Stream
{
  public Features Features { get; set; }
}

```
```HTML
<!-- In MVC view page -->
@model Stream
<label>
  <input type="checkbox" asp-enum-flag-for="Features" asp-enum-flag-value="Features.CanSeek"> CanSeek
</label>
```
will generate the following code if the model's `Features` property contains the flag `CanSeek`:
```HTML
<label>
  <input type="checkbox" name="Features" value="CanSeek" checked="checked"> CanSeek
</label>
```
*NOTE: This tag helper only helps you to generate an input element. Unforetunately, the default MVC model binding system cannot handle flags enum items and merge them correctly. In order to receive the final data in you back end callback action, you may also need the `FlagsEnumModelBinder` feature, this binder can be found in the next section.*

###  `ConditionalClassTagHelper`

CSS class is an important part for HTML element. It can be used to both styling and item filtering. An element can belong to one or more classes sepecifed by `class` attribute. The `class` attribute is a composed value contains a list of class names, each of them a sepereated with spaces. You cannot specify multiple `class` attributes on a single element, and if you wish to apply some class conditionally, you may:

- Generate the entire class list using C# and set the value to `class` attribute, or
- Generate a string of class name or empty string according to the condition, and insert it as a part or class string

Neither of those methods are friendly to code reading. Now using the new tag helper, you can simplely append any class on a single element with different conditions using `asp-conditional-class-<className>` attribute. Here is a simple usage demo:
```HTML
<a class="page" asp-conditional-class-active="currentPage == 1" href="?page=1">Page 1<a>
```
If the C# expresion condition `currentPage == 1` is satisfied, the above code will generate:
```HTML
<a class="page active" href="?page=1">Page 1<a>
```
While if the condition is not satisfied, this attribute will be simplely ignored.

### `IdFormatTagHelper`

In a complex page, parts of layout may be repeated, e.g. A page of product introduction may has several user comments. Usually you control all of them using class filters or DOM tree traveling, however, sometimes you may need to control each of them invidually. Although you can capture them by location, using ID to pick-up one element is usually more efficient and straightforward. The HTML standard requires every ID must be unique in each page, thus for a server-side repeated range (usually generated by `for` or `foreach`), id must be dynamically generated inside the loop range.

The `IdFormatTagHelper` provide `asp-id-format` attribute on element in order to generate a id according to a specified format. The final id will be generated using `string.Format` method, will the placeholder `{0}` will a unique number start from one. Here is a example:
```HTML
@foreach (var i in Items)
{
  <div asp-id-format="region-{0}"></div>
}
```
The above code will generated the following HTML if the `Items` has 3 elements:
```HTML
<div asp-id-format="region-1"></div>
<div asp-id-format="region-2"></div>
<div asp-id-format="region-3"></div>
```
*Note: Currently the tag helper use the format string as the counting key, that means all element uses a same value of `asp-id-format` will share the counting number. The count is registered and saved in the view data dictionary, which means the entire page processing pipeline (including the layout page, main page, any partial page and view components) will be considered as a single document and the counting will be continous in the final page.*

## Binders

### `FlagsEnumModelBinder`

The `FlagsEnumInputTagHelper` can help you to generate flag checkboxs, however, default MVC model binder cannot automatically merge all the values. To solve this problem, you may add the `FlagsEnumModelBinder` into you MVC middleware, you may add it in the service configurating phase using the following code:
```C#
// In startup.cs

public void ConfigurationServices(IServiceCollection services)
{
  // Other configuration code
  services.AddMvc(options => 
  {
    // Using the followint code to add the new binder
    options.AddFlagsEnumModelBinderProvider();
  });
}
```
