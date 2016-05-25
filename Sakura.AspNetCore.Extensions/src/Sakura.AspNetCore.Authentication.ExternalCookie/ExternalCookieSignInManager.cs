using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.Extensions.Options;

namespace Sakura.AspNetCore.Authentication
{
	/// <summary>
	///     提供对于外部 Cookie 验证的相关服务。
	/// </summary>
	public class ExternalCookieSignInManager
	{
		/// <summary>
		///     用指定的依赖服务初始化一个对象的新实例。
		/// </summary>
		/// <param name="httpContextAccessor">HTTP 上下文访问接口。</param>
		/// <param name="identityOptions"><see cref="Microsoft.AspNetCore.Builder.IdentityOptions" /> 选项访问接口。</param>
		public ExternalCookieSignInManager(IHttpContextAccessor httpContextAccessor, IOptions<IdentityOptions> identityOptions)
		{
			HttpContext = httpContextAccessor.HttpContext;
			IdentityOptions = identityOptions.Value;
		}

		/// <summary>
		///     服务使用的 HTTP 上下文对象。
		/// </summary>
		protected HttpContext HttpContext { get; }

		/// <summary>
		///     身份验证管理器。
		/// </summary>
		protected AuthenticationManager AuthenticationManager => HttpContext.Authentication;

		/// <summary>
		///     服务使用的标识功能配置选项。
		/// </summary>
		protected IdentityOptions IdentityOptions { get; }

		/// <summary>
		///     异步获取当前 HTTP 上下文中包含的外部 Cookie 用户身份。
		/// </summary>
		/// <returns>一个表示异步操作的任务，其结果包含外部 Cookie 用户主体对象。如果当前不存在外部 Cookie 身份验证信息，则结果返回 <c>null</c>。</returns>
		public Task<ClaimsPrincipal> GetExternalCookiePrincipalAsync()
		{
			// 执行验证
			return AuthenticationManager.AuthenticateAsync(IdentityOptions.Cookies.ExternalCookieAuthenticationScheme);
		}

		/// <summary>
		///     异步使用外部 Cookie 信息登录到应用程序 Cookie。
		/// </summary>
		/// <param name="properties">验证相关的信息字典。</param>
		/// <returns>表示异步操作的任务。结果包含一个值，如果登录成功，返回 true；否则返回 false。</returns>
		public async Task<bool> SignInForExternalCookieAsync(AuthenticationProperties properties = null)
		{
			var principal = await GetExternalCookiePrincipalAsync();

			if (principal == null)
			{
				return false;
			}

			await
				AuthenticationManager.SignInAsync(IdentityOptions.Cookies.ApplicationCookieAuthenticationScheme, principal,
					properties);
			return true;
		}


		/// <summary>
		///     注销当前应用程序上下文的所有信息。
		/// </summary>
		public async Task SignOutAsync()
		{
			await SignOutForApplicationCookieAsync();
		}

		/// <summary>
		///     注销当前应用程序上下文的应用程序 Cookie 信息。
		/// </summary>
		public Task SignOutForApplicationCookieAsync()
		{
			return AuthenticationManager.SignOutAsync(IdentityOptions.Cookies.ApplicationCookieAuthenticationScheme);
		}
	}
}