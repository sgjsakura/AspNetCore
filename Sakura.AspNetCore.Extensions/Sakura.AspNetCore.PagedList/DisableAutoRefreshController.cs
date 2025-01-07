using System;

namespace Sakura.AspNetCore;

/// <summary>
///     Controller object used to disable auto refreshing. This class cannot be inherited.
/// </summary>
internal sealed class DisableAutoRefreshController<T> : IDisposable
{
	/// <summary>
	///     Initialize a new instance.
	/// </summary>
	/// <param name="cacheObject">The master <see cref="Cacheable{T}" /> object.</param>
	public DisableAutoRefreshController(Cacheable<T> cacheObject)
	{
		CacheObject = cacheObject;
		CacheObject.AddAutoRefreshControl();
	}

	/// <summary>
	///     Get the master <see cref="Cacheable{T}" /> object for this object.
	/// </summary>
	private Cacheable<T> CacheObject { get; }

	#region IDisposable Support

	/// <summary>
	///     Get or set a value that indicates whether this object is disposed.
	/// </summary>
	private bool IsDisposed { get; set; }

	/// <summary>
	///     Dispose the current object and free all resources.
	/// </summary>
	/// <param name="disposing">Whether unmanaged resource should be disposed.</param>
	private void Dispose(bool disposing)
	{
		if (!IsDisposed)
		{
			// Remark
			IsDisposed = true;

			if (disposing)
				CacheObject.RemoveAutoRefreshControl();
		}
	}

	/// <summary>
	///     Dispose the current object and free all resources.
	/// </summary>
	~DisableAutoRefreshController()
	{
		Dispose(false);
	}

	/// <summary>
	///     Dispose the current object and free all resources.
	/// </summary>
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	#endregion
}