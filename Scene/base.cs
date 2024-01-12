using RetryFramework.Events;
using RetryFramework.SDL2;

namespace RetryFramework.Scene;

public abstract class RetryScene : Prepare, Release, Update
{
    public abstract void Prepare();
    public abstract void Release();
    public abstract void Update();
    internal abstract void Draw(ref SDL.SDL_Rect win_size);
}