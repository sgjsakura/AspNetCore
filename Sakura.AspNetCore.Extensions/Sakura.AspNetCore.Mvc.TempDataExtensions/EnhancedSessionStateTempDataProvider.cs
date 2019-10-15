#if !NETCOREAPP3_0

using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Sakura.AspNetCore.Mvc
{
	/// <summary>
	///     Enhanced session-state based temp data provider which can be used to save and load complex data objects.
	/// </summary>
	public class EnhancedSessionStateTempDataProvider : SessionStateTempDataProvider
	{



		/// <summary>
		///     Initialize an new instance with required services.
		/// </summary>
		/// <param name="objectSerializer">The required object serializer service.</param>
		public EnhancedSessionStateTempDataProvider(IObjectSerializer objectSerializer)
		{
			ObjectSerializer = objectSerializer;
		}

		/// <summary>
		///     The object serializer service object used by the provider.
		/// </summary>
		[PublicAPI]
		protected IObjectSerializer ObjectSerializer { get; }

		/// <summary>
		///     Load temp data dictionary from the specified <see cref="HttpContext" /> object.
		/// </summary>
		/// <param name="context">The <see cref="HttpContext" /> object.</param>
		/// <returns>The loaded temp data dictionary. If no temp data is found, this method will return <c>null</c>.</returns>
		public override IDictionary<string, object> LoadTempData(HttpContext context)
		{
			var baseResult = base.LoadTempData(context);

			return baseResult?.ToDictionary(item => item.Key, item => ObjectSerializer.Deserialize(item.Value));
		}

		/// <summary>
		///     Save temp data dictionary to the specified <see cref="HttpContext" /> object.
		/// </summary>
		/// <param name="context">The <see cref="HttpContext" /> object.</param>
		/// <param name="values">The temp data dictionary to be saving.</param>
		public override void SaveTempData(HttpContext context, IDictionary<string, object> values)
		{
			if (values == null)
			{
				base.SaveTempData(context, null);
				return;
			}

			var newDic = values.ToDictionary(item => item.Key, item => ObjectSerializer.Serialize(item.Value));
			base.SaveTempData(context, newDic);
		}
	}
}

#endif