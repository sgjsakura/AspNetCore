using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Sakura.AspNetCore.Mvc
{
	/// <summary>
	/// Represent as a special exception used to return an <see cref="IActionResult"/> directly during the MVC executing pipeline.
	/// </summary>
	/// <remarks>
	/// To enable this special action, please mark <see cref="EnableActionResultExceptionAttribute"/> on a controller or action, or register it globally in your MVC configuration code. 
	/// </remarks>
	public class ActionResultException : Exception
	{
		/// <summary>
		/// Get or the result will be returned to the MVC pipeline when this exception is handled.
		/// </summary>
		public IActionResult Result { get; }

		/// <summary>
		/// Initialize a new <see cref="ActionResultException"/> with specified <see cref="IActionResult"/>.
		/// </summary>
		/// <param name="result">The <see cref="IActionResult"/> object which will be returned when this exception is handled.</param>
		public ActionResultException(IActionResult result)
		{
			if (result == null)
			{
				throw new ArgumentNullException(nameof(result));
			}

			Result = result;
		}

		/// <summary>
		/// Initialize a new <see cref="ActionResultException"/> with specified <see cref="IActionResult"/> and message.
		/// </summary>
		/// <param name="message">The message for this exception.</param>
		/// <param name="result">The <see cref="IActionResult"/> object which will be returned when this exception is handled.</param>
		public ActionResultException(string message, IActionResult result) : base(message)
		{
			if (result == null)
			{
				throw new ArgumentNullException(nameof(result));
			}

			Result = result;
		}

		/// <summary>
		/// Initialize a new <see cref="ActionResultException"/> with specified <see cref="IActionResult"/> and information.
		/// </summary>
		/// <param name="message">The message for this exception.</param>
		/// <param name="inner">The inner exception for this exception.</param>
		/// <param name="result">The <see cref="IActionResult"/> object which will be returned when this exception is handled.</param>
		public ActionResultException(string message, Exception inner, IActionResult result) : base(message, inner)
		{
			if (result == null)
			{
				throw new ArgumentNullException(nameof(result));
			}

			Result = result;
		}

		#region Shortcut Constructors

		/// <summary>
		/// Create a new <see cref="ActionResultException"/> with specified HTTP status code.
		/// </summary>
		/// <param name="statusCode">The value of the HTTP status code.</param>
		public ActionResultException(int statusCode)
		{
			Result = new StatusCodeResult(statusCode);
		}

		/// <summary>
		/// Create a new <see cref="ActionResultException"/> with specified HTTP status code.
		/// </summary>
		/// <param name="statusCode">The value of the HTTP status code.</param>
		public ActionResultException(HttpStatusCode statusCode)
		{
			Result = new StatusCodeResult((int)statusCode);
		}

		/// <summary>
		/// Create a new <see cref="ActionResultException"/> with specified HTTP status code and content.
		/// </summary>
		/// <param name="statusCode">The value of the HTTP status code.</param>
		/// <param name="value">The result content object.</param>
		public ActionResultException(int statusCode, object value)
		{
			Result = new ObjectResult(value) { StatusCode = statusCode };
		}

		/// <summary>
		/// Create a new <see cref="ActionResultException"/> with specified HTTP status code and content.
		/// </summary>
		/// <param name="statusCode">The value of the HTTP status code.</param>
		/// <param name="value">The result content object.</param>

		public ActionResultException(HttpStatusCode statusCode, object value)
		{
			Result = new ObjectResult(value) { StatusCode = (int)statusCode };
		}

		#endregion
	}
}
