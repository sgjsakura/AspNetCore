using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Sakura.AspNetCore.Mvc
{
	/// <summary>
	/// Enable special handling for <see cref="ActionResultException"/>.
	/// </summary>
	public class EnableActionResultExceptionAttribute : ExceptionFilterAttribute
	{
		/// <inheritdoc />
		public override void OnException(ExceptionContext context)
		{
			var actionResultException = context.Exception as ActionResultException;

			if (actionResultException != null)
			{
				context.ExceptionHandled = true;
				context.Result = actionResultException.Result;
			}
		}
	}
}
