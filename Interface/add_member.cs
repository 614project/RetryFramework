using RetryFramework.Objects;

namespace RetryFramework.Interface;

internal interface QuickAddObject
{
    public bool AddMember<T>(Action<T>? ready = null, int? z = null) where T : RetryObject;
    public bool AddMember(RetryObject obj,int? z = null);
}
