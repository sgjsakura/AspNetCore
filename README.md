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

---

## Contribution and Discussion

You are welcome to add issues and advices, if you want to contribute to the project, please fell free to make pull requests.
