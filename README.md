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

This project provides the `EnhancedSessionStateTempDataProvider` service provider, with can replace the original [`SessionStateTempDataProvider`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.viewfeatures.sessionstatetempdataprovider?view=aspnetcore-10.0), in order to enhance the type compatibility for session data. The original TempData provider can only work with primitive types, arrays, and one-level plain-objects, objects with complex properties are not supported. The `EnhancedSessionStateTempDataProvider` can successfully work with most data objects with arbitray level structures.

 Internally, it uses certain serializer techinque to convert complex data into string value, and store it together with its type full name. When application loading its content, it will deserialize the string to recover the object data structure. The default implmementation uses `JsonObjectSerializer`, and you may also replace it with your own implementation if necessary.


### ASP.NET Core MVC Messages Packages

#### NuGet Packages

* `Sakura.AspNetCore.Messages.Abstractions`
* `Sakura.AspNetCore.Messages`
* `Sakura.AspNetCore.Mvc.Messages`

This project enhances the common [Post/Redirect/Get (PRG)](https://en.wikipedia.org/wiki/Post/Redirect/Get) pattern by providing a streamlined way to display operation feedback (e.g., success or error messages) in web applications.

---

#### Component Breakdown

1. Sakura.AspNetCore.Messages.Abstractions

Defines the core messaging contracts:

* **`OperationMessage`**: The data structure for messages and the `OperationMessageLevel` enum members.
* **`IOperationMessageAccessor`**: The service contract for accessing operation messages within the current context.

2. Sakura.AspNetCore.Messages

Provides the core implementation:

* [`DefaultOperationMessageAccessor`](https://github.com/sgjsakura/AspNetCore/blob/master/Sakura.AspNetCore.Extensions/Sakura.AspNetCore.Messages/DefaultOperationMessageAccessor.cs): An implementation of `IOperationMessageAccessor` based on [`TempData`](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-10.0#tempdata), allowing data to persist across requests until it is read.
* [`OperationMessageOptions`](https://github.com/sgjsakura/AspNetCore/blob/master/Sakura.AspNetCore.Extensions/Sakura.AspNetCore.Messages/OperationMessageOptions.cs): Allows customization of the `DefaultOperationMessageAccessor`'s behavior.

> **Note**: The default [`SessionStateTempDataProvider`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.viewfeatures.sessionstatetempdataprovider?view=aspnetcore-10.0) in ASP.NET Core **does not** support storing complex types (like the `ICollection<OperationMessage>` used by this library). To resolve this, you can use `Sakura.AspNetCore.Mvc.TempDataExtensions` or implement a custom `ITempDataProvider`.

3. Sakura.AspNetCore.Mvc.Messages

Provides MVC-specific UI components using **Bootstrap** alerts or toasts:

* **`MessageTagHelper`**: A TagHelper that uses the `operation-message-list` attribute on `div` elements to generate the message UI. It supports various styles and layout options.
* **`IOperationMessageHtmlGenerator`**: An internal service for generating the HTML UI. It includes a built-in Bootstrap style generator.
* **`BootstrapIconMapper`**: Maps `OperationMessageLevel` to specific [Bootstrap Icons](https://icons.getbootstrap.com/).

---

#### Quick Start

1. Install Packages

Add the following packages to your project:

```powershell
dotnet add package Sakura.AspNetCore.Mvc.Messages
dotnet add package Sakura.AspNetCore.Mvc.TempDataExtensions
```

*(The Abstractions and Core packages will be included automatically as dependencies.)*

2. Install Bootstrap Icons

This library relies on Bootstrap Icons for visual indicators. Please refer to the [Official Documentation](https://icons.getbootstrap.com/#install) for installation instructions.

3. Configure Services

In your `Program.cs` or `Startup.cs`:

```csharp
using Sakura.AspNetCore.Mvc.Implementations;

// ...

public void ConfigureServices(IServiceCollection services)
{
    // Add operation message services with Bootstrap 5 styling
    services.AddOperationMessages()
        .UseBootstrap5Alerts(options =>
        {
            options.ContentLayout = Bootstrap5AlertMessageContentLayout.SingleRow;
            options.DismissAriaLabel = "Dismiss";
            options.Dismissible = true;
            options.ShowIcon = true;
        })
        .UseBootstrapIcons(options =>
        {
            options.IconStyle = BootstrapIconStyle.Normal;
            options.UseCircleForExclamation = false;
        });

    services.AddSession();
    
    // Add enhanced TempData support for complex object serialization
    services.AddEnhancedTempData(options => options.EnableHtmlContentSerialization());
}

```

#### 4. Register TagHelpers

Add the library's TagHelpers to your `_ViewImports.cshtml`:

```razor
@addTagHelper *, Sakura.AspNetCore.Mvc.Messages
```

#### 5. Add the Message Container

Place the container in your layout file (e.g., `_Layout.cshtml`) where you want messages to appear:

```html
<div class="container">
    <operation-message-list></operation-message-list>
    <main role="main" class="pb-3">
        @RenderBody()
    </main>
</div>

```

#### 6. Usage in Controllers

Inject `IOperationMessageAccessor` into your controller and add messages before redirecting:

```csharp
public class MyController(IOperationMessageAccessor messageAccessor) : Controller
{
    [HttpPost("Delete")]
    public async Task<IActionResult> DeleteItemAsync(int id)
    {
        // Your business logic...
        
        // Add a success message
        messageAccessor.Add(OperationMessageLevel.Success, "Success", "The item has been deleted successfully.");
        
        return RedirectToAction("Index");
    }
}

```

**Done!** Your application will now automatically display operation messages after a redirect.



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

### ASP.NET Entity Framework Core FromSql Extension Package

*Nuget Package Name: `Sakura.EntityFrameworkCore.FromSqlExtensions`*

In Entity Framework Core 2.1, the concept of the new `QueryType` is introduced, which allows developers to mapping non-table-based query results (e.g. data comes from a database view or a stored procedure) to CLR types. However, in order to use query types, you must first include it in your model. This restriction will cause you to take a lot of unnecessary work when you want to executing a raw SQL statement directly.

More specifically, let's see an example. You want to query some data using a stored procedure named "GetTopStudents" from your database, you know the procedure will produce the top 10 students with highest scores along with their name, and then you may define a query type as well as the following executing codes:

```C#
public class TopStudentInfo
{
  public string Name { get; set; }
  public int Score { get; set; }
}

// The following code is used to extract the top student infomation.
var result = MyDbContext.Query<TopStudentInfo>().FromSql("EXEC [GetTopStudents]");
```

However, when you try to execute the code above, Entity Framework Core will tell you that the `TopStudentInfo` is not included in the model. In order to fix this exception, you must first include it just like:

```C#
public class StudentDbContext : DbContext
{
  public virtual DbQuery<TopStudentInfo> TopStudents { get; set; }
}
```

And then you can get the result using either 
```C#
MyDbContext.Query<TopStudentInfo>().FromSql("EXEC [GetTopStudents]")
```
or
```C#
MyDbContext.TopStudents.FromSql("EXEC [GetTopStudents]")
```

The reason why EF requires you to include the query type is to support a chained-query just like `from i in MyDbContext.TopStudents select i`. Under such circumstance, the query type must be included and mapped with either `ToView` or `ToQuery` to declare its original data source. However, if you just want to take usage of the `FromSql` method, the mapping step will be reduntant, but you still need to define the query type in your `DbContext` class. 

Considering that you may executing serveral different raw SQL queries or stored procedures in your project. For each of them, you must include the query type into your model individually and this requirement will be a quite boring work. Now the `Sakura.EntityFrameworkCore.FromSqlExtensions` package provides extension methods for including query types and executing raw SQL statements directly, now you may using the following code:

```C#
var result = MyDbContext.FromSql<TopStudents>("EXEC [GetTopStudents]");
```

to get the result without explicitly defining a context-level `DbQuery` instance.

*Note: Internally, this extension method will register the query type into the model globally when you call it if it not exists before, and thus after you call this method sereval times with different types, there will be a huge number of query types added in your model. It may seems to be a side-effect (although adding a new query type actually does not generate performance burden for EF Core), while is problem is caused by the internal design of the EF Core and currently it is not possible to implement query-level type registrations.*

---

## Contribution and Discussion

You are welcome to add issues and advices, if you want to contribute to the project, please fell free to make pull requests.
