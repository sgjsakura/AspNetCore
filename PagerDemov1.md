# ASP.NET 5 Data Paging and Pager Usage Guide

*NOTE: This is a documenation old version, we recommend you to use version newest of `Sakura.AspNetCore.Mvc.PagedList` package. The documentation for the newest package can be found [here](PagerDemo.md).*

This page will give you a step-by-step guide to use ASP.NET 5 MVC6 Data Paging and Pager features. This guide includes the following topics:
- How to paging your data source
- How to enumerate your data page and get paging information
- How to show a pager in your MVC6 Web Page

## Data Paging

To use MVC6 Pager, you must first paging your data source (which may come from a EntityFramework or memory query) into an `IPagedList` instance. The easiest way to create it is using the extension methods defined in `Sakura.AspNet.PagedListCreationHelper` class. The following code shows the basic way to use them:

```C#
// Import extension methods in PagedListCreationHelper class.
using Sakura.AspNet;

var pageNumber = 1; // Note that page number starts from 1 (not zero!)
var pageSize = 10;

// data is assumed as coming from an EntityFramework DbSet here. All data source with IEnumerable<T> and IQueryable<T> are both supported with different implementations.
var data = from i in SportsManageModel.Players select i;

// The IPagedList type, which contains a partial view for the current page, and the paging information.
var pagedData = data.ToPagedList(pageSize, pageNumber);
```
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
@model Sakura.AspNet.IPagedList<Player>

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

*Note: Actually, the `IPagedList` is not related with the MVC platform, which means it can be used in any .NET Core Platform targeted project (Including ASP.NET Class Library and ASP.NET Web Application).*

## Pager

Pager feature is much more complex, since it is related with data handling, HTML generation, and MVC view code writing. The detailed step-by-step configuration is list as following.

### Pager Configuration
Before you use pager feature, you must first configure pager options. Pager options is represented using `Sakura.AspNet.Mvc.PagerOptions` class, which controls the behavior for pager generation. A simple pager options defination may created as below:
```C#
using Sakura.AspNet.Mvc;

var pagerOptions = new PagerOptions
{
  ExpandPageLinksForCurrentPage = 2, // Will display more 2 pager buttons before and after current page.
  PageLinksForEndings = 2, // Will display 2 pager buttons for first and last pages.
  Layout = PagedListPagerLayouts.Default, // Layout controls which elements will be displayed in the pager. For more information, please read the documentation.

  // Options for all pager items.
  Items = new PagerItemOptions
  {
    TextFormat = "{0}", // The format for the pager button text, here means the content is just the actual page number. This property is used with string.Format method.
    LinkParameterName = "page", // This property measn the generated pager button url will append the "page={pageNumber}" to the current URL.
  }, 
  
  // Configure for "go to next" button
  NextButton = new SpecialPagerItemOptions 
  {
    Text = "Next",
    InactiveBehavior = SpecialPagerItemInactiveBehavior.Disable, // When there is no next page, disable this button
		LinkParameterName = "page"
  }, 
	
	// Configure for "go to previous" button
	PreviousButton = new SpecialPagerItemOptions
	{
    Text = "Previous",
    InactiveBehavior = SpecialPagerItemInactiveBehavior.Disable, // When there is no next page, disable this button
		LinkParameterName = "page"
	},
	
	// Configure for "go to first page" button
	FirstButton = new FirstAndLastPagerItemOptions
	{
		Text = "First",
		ActiveMode = FirstAndLastPagerItemActiveMode.Always,
		InactiveBehavior = SpecialPagerItemInactiveBehavior.Disable,
		LinkParameterName = "page",
	},
	
  // Configure for "go to last page" button
	LastButton = new FirstAndLastPagerItemOptions
	{
		Text = "Last",
		ActiveMode = FirstAndLastPagerItemActiveMode.Always,
		InactiveBehavior = SpecialPagerItemInactiveBehavior.Disable,
		LinkParameterName = "page",
	},
	
	// Configure for omitted buttons (placeholder button when there's too many pages)
	Omitted = new PagerItemOptions
	{
		Text = "...",
		Link = string.Empty // disable link
	},
}
```
### Pager Generation Service
Next, you also must have an `IPagerHtmlGenerator` service registered in your ASP.NET 5 application. The easiest way is using the default implementation using bootstrap theme. You can active this service in the `ConfigureServices` method in your ASP.NET 5 Startup class as below:

```C#
public class Startup
{
  public void ConfigureServices(IServiceCollection services)
  {
    // other service configuration of your application
    services.UseBootstrapPagerGenerator();
  }
}

```
If you uses different theme or frontend framework in your project, you may implement your own `IPagerHtmlGenerator` for custom UI.

### Pager Tag-Helper Directive
After all these above steps, now you are ready to show a pager in your rozar page. First, you must add tag helpers for pager directive, in `_ViewImports.cs`, add the folloing line:
```Rozar
@addTagHelper "*, Sakura.AspNet.Mvc.PagedList"
```
Then, you can enable pager UI for any <nav> element in your HTML page. The method is very simple as:
```HTML
<nav asp-pager-source="myPagedList" asp-pager-options="myPagerOptions" ></nav>
```
In the above example, `myPagedList` are an `IPagedList` object which represent the paged data, and `myPagerOptions` are your pager options (no @ perfix is needed because the value of both attribute are defined as C# object). You can define the pager options anywhere before this directive. In most cases, your paged data is just the model of your view, thus your can change the above code as:
```HTML
<nav asp-pager-source="Model" asp-pager-options="myPagerOptions" ></nav>
```

If you want to use an unified pager option for all of your pages, you can configure the default pager options in `UseBootstrapPagerGenerator` method like:
```C#
services.UseBootstrapPagerGenerator(pagerOptions => 
{
  pagerOptions.Items = new PagerItemOptions { /* ... */ };
  // ...
});
```
Then you can omit the `asp-pager-options` attribute, and your final code may be
```HTML
<nav asp-pager-source="Model"></nav>
```
-------------------
# FAQ

- *Is there any build-in AJAX support for this library?*
Unfortunately there is no build-in AJAX support in the current version. You may use JQuery scripts to enable AJAX nagivation for linkes, a sample may be:
```Javascript
$(document).ready(function(){
  // Add the 'data-ajax' attribute to all anchor in the pager 
  $('#your-pager-id').find('a').attr('data-ajax', 'true');
});
```
The author a designing a new version of this library, and in the new version AJAX support will be added as an build-in feature.
