using Microsoft.AspNetCore.Mvc.Filters;

namespace Sakura.AspNetCore.Mvc;

/// <summary>
///     Enable special handling for <see cref="ActionResultException" />.
/// </summary>
public class EnableActionResultExceptionAttribute : ExceptionFilterAttribute
{
	/// <inheritdoc />
	public override void OnException(ExceptionContext context)
	{
		if (context.Exception is ActionResultException actionResultException)
		{
			context.ExceptionHandled = true;
			context.Result = actionResultException.Result;
		}
	}
}