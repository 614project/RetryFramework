namespace RetryFramework.Resource;

/// <summary>
/// 프레임워크에 사용되는 자원임을 명시하는 클래스입니다. 자원은 항상 GC에 의해 안전하게 해제되어야만 합니다.
/// </summary>
public abstract class Source : IDisposable
{
    internal IntPtr _pointer { get; private set; } = IntPtr.Zero;

    public abstract void Dispose();
    ~Source() => Dispose();
}
