# ASP.NET Core Utility

This solution add some useful features for ASP.NET Core projects. All projects in this solution are designed for cross-platform  "netstandard" and triditional .NET Full "net" frameworks.

This solution contains the following aspects:

* Tag Helpers
* Middlewares
* Utilities

---

## Current Project List

This section lists all projects in the repo. The list will always update in time.

### External Cookie Services

*Nuget Package Name: `Sakura.AspNetCore.Authentication.ExternalCookie`*

ASP.NET Core Identity Service (`Microsoft.AspNet.Identity` package) already added support for external cookie services, which is required for 3rd application authentication scheme (e.g. Microsoft of Fackbook account). You may use `AddIdentity` and `UseIdentity` method to enable external cookie services. However, in sometimes you may wish to enable external services indivindually without full ASP.NET Identity Service enabled. This project seperates the external cookie services from ASP.NET Identity Services, and may be used in more complex authentication scenes.

The major feature of this project contains:

* Configure ASP.NET Core Application to support ApplicationCookie and ExternalCookie services (provides extension methods can be used in `ConfigureServices` method of ASP.NET Startup class)
* Provide ExternalSignInManager service to simpify external cookie ticket management.


### ASP.NET Core MVC TagHelper Extension Library

*Nuget Package Name: `Sakura.AspNetCore.Mvc.TagHelpers`*

Provide new tag helpers in order to simplify MVC code writing. For more details, please visit the [Tag Helper Demo](TagHelperDemo.md) page.

### ASP.NET TempData Extension Package

*Nuget Package Name: `Sakura.AspNetCore.Mvc.TempDataExtensions`*

This project provides the `EnhancedSessionStateTempDataProvider` service provider, with can replace the original `SessionBasedTempDataProvider`, in order to enhance the type compatibility for session data. The original TempData provider can only work with primitive types, arrays, and one-level plain-objects, objects with complex properties are not supported. The `EnhancedSessionStateTempDataProvider` can successfully work with most data objects with arbitray level structures.

 Internally, it uses certain serializer techinque to convert complex data into string value, and store it together with its type full name. When application loading its content, it will deserialize the string to recover the object data structure. The default implmementation uses `JsonObjectSerializer`, and you may also replace it with your own implementation if necessary.


### ASP.NET Core MVC Messages Package

*Nuget Package Name: `Sakura.AspNetCore.Mvc.Messages`*

This project add the feature of common operation message response in web applications, as well as tag helpers to simplify message presentations. The detailed features includes:

* `OperationMessage` definitions and different `OperationMessageLevel` enum items.
* `IOperationMessageAccessor` service, which can be inject in both views and controllers to access operation message data.
* `MessageTagHelper` helper class, and you may use `asp-message-list` attribute on `div` element to generate message list UI, with various styles and additional layout options can be specified.
* `IOperationMessageHtmlGenerator` service, wich is used internally for generating message list UI, and default bootstrap style generator are built-in implemented.

*NOTE: To use this package, your `ITempDataProvider` implementation must have the ability to store and load `ICollection<OperationMessage>` instance. Defaultly, the ASP.NET5 `SessionStateTempDataProvider` cannot support data operation of complex objects. You may change into another `ITempDataProvider` implementation, or just use `EnhancedSessionStateTempDataProvider` in the `ASP.NET TempData Extension Package` project.

### ASP.NET Core PagedList Packages

*Nuget Package Name:*
- *`Sakura.AspNetCore.PagedList`*
- *`Sakura.AspNetCore.PagedList.Async`*
- *`Sakura.AspNetCore.Mvc.PagedList`*

The `Sakura.AspNetCore.PagedList` package provides the `IPagedList` core interface to represent as a data page for a large data source. Some extension methods are also provided to generate instance from any `IEnumerable<T>` or `IQueryable<T>` data sources.

The `Sakura.AspNetCore.PagedList.Async` package helpes you to generate `IPagedList` using async extension method (`ToArrayAsync`, etc.) defined in `Microsoft.EntityFrameworkCore` package.

The `Sakura.AspNetCore.Mvc.PagedList` allows you to use `<pager>` tag in your MVC view page to generate a full featured pager structure.

*For detailed usage, please visit the [Demo](PagerDemo.md) page. Notice: this package has been updated to version 2 (the recommended version)． For usage of version 1, please visit the [Version 1 Demo](PagerDemov1.md) page.*

### ASP.NET ActionResult Extensions Package

*Nuget Package Name: `Sakura.AspNetCore.Mvc.ActionResultExceptionExtensions`*

In MVC Projects, you need to return a instance of `IActionResult` to finish the action pipeline, this design made it difficult to add common helper functions to make argument or permission checking and then report a specified status code directly to end user. This library allows you to terminate action executing pipeline directly with a specified result through the standard exception handling system. 

In order to enable this feature, all you need is adding an `EnableActionResultException` attribute on a controller or action, and then you can throw an `ActionResultException`instance to terminate a action executing pipeline directly and provide the final action result. If you need to enable this feature globally, you can use `EnableActionResultExceptionFilter` extension method on `MvcOptions` parameter when you add the MVC middleware.

### ASP.NET Core MVC Dyanmic Localizer Package

*Nuget Package Name: `Sakura.AspNetCore.DynamicLocalizer`*

ASP.NET Core 1.0 introduced a new localization design model, which allow developers to access localized resources using `IStringLocalizer`, `IHtmlLocalizer` or `IViewLocalizer` service instances. In order to keep compatibilty and reduce the time cost for switching the developing time single language website into production multiple language implementation, ASP.NET Core team suggests developers to use string context itself as the language resource keys. e.g. the following code
```HTML
@ViewLocalizer["Hello World"]
```
while output the key string "hello world" when there's no resource files defined or the resource for current culture is unavialable.

Although this design reduces the time cost of enabling multiple language support, lots of developers may still build the website with multiple language support from the beginning. The above manner may not be optimized for these scenes, since under such circumstance, developers may choose to use identifier-like word as keys for long messages, e.g. the final code in the CSHTML file maybe: 
```HTML
@ViewLocalizer["HelloWorldMessage"]
```
However, actually developers may love the following code style much more:
```HTML
@ViewLocalizer.HelloWorldMessage
```
The above code style looks with much more object-oriented code style, and may also take a lot of editing and compiling time benefits such as identifier checking, searching and intellisence. This behaviour is also favorite for old-time .NET Resource Manager based localizaable applications, in which each resources is named with an identifier and the resource file generators will help you to generate classes for resources, and developers may use strong named propreties to access resources. 

The `Sakura.AspNetCore.DynamicLocalizer` package now provides a simplified simulation for object-oreinted resource accessing style using .NET dynamic objects. For a simple usage, you may install this package and add the necessary services on startup using the following code:

```C#
public void ConfigureServices(IServiceCollection services)
{
  // other service configurations here
  
  // Note: the following base services are necessary and must be also added manually in your startup code
  services.AddLocalization();
  services.AddMvc().AddViewLocalization();
  
  // add core services used for dynamic localizer
  services.AddDynamicLocalizer();
}
```

And now in MVC views, you may using dyanmic object as localization services, the basic usage is shown in the following code:

```HTML
@inject Sakura.AspNetCore.IDynamicViewLocalizer ViewLocalizer
@inject Sakura.AspNetCore.IDynamicHtmlLocalizer<MyResource> MyResourceLocalizer

<p>@ViewLocalizer.Html.WelcomeMessage</p>
<p>@ViewLocalizer.Html.UserNameFormat(ViewBag.UserName)</p>

<p>@MyResourceLocalizer.Html.ImportantTip<Hello>
```
More specifically, the relationship between original localizers and dynamic localizers are shown as bellow:

|Original|Dynamic|
|--------|-------|
|`IViewLocalizer`|`IDyanmicViewLocalizer`|
|`IHtmlLocalizer<T>`|`IDynamicHtmlLocalizer<T>`|
|`IStringLocalizer<T>`|`IDynamicStringLocalizer<T>`|

The following tables showes the supported invocation syntax for dynamic localizers (words in braces are identfier placeholders):

|Syntax|Equivelant Orignal Syntax|Notes|
|------|-------------------------|-----|
|`localizer.Html.{Key}`|`localizer.GetHtml("{Key}")]`|This method is not available in `IStringLocalizer`|
|`localizer.Html.{Key}({Value1}, {Value2})`|`localizer.GetHtml("{Key}", Value1, Value2)]`|This method is not available in `IStringLocalizer`|
|`localizer.Text.{Key}`|`localizer.GetString("{Key}")`|Allowed in all localizers|
|`localizer.Text.{Key}({Value1}, {Value2})`|`localizer.GetString("{Key}", Value1, Value2)]`|Allowed in all localizers|

Note: The behaviour default index-style syntax (e.g. `localizer["Key", Value1, Value2]`) depends on the type of the localizer. For `IViewLocalizer` and `IHtmlLocalizer`, it is equivelant to `GetHtml`,  while for `IStringLocalizer`, it's equivelant to `GetString`.

For compatbility reason, the dynamic localizer also support the index style syntax, which will generate the same effect for the new syntax, e.g. `localizer.Html["{Key}", Value1, Value2]` are equivelant to `localizer.Html.Key(Value1, Value2)`.

---

## Contribution and Discussion

You are welcome to add issues and advices, if you want to contribute to the project, please fell free to make pull requests.
