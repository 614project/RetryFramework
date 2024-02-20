using RetryFramework.Events;

namespace RetryFramework;

/// <summary>
/// 소리로 재생 가능한 객체
/// </summary>
public abstract class Playable : Release, IDisposable
{
    public abstract void Release();
    public void Dispose() => Release();
    public bool IsLoad => _ptr != IntPtr.Zero;

    internal virtual IntPtr pointer => _ptr;

    private protected IntPtr _ptr = IntPtr.Zero;
}