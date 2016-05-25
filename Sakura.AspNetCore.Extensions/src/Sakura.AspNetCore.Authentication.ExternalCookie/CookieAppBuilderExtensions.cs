using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Sakura.AspNetCore.Authentication
{
	/// <summary>
	///     为 ASP.NET 应用程序的 Cookie 身份验证提供辅助方法。
	/// </summary>
	public static class CookieAppBuilderExtensions
	{
		/// <summary>
		///     获取应用程序当前的 <see cref="IdentityOptions" /> 配置。
		/// </summary>
		/// <param name="app">ASP.NET 应用程序对象。</param>
		/// <returns>应用程序当前的 <see cref="IdentityOptions" /> 对象。</returns>
		public static IdentityOptions GetIdentityOptions(this IApplicationBuilder app)
		{
			return app.ApplicationServices.GetRequiredService<IOptions<IdentityOptions>>().Value;
		}

		/// <summary>
		///     配置应用程序使用默认的应用程序 Cookie 服务。
		/// </summary>
		/// <param name="app">ASP.NET 应用程序对象。</param>
		/// <returns><paramref name="app" /> 对象。</returns>
		public static IApplicationBuilder UseApplicationCookie(this IApplicationBuilder app)
		{
			return app.UseCookieAuthentication(app.GetIdentityOptions().Cookies.ApplicationCookie);
		}

		/// <summary>
		///     配置应用程序使用默认的外部 Cookie 服务。
		/// </summary>
		/// <param name="app">ASP.NET 应用程序对象。</param>
		/// <returns><paramref name="app" /> 对象。</returns>
		public static IApplicationBuilder UseExternalCookie(this IApplicationBuilder app)
		{
			return app.UseCookieAuthentication(app.GetIdentityOptions().Cookies.ExternalCookie);
		}

		/// <summary>
		///     启用 <see cref="IdentityOptions" /> 类型中支持的所有 Cookie 选项。
		/// </summary>
		/// <param name="app">ASP.NET 应用程序对象。</param>
		/// <returns><paramref name="app" /> 对象。</returns>
		public static IApplicationBuilder UseAllCookies(this IApplicationBuilder app)
		{
			var identityOptions = app.GetIdentityOptions();

			// 注意顺序
			app.UseCookieAuthentication(identityOptions.Cookies.ExternalCookie);
			app.UseCookieAuthentication(identityOptions.Cookies.TwoFactorRememberMeCookie);
			app.UseCookieAuthentication(identityOptions.Cookies.TwoFactorUserIdCookie);
			app.UseCookieAuthentication(identityOptions.Cookies.ApplicationCookie);

			return app;
		}
	}
}