using RetryFramework.SDL2;

namespace RetryFramework;

/// <summary>
/// 화면에 어떤 도형이나 텍스쳐를 그리는 렌더러입니다.
/// </summary>
public class Renderer
{


    ~Renderer() => SDL.SDL_DestroyRenderer(_pointer);
    //internal
    internal IntPtr _pointer { get; private set; }
    internal Renderer(IntPtr pointer)
    {
        _pointer = pointer;
    }
    //public

    /// <summary>
    /// 텍스쳐를 그립니다.
    /// </summary>
    /// <returns></returns>
    //public bool DrawTexture() => SDL.SDL_RenderCopyExF(this.__pointer,);
}
