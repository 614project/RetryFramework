using RetryFramework.SDL2;

namespace RetryFramework.Resource;

/// <summary>
/// 텍스쳐입니다. 생성은 'Framework.LoadTexture' 를 사용하세요.
/// </summary>
public class Texture : Source
{
    internal SDL.SDL_Rect _rect;
    internal Texture(IntPtr Pointer)
    {
        _rect = new();
        SDL.SDL_QueryTexture(Pointer,out _,out _,out _rect.w,out _rect.h);
    }

    public override void Dispose()
    {
        SDL.SDL_DestroyTexture(this._pointer);
        GC.SuppressFinalize(this);
    }
}
