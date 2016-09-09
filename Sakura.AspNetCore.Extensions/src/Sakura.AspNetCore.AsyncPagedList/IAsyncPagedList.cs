using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sakura.AspNetCore
{
	public interface IAsyncPagedList<T> : IAsyncEnumerable<T>
	{
		Task<int> CountAsync();
		Task<int> TotalCountAsync();

		int PageSize { get; }
		int PageIndex { get; }

		Task<T> GetAsync(int index);
	}
}
