using System.Dynamic;

namespace Sakura.AspNetCore.Localization.Internal;

/// <summary>
///     Provide common base interface for dynamic localizer services. This interface is for internal usage only and should
///     not be used in user code.
/// </summary>
public interface IDynamicLocalizerWrapper : IDynamicMetaObjectProvider
{
}