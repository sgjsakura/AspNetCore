# ASP.NET Core Data Paging and Pager Usage Guide

*Note: here is the guide of version 2. For the version 1 guide, please click [here](PagerDemov1.md).*

This page will give you a step-by-step guide to use ASP.NET Core MVC Data Paging and Pager features. This guide includes the following topics:
- How to paging your data source
- How to enumerate your data page and get paging information
- How to show a pager in your ASP.NET Core MVC Web Page

The following packages are required for this guide:
- `Sakura.AspNetCore.PagedList`: for data paging
- `Sakura.AspNetCore.PagedList.Async`: for async data paging
- `Sakura.AspNetCore.Mvc.PagedList`: for HTML pager generation

## Data Paging

Before using the ASP.NET Core MVC Pager, usually you may want to paging your data source (which may come from a EntityFramework or memory query) first. The `Sakura.AspNetCore.PagedList` package provides a interface named `IPagedList` to represent as a paged data source. The easiest way to create an instance of it is to use the extension methods defined in `Sakura.AspNetCore.PagedListCreationHelper` class. The following code shows the basic usage:

```C#
// Import extension methods in PagedListCreationHelper class.
using Sakura.AspNetCore;

var pageNumber = 1; // Note that page number starts from 1 (not zero!)
var pageSize = 10;

// data is assumed as coming from an EntityFramework DbSet here. Any object with type IEnumerable<T> or IQueryable<T> is supported with different implementations.
var data = from i in SportsManageModel.Players select i;

// The created IPagedList object, which contains a partial view for the current page, and the paging information.
var pagedData = data.ToPagedList(pageSize, pageNumber);
```

**Note:** actually, the content of `IEnumerable<T>` or `IQueryable<T>` may vary if they represent as a unstable source (e.g. the result a query from a database may change when another thread added new data). However, the the `ToPagedList` method always creates a static snapshot from the source at the creation time, which means the all the information of the `IPagedList` (including the total page count and the content of current page) never changes after you created it. If you want to capture a dynamic reference of the source, please see the next section.

### Dynamic Data Paging

For advanced scenes, you may try to track the change of the data source. Under such circumstance, you may use `IDynamicPagedList`. Another additional feature of this interface is you can change paging settings (page size and page index) even after you created it and receive data in new page. In order to use create a dynamic paged list, you may use `ToDynamicPagedList` extension method on either `IEnumerable<T>` or `IQueryable<T>` object.

### Async Data Paging

Entity Framework provides some extention method to quey data asynchronously (e.g. `ToArrayAsync`). If you would like to take advantage for these async operations, you may add the `Sakura.AspNetCore.PagedList.Async` package into your project, and use `ToPagedListAsync` method to generate `IPagedList` asynchronously.

**Note:** it is similar with the `ToPageList` method that the async method also creates snapshot for data source. Async operation on  dynamic paged lists is currently not supported, since there is no way to set a property value asynchronously.

## Paged Data Access

In MVC Projects, you may pass the paged data source from your controller into your view, and iterates the current pages easily. The following code shows the basic way:
```C#
// In MVC Controller
public IActionResult MyDataView()
{
  // Code for data source access is omitted here.
  var pagedData = data.ToPagedList(pageSize, pageNumber);
  return View(pagedData);
}
```

```C#
// In MVC View
@model Sakura.AspNetCore.IPagedList<Player>

@* The TotalCount property of a IPagedList is used to indicate the count of all (non-paged) items. *@
<span>Total Players: @Model.TotalCount </span>

<ul>
  @* Note: IPagedList implements the enumration for current page. (data not in current page will not be accessed.) *@
  @foreach (var i in Model)
  {
    <li>@i.Name</li>
  }
</ul>

```

*Note: Actually, the `IPagedList` is not related with the MVC platform, which means it can be used in any .NET Core Platform targeted project (Including ASP.NET Core Class Library and ASP.NET Core Web Application).*

## Pager

Pager feature is much more complex, since it is related with data handling, HTML generation, and MVC view code writing. The detailed step-by-step configuration is list as following.

### Simple Usage

First of all, you need to install the package for MVC pager, you should add the package `Sakura.AspNetCore.Mvc.PagedList` into your `project.json`.

And then, for the most simple usage, you may add the following code into your `Startup.cs` file:
```C#
// Add the following line at the top area of the file to import extension methods.
using Sakura.AspNetCore.Mvc;

public void ConfigureServices(IServiceCollection services)
{
  // .. Other configuration codes in you application
 
  // Add default bootstrap-styled pager implementation
  services.AddBootstrapPagerGenerator(options =>
  {
    // Use default pager options.
    options.ConfigureDefault();
   });
}
```
Next, you should add the `PagerTagHelper` into your view processing pipeline, you may add a new line in the `_ViewImports.cs` like:
```HTML
@addTagHelper *, Sakura.AspNetCore.Mvc.PagedList
```
Note the above line will enabled the `<pager>` tag for all view pages. If you want to enable it only in a few views, you may add this line to each view file invididually.

Finally, in your MVC View (.cshtml) file, use the following code to display a pager.
```HTML
<!-- The "source" attribute must be a C# expression with return type of `IPagedList` (no "@" perfix is needed) -->
<pager source="youDataModel" />
```
**Note: The `<pager>` tag must use the self-closing mode. Adding any content to this tag is not supported.**

If the model type of your view is just `IPagedList` or any inheritted type, you can even omit the "source" attribute and show a pager like this:
```HTML
<pager />
```

### Pager without Source

In most cases, your pager source is a value with type of `IPagedList`. The usage of the pager source is shown as above. However sometimes you may want to generate a pager with out a paged list source. In this case, you can use the `current-page` and `total-page` attribute on the tag to generate a static pager as following:
```HTML
<pager current-page="3" total-page="10" />
```
The above code will generate a pager with 10 pages, and the 3rd page will be current page and displays in an active style.

**Note: You must set both `current-page` and `total-page` attribute to generate a static pager. In addition, these 2 attribute cannot  be used together with the `source` attribute, otherwise you will receive an error message during the pager generating.**
###

### Generation Result and Partial Generation
The default bootstrap style pager generator will generate a `<ul class="pagination">` HTML element, and each pager item will be an `<li>` element inside it. According to the state of the page, there will be an `<a>` element (for most of pages) or an `<span>` element (for active page or any page without an effective link) as the content of the `<li>` element.

The original `<pager>` element will be removed from the final HTML page.

In some cases, you may want to use your own container for your pager, or add some custom items in the list. In this cases, You may change the generation mode of the pager to `PagerGenerationMode.ListOnly`. On this mode, only `<li>` elements for each pager items will be generated, a examples may be following:
```HTML
<!-- You must provide a container manually --> 
<ul class="pagination">
  <li>My Custom Item</li>
  <pager generation-mode="ListOnly" />
  <li>Another Custom Item</li>
</ul>
```
### PagerOptions Configuration
To customize the pager genreation, the most effective way is change the options for the pager. The options are designed as a type named `PagerOptions`, and you can set a custom options to your pager using the `options` attribute like below (the detailed description for options settings will be introduced later in this page):
```HTML
<!-- the `myOptions` must be a C# expression with return type of PagerOptions, no "@" perfix is needed. -->
<pager options="myOptions" />
```

#### `PagerOptions` Settings Description
There are many settings you can control in the `PagerOptions` class, a brief inroduction for these settings are listed below:

Setting|Description|Typical Usage
-------|-----------|-------------
`ExpandPageItemsForCurrentPage`|How many pages arround the current page (in both side) will be displayed. Set to 1 means one extra page for both left and right side will be generated. Set to 0 will display no extra pages (the current page is always displayed).|2
`PagerItemsForEndings`|How many pages at the ending will be displayed. Set to 1 means only the 1st first and last page will be displayed. Set to 0 will disable this feature.|3
`Layout`|The layout of the pager controls the element(s) will be displayed in the pager and their display order. For more information, please see the documentation of `PagerLayoutElement` class.|`PagerLayouts.Default`, `PargerLayouts.Custom()`
`IsReversed`|If true, all pager items (including number items and special buttons) will be reversed. |*N/A*
`HideOnSinglePage`|If true, the pager will supress all output when the pager source only has one page.|*N/A*
`ItemOptions`|Controls the content and link generation for different types of pager items.|*See below*
`AdditionalSettings`|Provide additional settings used for 3rd and expaned handlers.|An example can be found in `Enabling Ajax` section

In the `PagerOptions.ItemOptions` property, there are series of different individual properties to control the different pager element, each of them is an instance of `PagerItemOptions` class. The list of all items are as below:

Item|Usage|Base Item
----|-----|---------
`Default`|Shared settings for all pager items|*None*
`Normal`|Settings for all numbered pager links|`Default`
`Active`|Settings for current active (highlighted) page|`Normal`
`Omitted`|Settings for omitted page placehoders|`Default`
`FirstPageButton`|Settings for invidiual "Go to first page" button|`Default`
`LastPageButton`|Settings for invidiual "Go to last page" button|`Default`
`PreviousPageButton`|Settings for invidiual "Go to previous page" button|`Default`
`NextPageButton`|Settings for invidiual "Go to next page" button|`Default`

In the above table, `Base Item` means the setting will be generated will the specified base item. e.g. The lost settings in `Active` mode will be merged with values in `Normal` mode, and then merged again with `Default` mode. It means you do not need to rewrite the settings which are same as the base item.

The settings in the `PagerItemOptions` are described as below:

Setting|Description|Typical Usage
-------|-----------|-------------
`Content`|A content generator used to generate the content of each pager item|`PagerItemContentGenerators.XXX`
`Link`|A link generator used to generate the link of each pager item, if the generator returns a null string, the pager will has no link effect|`PagerItemLinkGenerators.XXX`
`InactiveBehavior`|How to handle the pager item if its current state is not meanful (e.g. the state of "go to next page" button when you are already in the last page), this setting only affects "go to first/last/next/previous page button"|*Please see documentation*
`ActiveMode`|How to determine if the current pager item is meanful (for further handling of `InactiveBehavior` setting), this setting only affect "go to first/last page button"|*Please see documentation*

#### Configure Default Options using Setup Actions

You do not need to set `options` attribute for each pager; You can set a default options at application startup time. The ASP.NET Core framework has provide the `Configure` extension method to save a application-level options value, you may use code like bellow to set the default `PagerOptions`:
```C#
public void ConfigureServices(IServiceCollection services)
{
  // .. Other configuration codes in you application

  services.Configure<PagerOptions>(options =>
  {
    // This following line is an example to set an option value
    options.PagerLinksForEnding = 2;
  });
}
```
You may also to to use the setup delegate in the `AddBootstrapPagerGenerator` method (see the example in the `Simple Usage` section), it is a shortcut manner with the same effect as `Configure` method.

#### Configure Options using Config File
ASP.NET Core application provide the ability to load configuration from setting files (usually named as `appsettings.json`). This pager framework also supports set most of the options in the settings file, a example is shown as below:

In `appsettings.json`:
```JSON
{
  "Pager": {
    "ExpandPageItemsForCurrentPage" : 2,
    "PageItemsForEnding": 3,
    "Layout": "Default",
    "AdditionalSettings": {
      "my-setting-one": "1"
    },
    
    "ItemOptions": {
      "Default": {
        "Content": "TextFormat:{0}",
        "Link": "QueryName:page",
        "InactiveBehavior": "Hide",
        "ActiveMode": "Always"
      },
      "GoToLastPage": {
        "Content": "Text:Go To Last Page"
      }
    }
  }
}
```
In `Startup.cs`:
```C#
// You need to the "Microsoft.Extensions.Options.ConfigurationExtensions" package, which is pre-loaded in ASP.NET Core RTM.

public void ConfigureServices(IServiceCollection services)
{
  // .. Other configuration codes in you application

  // Loading the "pager" section in the config file and set as the default pager options.
  services.Configure<PagerOptions>(Configuration.GetSection("Pager"));
}
```

The ASP.NET Core framework can mapp JSON key and values into strong typed propreteis of all simple types (int, string, etc.), enums, and `Dictionary<string,string>` automatically. However, the `PagerOptions.Layout`, `PagerItemOptions.Content` and `PagerItemOptions.Link` are complex types, thus the package provides different converters to convert string based values into actual implementations, the supported configuration string a listed below (allow constant string in `Format` column is case insensitive):

##### For `PagerOptions.Layout`

Format|Description|Example
------|-----------|-------
`Default`|Use default layout setting, please see `PagerLayouts.Default`|*N/A*
`Custom:{items}`|Use a custom layout, `{items}` is a comma `,` splitted string, in which each term is one enum item of `PagerLayoutElement`(case insensitive)|`Custom:GoToFirstPageButton,Items`: The pager will contains a standalone "go to first page" button and a list of pages, no other buttons will be displayed.

##### For `PagerItemOptions.Content`
Format|Description|Example
------|-----------|-------
`Text:{text}`|Use a fixed text as item content, string will be HTML-encoded before write to page.|`Text:Go to First` will display string "Go to First" in the button
`Html:{html}`|Use a fixed text as item content, string will **NOT** be HTML-encoded before write to page.|`Html:&lsaquo;` will display a single left quote mark ("<") in the button
`TextFormat:{format}`|Use a format string as item content, the placeholder `{0}` will be the page number.string will be HTML-encoded before write to page.|`TextFormat:Page {0:d}` will display `Page 3` for the 3rd page
`HtmlFormat:{format}`|Same as above, but the content will **NOT** be HTML-encoded.|*N/A*
`Default`|Use the default settings which display the page number only (equivelent to `Text:{0:d}`)|*N/A*

*Note: There are many other generators in `PagerItemContentGenerators` and `PagerItemLinkGenerators` class, however, many of them cannot be describe and created from a string configuration. In order to use them, you must craete them in your code.*

##### For `PagerItemOptions.Link`
Format|Description|Example
------|-----------|-------
`Query:{name}={format}`|Add a query parameter with a fixed name and a formatted query parameter value as link.|`Query:page={0:d}` will generate append(or replace) a query parameter `page=3` of the 3rd page on the current URL
`QueryName:{name}`|Add a query parameter with a fixed name, the value of the parameter will be the page number.|`QueryName:page`  will generate append(or replace) a query parameter `page=3` of the 3rd page on the current URL
`Default`|Use the default setting (equivelent to `QueryName:page`)|*N/A*
`Format`|Use a format string as item link, the placeholder `{0}` will be the page number.|`Format:/Index?page={0:d}` will generate a link `/Index?page=3` for the 3rd page
`Fixed`|Use a fixed string as item link.|`Fixed:/Index/Home` will generate a link `/Index/Home` for the pager item
`Disabled`|Generate a null string as link, which will cause the button non clickable|*N/A*

#### Additional Settings
Both `PagerOptions` and `PagerItemOptions` contains a property named `AdditioanlSettings`. This settings are automatically inheritted, which means the settings is the `PagerOptions` will be merged into each `PagerItemOptions` at runtime, and the different types of `PagerItemOptions` will also be merged as the same behavior for other properties described above.

This settings are not used in `PagerTagHelper` itself, however, the generators may use them for extra customization. In the next section you will see how the default `BootstrapPagerHtmlGenerator` use it for customize element generation.

#### Shortcut Settings
Creating a `PagerOptions` at each time is boring. In order to shorten the developement time, the `<pager>` tag provide several shortcut attribute in order to partial customize the options. Currently the available attributes are:

Name|Description
----|-----------
`item-default-content`|The default content generator. It refers `PagerOptions.ItemOptions.Default.Content`
`item-default-link`|The defualt link generator. It refers `PagerOptoins.ItemOptions.Default.Link`
`settings` and `setting-*`|Additional settings for `PagerOptions.` You may use `settings` to set the entire dictionary, or use `setting-*` to set one item. e.g. You can use `setting-mydata="1"` to set item "mydata" with value "1" (Just like the design of MVC `asp-route-*` attributes).

Here's a sample to use shortcut settings:
```HTML
<pager item-default-link='PagerItemLinkGenerators.QueryName("datapage")' />
```

In the above code, the pager will change use a custom link generator instead of the default generator configured globally.

### Generators and Customization

Pagers are complex objects. Its building process consists of many steps, including calculating page numbers, generating button links and contents, and building-up HTML strutures. All these work is done by a service named `IPagerGenrator`. In order to generate a pager, you must register one implementation of this service at application startup time, or specify one in the tag like:
```HTML
<pager generator="yourGenrator" />
```
The default generator is named `DefaultPagerGenrator`, which is included in this package. If you would like to use another generator to get a pager of new style, you may register a new one and replace it.

#### Default Generator and Partial Customization
Here, I would like to provide a brief introduction to the default generator. It uses 3 different sub-services to finally generate the complete pager HTML. These services are:

##### `IPagerListGenerator`
This service is used to generate a logical `PagerList` according to the pager information provided in the tag context. The logical `PagerList` describe the type and page number (if any) of each pager item, and their content and link generation manner. However, the visual information (e.g. the visual state and actual content) is not included. The `DefaultPagerListGenerator` class implements it by default.

##### `IPagerRenderingListGenerator`
This service is used to generate the visual `PagerRenderingList`, in which the visual information for the pager is generated, including its content HTML ,its link address, and visual state. The logical information like page number is no longer accessible. The `DefaultPagerRenderingListGenerator` class implements it by default.

##### `IPagerHtmlGenerator`
This service is used to finally build-up the entire HTML result. Since the `PagerRenderingList` only describe the information for the pager elements, it is still lack of the entire HTML structure, while this service take the responsibility for generating them. This service may take a lot of HTML operations, and the style of the final result is usually very limited by its implementation. If you just want to generate a pager with a different style, you may consider to find a new implementation for this service and register it before the default `BootstrapPagerHtmlGenerator` works.

#### Customization for Default Implementation

The default `BootstrapPagerHtmlGenerator` generate a bootstrap style pager, which includes a `<ul>` container, a series of `<li>` items, and each of them contains an `<a>` or `<span>` element accroding to the pager button's style.

In order to enhance the customization ability for this pager, the `BootstrapPagerHtmlGenerator` is designed to accept the following additional settings:
- Any setting with a name start with `list-attr-` will be append to the root `<ul>` element
- Any setting with a name start with `item-attr-` will be append to the `<li>` element
- Any setting with a name start with `link-attr-` will be append to the `<a>` and `<span>` element

For example, you may use the following code to set the `id` of the generated `<ul>` element as `myPager`:
```HTML
<pager setting-list-attr-id="myPager" />
```

#### Enabling AJAX
AJAX navigation is an common feature for a lots of mordern web applications. It should be pointed out that navigation is not the core business for a pager, but a feature on HTML interaction. Thus, this feature is not provided in the `PagerOptions` level, and you should ask the `IPagerHtmlGenerator` service to support AJAX navigation.

For the default `BootstrapPagerHtmlGenerator`, it is suggested that use the **Microsoft jQuery Unobtrusive Ajax** library as your AJAX support library. This library allows you to add static HTML attributes to control the AJAX behavior, which is highly compatible with this package. For example, a simple way to just enable AJAX nagivation is like:
```HTML
<pager setting-link-attr-data-ajax="true" />
```
The above code will generate all `<a>` element with a setting of `data-ajax="true"`, which can enable the AJAX navigation for links. For more detailed information about AJAX behaviour controlling, please read the offical documentation of **Microsoft jQuery Unobtrusive Ajax** library.

#### Bootstrap Version Compatibility Notes
Bootstrap V4 changed the HTML class requirement for pagination elements from V3, and requies `page-item` and `page-link` classes must be explicitly specified on the `<li>` and `<a>`(or `<span>`) element in the pager. Since the package version 2.0.10 of `Sakura.AspNetCore.Mvc.PagedList`, the default `BootstrapPagerHtmlGenerator` will automatically adds these classes on pager elements. This behaviour usually does not make downside effects on pages using Boostrap version 3 since these classes are not defined in the Version 3 CSS files, and thus no actual UI effect will raised. However, if your web site defines these classes and it DO affect the pager appearance, you may use the following settings to disable these additional classes:
```HTML
<pager setting-disable-bootstrap-v4-class="true" />
```

*** Feature Works and Contribution

The author is planned to add the following new features:
- [ ] Hash (Fragment) based URL generators
- [ ] `FormatProvider` controlling for generators
- [x] `IsReversed` property for `PagerOptions`
- [ ] Extenders for pager generators

Anyone who want to help improve the library is very welcome~
